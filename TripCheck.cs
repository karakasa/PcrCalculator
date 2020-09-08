using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcrCalculator
{
    public struct Segment
    {
        public string DepartureAirport;
        public string AirlineCode;
        public DateTime LocalDepartureTime;
        public DateTime LocalArrivalTime;
    }
    public struct Transfer
    {
        public string Airport;
        public string ArrivalAirlineCode;
        public string DepartureAirlineCode;
        public DateTime LocalArrivalTime;
        public DateTime LocalDepartureTime;
    }
    public enum RouteStatus
    {
        Pass,
        AdditionalDocument,
        Suspicious,
        Invalid
    }
    public class TripCheck
    {
        public TimeZoneInfo TestTimeZone;

        public string EntryPoint = null;
        public string FinalDestination = null;
        public List<Segment> Segments = new List<Segment>();
        public RouteStatus Status { get; private set; } = RouteStatus.Pass;
        private void InvalidRoute()
        {
            Status = RouteStatus.Invalid;
        }

        private void SuspiciousRoute()
        {
            if (Status != RouteStatus.Invalid)
                Status = RouteStatus.Suspicious;
        }

        private void AdditionalDocument()
        {
            if (Status == RouteStatus.Pass)
                Status = RouteStatus.AdditionalDocument;
        }
        private void AddMessage(string message)
        {
            if (string.IsNullOrEmpty(message) && Messages.Count == 0)
                return;

            Messages.Add(message);
        }
        private void CheckLastDeparture()
        {
            // HKG/TPE 不允许转机回大陆
            var lastAirport = Segments.Last().DepartureAirport;
            if (lastAirport == "HKG" || lastAirport == "TPE")
            {
                InvalidRoute();
                AddMessage("不允许经由香港与台北转机前往中国大陆。");
                return;
            }

            if (lastAirport == "HND")
            {
                InvalidRoute();
                AddMessage("羽田不执飞飞往中国大陆的航班。");
                return;
            }

            if (!DomesticDataset.DepartureCities.Contains(lastAirport))
            {
                SuspiciousRoute();
                AddMessage("最终始发地不在航班列表内，有可能是本程序尚未收录的路线。请格外确认该航班是否真的会执行。");
            }
        }
        private void CheckSchengenTransfer()
        {
            if (SchengenAirport.Contains(Segments[0].DepartureAirport))
                return;

            for (var i = 0; i < Segments.Count - 1; i++)
            {
                if (SchengenAirport.Contains(Segments[i].DepartureAirport)
                    && SchengenAirport.Contains(Segments[i + 1].DepartureAirport))
                {
                    AddMessage("不允许在申根内转机。");
                    InvalidRoute();
                    return;
                }
            }
        }
        private void CheckCanadaTransfer()
        {
            // 直飞
            if (Segments.Count == 1)
                return;

            // 不涉及加拿大
            if (!Segments.Any(seg => IsCanadaAirport(seg.DepartureAirport)))
                return;

            // 不允许2转，首段必须 AC 执飞，YYZ不可换航站楼
            if (Segments.Count == 2 && Segments[0].AirlineCode == "AC" && Segments[1].DepartureAirport == "YVR")
            {
                var airline = Segments[1].AirlineCode;
                if (airline == "AC" || airline == "CA" || airline == "MU" || airline == "CZ" || airline == "MF" || airline == "HU")
                {
                    AddMessage("初步判定无需签证即可在加拿大转机，请注意 CTP 的其他条件。");
                    return;
                }
            }

            // 加拿大始发
            if (IsCanadaAirport(Segments[0].DepartureAirport))
                return;

            for (var i = 0; i < Segments.Count - 1; i++)
            {
                if (IsCanadaAirport(Segments[i].DepartureAirport) && IsCanadaAirport(Segments[i + 1].DepartureAirport))
                {
                    AddMessage("不允许在加拿大国内转机。");
                    InvalidRoute();
                    return;
                }
            }

            RequireVisa("加拿大签证");
        }

        private void RequireVisa(string country)
        {
            AddMessage($"需要 {country} 或其他旅行文件。");
            AdditionalDocument();
        }
        private static bool IsCanadaAirport(string code) => code == "YVR" || code == "YYZ" || code == "YUL";
        private void CheckTransfers()
        {
            var transfers = GetTransfers().ToArray();

            foreach (var it in transfers)
                CheckTransfer(it);

            if (BaggageCount > 0 && Status != RouteStatus.Invalid)
            {

                AddMessage("");

                foreach (var transfer in transfers)
                {
                    var header = $"行李直挂: {transfer.ArrivalAirlineCode} -> {transfer.DepartureAirlineCode} ";
                    var message = "";

                    if (transfer.ArrivalAirlineCode == "TP" && transfer.DepartureAirlineCode == "JD")
                    {
                        message = "不可行。需向首都航空声明，走行李丢失流程。";
                    }
                    else
                    {
                        switch (AirlineDataset.GetIetState(transfer.ArrivalAirlineCode, transfer.DepartureAirlineCode))
                        {
                            case InterlineStatus.LackOfData:
                                message = "数据不足";
                                break;
                            case InterlineStatus.Ok:
                                message = "可行（联盟内）";
                                break;
                            case InterlineStatus.OkOutOfAlliance:
                                message = "理论上可行（联盟外）";
                                break;
                            case InterlineStatus.UnknownOrNo:
                                message = "不可行 / 未知";
                                break;
                        }
                    }

                    AddMessage(header + message);
                }
            }
        }
        private static readonly string[] AllowLongTransfer = new string[]
        {
            "KUL"
        };
        private static readonly string[] SameDayOnlyTransfer = new string[]
        {
            "NRT", "KIX", "ADD"
        };
        private static readonly string[] ForbiddenTransfer = new string[]
        {
            "SIN", "BKK", "AKL", "SYD"
        };

        private void CheckTransfer(Transfer transfer)
        {
            if (ForbiddenTransfer.Contains(transfer.Airport))
            {
                InvalidRoute();
                AddMessage($"不可在 {transfer.Airport} 转机。");
                return;
            }

            var sameDay = transfer.LocalDepartureTime.Date == transfer.LocalArrivalTime.Date;
            var duration = (transfer.LocalDepartureTime - transfer.LocalArrivalTime).TotalSeconds;

            if (duration <= 0)
            {
                InvalidRoute();
                AddMessage($"{transfer.Airport} 转机时后程起飞比前程降落更早。");
                return;
            }

            var mct = GetMCT(transfer.Airport, transfer.ArrivalAirlineCode, transfer.DepartureAirlineCode);
            if (mct > duration / 60)
            {
                SuspiciousRoute();
                AddMessage($"{transfer.Airport} 转机时间可能不足。建议至少 {mct} 分钟。");
            }

            if (duration >= 86400 && !AllowLongTransfer.Contains(transfer.Airport))
            {
                InvalidRoute();
                AddMessage($"{transfer.Airport} 转机须在 24 小时内。");
                return;
            }

            if (duration >= 86400 * 2 && transfer.Airport == "KUL")
            {
                InvalidRoute();
                AddMessage($"{transfer.Airport} 转机须在 48 小时内。");
                return;
            }

            if (transfer.Airport == "AUH")
            {
                if (duration >= 6 * 3600)
                {
                    InvalidRoute();
                    AddMessage($"AUH 转机须在 6 小时内。");
                    return;
                }
                else
                {
                    AddMessage("AUH 转机需要联程票。");
                    return;
                }
            }

            if (duration >= 12 * 3600 && transfer.Airport == "RUH")
            {
                InvalidRoute();
                AddMessage($"RUH 转机须在 6 小时内。");
                return;
            }

            if (!sameDay && SameDayOnlyTransfer.Contains(transfer.Airport))
            {
                InvalidRoute();
                AddMessage($"{transfer.Airport} 转机须在同日。");
                return;
            }

            switch (transfer.Airport)
            {
                case "YVR":
                    if (!sameDay)
                        AddMessage("非同日转机需在机场 Fairmont 有预订，并且不能离开机场。");
                    break;
                case "SVO":
                    if(Segments.Count != 2 || transfer.DepartureAirlineCode != "SU"
                         || transfer.ArrivalAirlineCode != "SU"
                         || !AirportInEurope.Contains(Segments[0].AirlineCode))
                    {
                        AddMessage("不符合俄罗斯转机要求。");
                        InvalidRoute();
                        return;
                    }    
                    break;
                case "LHR":
                    RequireVisa("美国/加拿大/英国签证");
                    break;
                case "DOH":
                    if (Segments.Count != 2)
                    {
                        AddMessage("不符合 EK 转机要求。");
                        InvalidRoute();
                        return;
                    }
                    break;
                case "MNL":
                    AddMessage("菲律宾有可能不能转机。");
                    SuspiciousRoute();
                    break;
                case "PNH":
                case "KOS":
                    RequireVisa("柬埔寨商务签");
                    break;
            }
        }

        private static readonly string[] AirportInEurope = new string[]
        {
            "AGP", "ALC", "AMS", "ARN", "ATH", "AYT", "BCN", "BEG",
            "BLQ", "BRU", "BUD", "CDG", "CPH", "DRS", "DUB", "DUS",
            "FCO", "FRA", "GVA", "HAJ", "HAM", "HEL", "IST", "LCA",
            "LHR", "LIS", "LJU", "LYS", "MAD", "MRS", "MUC", "MXP",
            "NAP", "NCE", "OSL", "OTP", "PRG", "RIX", "SKG", "SOF",
            "STR", "SXF", "TFS", "TIV", "TLL", "VCE", "VIE", "VLC",
            "VNO", "VRN", "WAW", "ZAG", "ZRH"
        };

        private static readonly string[] SchengenAirport = new string[]
        {
            "CDG", "FRA", "AMS", "HEL", "ZRH", "BRU", "LIS", "CPH",
            "ARN", "MAD", "VIE", "ATH", "MXP", "WAW"
        };
        private IEnumerable<Transfer> GetTransfers()
        {
            for (var i = 1; i < Segments.Count; i++)
            {
                yield return new Transfer()
                {
                    Airport = Segments[i].DepartureAirport,
                    ArrivalAirlineCode = Segments[i - 1].AirlineCode,
                    DepartureAirlineCode = Segments[i].AirlineCode,
                    LocalArrivalTime = Segments[i - 1].LocalArrivalTime,
                    LocalDepartureTime = Segments[i].LocalDepartureTime
                };
            }
        }
        private int GetMCT(string airport, string from, string to)
        {
            switch (airport)
            {
                case "YYZ":
                    if (to == "MU")
                        return to == "AC" ? 150 : 120;
                    else if (to == "HU")
                        return 120;
                    return 90;
                case "YVR":
                    if(BaggageCount > 0)
                        AddMessage("YVR 转机行李非直挂时间更长。");

                    if (to == "3U")
                        return 120;
                    if (to == "MF")
                        return 125;

                    return 90;

                case "ICN":
                    if (from == "KE" && to == "KE")
                        return 45;

                    return 90;

                case "NRT":
                    if (to == "MU")
                        return 90;

                    AddMessage("NRT 转机与航站楼有很大关系，最长可达 130 分钟。");
                    return 90;

                case "KIX":
                    return 90;
            }

            if (airport == "MAD" || airport == "CDG")
                return 90;

            if (SchengenAirport.Contains(airport))
                return 60;

            return 0;
        }

        private void CheckAa()
        {
            if (Segments[0].AirlineCode == "AA")
            {
                if (Segments.Count != 2 || Segments[1].AirlineCode != "CA" || Segments[1].DepartureAirport != "MAD" || Segments[0].DepartureAirport != "DFW")
                {
                    SuspiciousRoute();
                    AddMessage("AA 可能会拒发登机牌。");
                    return;
                }
            }
        }

        public List<string> Messages = new List<string>();
        public void Check()
        {
            if (Segments.Count == 0)
                return;

            AddMessage("请注意，本工具并不会检查航线是否存在以及航线时间是否正确。");

            CheckPCRRequirement();
            CheckPCRReportRequirement();
            CheckAa();
            CheckBaggageAllowance();
            CheckTransfers();
            CheckCanadaTransfer();
            CheckSchengenTransfer();
            CheckLastDeparture();
            var msg = DomesticDataset.GetDestinationRegulation(EntryPoint, FinalDestination);
            if (!string.IsNullOrEmpty(msg))
                AddMessage(msg);
        }

        private void CheckPCRReportRequirement()
        {
            foreach (var it in Segments)
                if (RequirePCRReport.Contains(it.DepartureAirport) || RequirePCRReport.Contains(it.AirlineCode))
                    AddMessage($"{it.DepartureAirport} 可能需要核酸报告。");

        }

        private static readonly string[] RequirePCRReport = new string[]
        {
            "ICN", "IST", "EK", "TP"
        };
        private static readonly string[] OneBaggageAirports = new string[]
        {
            "IST", "SVO", "KUL", "DXB", "AUH", "SIN", "DOH",
            "MNL", "PNH", "KOS", "HAN", "KWI", "BKK", "RUH"
        };
        private static bool IsOneFreeBaggage(string airport)
        {
            if (AirportInEurope.Contains(airport))
                return true;

            if (OneBaggageAirports.Contains(airport))
                return true;

            return false;
        }
        private static bool IsUSAirlines(string airline) => airline == "UA" || airline == "DL" || airline == "AA";
        private void CheckBaggageAllowance()
        {
            if (BaggageCount == 0)
                return;

            for (var i = 0; i < Segments.Count; i++)
            {
                var from = Segments[i].DepartureAirport;
                var to = (i == Segments.Count - 1) ? GetCode(EntryPoint) : Segments[i + 1].DepartureAirport;

                if (BaggageCount > 1 && IsOneFreeBaggage(from))
                {
                    AddMessage($"{from} -> {to} 经济舱默认只有 1 件免费行李。");
                }
                else if ((IsUSAirlines(Segments[i].AirlineCode) && AirportInEurope.Contains(to)))
                {
                    if(BaggageCount > 1)
                    {
                        AddMessage($"{from} -> {to} Main Cabin 经济舱默认只有 1 件免费行李，Basic Economy 无免费行李。");
                    }
                    else if(BaggageCount == 1)
                    {
                        AddMessage($"{from} -> {to} 如果是 Basic Economy 则无免费行李。");
                    }
                }
            }
        }

        private static string GetCode(string dest)
        {
            return dest.Substring(dest.IndexOf("(") + 1, 3);
        }

        public int BaggageCount = 0;
        private void CheckPCRRequirement()
        {
            if ((Segments[0].DepartureAirport == "NRT" || Segments[0].DepartureAirport == "KIX") && Segments.Count == 1)
            {
                AddMessage("日本始发仅需要 14 天连续打卡健康码。");
                return;
            }

            var required = false;
            var earliestSubmitTime = new DateTime(2020, 1, 1, 0, 0, 1);

            foreach(var it in Segments)
            {
                if (!AirportDataset.TryGetAirport(it.DepartureAirport, out var airport))
                    continue;

                if (airport.PCRInAdvance == -1 || airport.StartTime > it.LocalDepartureTime)
                    continue;

                var localReportTime = it.LocalDepartureTime - new TimeSpan(airport.PCRInAdvance, 0, 0, 0);
                var localEarliestTime = new DateTime(localReportTime.Year, localReportTime.Month, localReportTime.Day, 0, 0, 1);

                var earliestTime = TimeZoneInfo.ConvertTime(localEarliestTime, airport.TimeZone, TestTimeZone);
                if (earliestTime > earliestSubmitTime)
                {
                    required = true;
                    earliestSubmitTime = earliestTime;
                }
            }

            if (required)
            {
                if(earliestSubmitTime > Segments[0].LocalDepartureTime)
                {
                    AddMessage("核酸报告时间无法赶上第一程航班。");
                    InvalidRoute();
                    return;
                }
                AddMessage("核酸码需要报告出具时间为 " + earliestSubmitTime.ToString("yyyy/MM/dd HH:mm") + " 后的报告。");
            }
        }
    }
}

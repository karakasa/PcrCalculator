using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PcrCalculatorLib
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
        public TimeZoneInfo PcrTimezone;
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
        private void CheckOtherDomesticTransfer()
        {
            var calculatedUs = false;

            for (var i = 1; i < Segments.Count; i++)
            {
                if (AirportInUS.Contains(Segments[i].DepartureAirport))
                {
                    if (IsTransferForbiddenInUs(Segments[0].DepartureAirport))
                    {
                        InvalidRoute();
                        AddMessage("14 天内到访过热点国家，不允许在美国转机。");
                        return;
                    }
                    else
                    {
                        RequireVisa("美国签证");
                        calculatedUs = true;
                        break;
                    }
                }
            }

            for (var i = 1; i < Segments.Count - 1; i++)
            {
                var from = Segments[i].DepartureAirport;
                var to = Segments[i + 1].DepartureAirport;

                if (AirportInUS.Contains(from) || AirportInUS.Contains(to))
                {
                    if (!calculatedUs)
                    {
                        if (IsTransferForbiddenInUs(Segments[0].DepartureAirport))
                        {
                            InvalidRoute();
                            AddMessage("14 天内到访过热点国家，不允许在美国转机。");
                            return;
                        }
                        else
                        {
                            RequireVisa("美国签证");
                            calculatedUs = true;
                        }
                    }
                }
                else if (IsWithinSameCountry(from, to))
                {
                    InvalidRoute();
                    AddMessage($"不允许 {from} -> {to} 的国内转机。");
                    return;
                }
            }
        }

        private static bool IsTransferForbiddenInUs(string code) => SchengenAirport.Contains(code) || USForbiddenTransferOrigin.Contains(code);
        private static readonly string[] AirportInJapan = new string[] { "NRT", "HND", "KIX" };
        private static readonly string[] AirportInKorea = new string[] { "ICN", "CJU" };
        private static readonly string[] AirportInUAE = new string[] { "DXB", "AUH" };
        private static readonly string[] AirportInUS = new string[] { "SEA", "SFO", "LAX", "JFK", "DTW", "ATL", "ORD", "SLC", "DFW" };
        private static readonly string[] USForbiddenTransferOrigin = new string[]
        {
            "IKA", "LHR", "GIG", "DUB"
        };

        private void CheckStartSegmentsInOneCountrry()
        {
            if (Segments.Count > 1 && IsWithinSameCountry(Segments[0].DepartureAirport, Segments[1].DepartureAirport))
            {
                AddMessage("请不要包含国内段航班，分析结果有可能有错。");
                AddMessage("");
            }
        }
        private static bool IsWithinSameCountry(string a, string b)
        {
            return
                (AirportInJapan.Contains(a) && AirportInJapan.Contains(b))
                || (AirportInKorea.Contains(a) && AirportInKorea.Contains(b))
                || (AirportInUAE.Contains(a) && AirportInUAE.Contains(b))
                || (AirportInUS.Contains(a) && AirportInUS.Contains(b));
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
                if (airline == "AC" || airline == "CA" || airline == "MU" || airline == "CZ" || airline == "MF" || airline == "HU" || airline == "3U")
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
                var trouble = false;
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
                            case InterlineStatus.Unknown:
                                message = "未知";
                                break;
                            case InterlineStatus.Ok:
                                message = "可行（联盟内）";
                                break;
                            case InterlineStatus.OkOutOfAlliance:
                                message = "理论可行（联盟外）";
                                break;
                            case InterlineStatus.No:
                                message = "不可行";
                                trouble = true;
                                break;
                        }
                    }

                    AddMessage(header + message);
                }

                if (trouble)
                    AddMessage("请注意不可行不一定意味着一定不能直挂，有可能可以由地勤代捞行李或像边境部门求情。");
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
                    if (!sameDay && transfer.LocalDepartureTime.Hour >= 4)
                        AddMessage("过夜转机需在机场 Fairmont 有预订，并且不能离开机场。");
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
                case "HKG":
                    if (transfer.ArrivalAirlineCode != transfer.DepartureAirlineCode || transfer.ArrivalAirlineCode != "CX")
                    {
                        InvalidRoute();
                        AddMessage("不满足香港转机要求。");
                    }
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
            AddMessage("");

            CheckStartSegmentsInOneCountrry();
            CheckPCRRequirement();
            CheckPCRReportRequirement();
            CheckAa();
            CheckBaggageAllowance();
            CheckTransfers();
            CheckCanadaTransfer();
            CheckSchengenTransfer();
            CheckOtherDomesticTransfer();
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

            if (BaggageCount >= 3)
                AddMessage("请注意 3+ 行李的超额行李费。");

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
        private static readonly TimeZoneInfo WestCoastTimeZone = TimeZoneConverter.TZConvert.GetTimeZoneInfo("Pacific Standard Time");

        private void CheckPCRRequirement()
        {
            if (Segments.Count == 1 
                && AirportInJapan.Contains(Segments[0].DepartureAirport) 
                && Segments[0].LocalDepartureTime >= new DateTime(2020, 9, 25, 0, 0, 1))
            {
                AddMessage("日本直飞无需提供核酸码/14天打卡健康码，但是需要提供在指定核酸测试机构的纸质测试报告，并需持复印件供航空公司留存。详情请参阅资源一栏。");
                return;
            }

            var required = false;
            var earliestSubmitTime = new DateTime(2020, 1, 1, 0, 0, 1);

            var isUsOrigin = AirportInUS.Contains(Segments[0].DepartureAirport);
            var hasOriginAirport = AirportDataset.TryGetAirport(Segments[0].DepartureAirport, out var originAirport);
            var testTimezone = isUsOrigin ? WestCoastTimeZone : PcrTimezone;

            foreach (var it in Segments)
            {
                if (!AirportDataset.TryGetAirport(it.DepartureAirport, out var airport))
                    continue;

                if (airport.PCRInAdvance == -1 || airport.StartTime > it.LocalDepartureTime)
                    continue;

                var airportTimeInLocal = TimeZoneInfo.ConvertTime(it.LocalDepartureTime, airport.TimeZone, testTimezone);
                var pcrTimeInLocal = airportTimeInLocal - new TimeSpan(airport.PCRInAdvance, 0, 0, 0);
                var earliestTime = new DateTime(pcrTimeInLocal.Year, pcrTimeInLocal.Month, pcrTimeInLocal.Day, 0, 1, 0);

                if (earliestTime > earliestSubmitTime)
                {
                    required = true;
                    earliestSubmitTime = earliestTime;
                }
            }

            if (required)
            { 
                if (AirportInUS.Contains(Segments[0].DepartureAirport) && Segments[0].LocalDepartureTime < new DateTime(2020, 9, 15, 0, 0, 0))
                {
                    AddMessage("2020/09/15 前美国始发颁发的 5 日核酸码只要有效，就可以在 3 日核酸码地区转机。但是部分机场（比如韩国）可能会额外要求 3 日内核酸报告。");
                }

                AddMessage("核酸码需要报告出具当地时间为 " + earliestSubmitTime.ToString("yyyy/MM/dd") + " 及以后的报告。");
                if (isUsOrigin)
                {
                    AddMessage("请注意，美国始发核酸报告出具时间会统一视作美西时间。");
                }

                if (hasOriginAirport)
                {
                    earliestSubmitTime = TimeZoneInfo.ConvertTime(earliestSubmitTime, PcrTimezone, originAirport.TimeZone);

                    if (earliestSubmitTime > Segments[0].LocalDepartureTime)
                    {
                        AddMessage("核酸报告时间无法赶上第一程航班。");
                        InvalidRoute();
                        return;
                    }

                    if ((Segments[0].LocalDepartureTime - earliestSubmitTime).TotalSeconds < 86400)
                    {
                        SuspiciousRoute();
                        AddMessage("报告出具时间到乘机不到 24 小时。有核酸码不能准时审核通过的风险。");
                    }
                }
                else
                {
                    AddMessage("出发机场未知，无法判定核酸报告时间情况。");
                }
            }
            else
            {
                AddMessage("不需要核酸码，请注意打小飞机。");
            }
        }
    }
}

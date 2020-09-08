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
        public List<Segment> Segments = new List<Segment>();
        private RouteStatus _status = RouteStatus.Pass;
        private void InvalidRoute()
        {
            _status = RouteStatus.Invalid;
        }

        private void SuspiciousRoute()
        {
            if (_status != RouteStatus.Invalid)
                _status = RouteStatus.Suspicious;
        }

        private void AdditionalDocument()
        {
            if (_status == RouteStatus.Pass)
                _status = RouteStatus.AdditionalDocument;
        }
        private void AddMessage(string message)
        {

        }
        private void CheckLastDeparture()
        {
            // HKG/TPE 不允许转机回大陆
            var lastAirport = Segments.Last().DepartureAirport;
            if (lastAirport == "HKG" || lastAirport == "TPE")
            {
                InvalidRoute();
                AddMessage("不允许经由香港与台北转机前往中国大陆。");
            }

            if (!DomesticDataset.DepartureCitys.Contains(lastAirport))
            {
                SuspiciousRoute();
                AddMessage("最终始发地不在航班列表内，有可能是本程序尚未收录的路线。请格外确认该航班是否真的会执行。");
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
                    var sameDay = Segments[1].LocalDepartureTime.Date == Segments[0].LocalArrivalTime.Date;
                    var duration = (Segments[1].LocalDepartureTime - Segments[0].LocalArrivalTime).TotalSeconds;

                    if (duration >= 86400)
                    {
                        InvalidRoute();
                        AddMessage("不可在加拿大超过 24 小时转机。");
                        return;
                    }

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

            RequireVisa("加拿大");
        }

        private void RequireVisa(string country)
        {
            AddMessage($"需要 {country} 签证或其他旅行文件。");
            AdditionalDocument();
        }
        private static bool IsCanadaAirport(string code) => code == "YVR" || code == "YYZ" || code == "YUL";
        private void CheckTransfers()
        {
            foreach (var it in GetTransfers())
                CheckTransfer(it);
        }
        private void CheckTransfer(Transfer transfer)
        {

        }
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
    }
}

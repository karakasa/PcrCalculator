using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcrCalculatorLib
{
    public static class DomesticDataset
    {
        public static readonly string[] DepartureCities = new string[]
        {
            "JFK",
            "SEA",
            "DTW",
            "SFO",
            "LAX",
            "YYZ",
            "YVR",
            "ICN",
            "CJU",
            "NRT",
            "KIX",
            "KUL",
            "PNH",
            "KOS",
            "DXB",
            "AUH",
            "DOH",
            "RUH",
            "LHR",
            "CDG",
            "FRA",
            "AMS",
            "ZRH",
            "HEL",
            "BRU",
            "LIS",
            "CPH",
            "ARN",
            "MAD",
            "VIE",
            "ATH",
            "MSQ",
            "IST",
            "MXP",
            "WAW",
            "SVO",
            "ADD",
            "CAI",
            "NBO"
        };

        public static readonly string[] EntryPoints = new string[]
        {
            "上海 (PVG)",
            "北京 (PEK)",
            "广州 (CAN)",
            "厦门 (XMN)",
            "成都 (CTU)",
            "南京 (NKG)",
            "杭州 (HGH)",
            "青岛 (TAO)",
            "沈阳 (SHE)",
            "天津 (TSN)",
            "威海 (WEH)",
            "长春 (CGQ)",
            "深圳 (SZX)",
            "西安 (XIY)",
            "福州 (FOC)",
            "哈尔滨 (HRB)",
            "大连 (DLC)",
            "常州 (CZX)",
            "昆明 (KMG)"
        };

        public static readonly string[] FinalDestinations = new string[]
        {
            "其他",
            "山东",
            "辽宁",
            "江浙沪皖"
        };

        public static string GetDestinationRegulation(string entry, string destination)
        {
            if (destination == "山东")
            {
                return "入境隔离 14 天，进入山东后集中隔离 2 天测试核酸，之后按地方规定居家观察若干天。";
            }

            if (destination == "辽宁" && entry != "沈阳 (SHE)")
            {
                return "入境隔离 14 天，进入辽宁省后非本省口岸入境的需额外隔离 14 天。";
            }

            if (destination == "江浙沪皖" && entry == "上海 (PVG)")
            {
                return "可在上海隔离 3 天后，回户籍地再集中隔离 11 天；居住在上海本地的，可集中隔离 7 天后再居家隔离 7 天。";
            }

            return null;
        }
    }
}

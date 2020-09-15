using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcrCalculatorLib
{
    public struct Airline
    {
        public string FriendlyName;
        public string Code;
        public string[] IET;

        public Airline(string code, string name, string[] iet)
        {
            FriendlyName = name;
            Code = code;
            IET = iet;
        }
    }
    public static class AirlineDataset
    {
        public static readonly string[][] Alliance = new string[][]
        {
            new string[]
            {
                "UA", "LH", "AC", "TG", "SK", "NH", "NZ", "SQ", "OS", "LO", "OZ", "OU", "TP", "LX", "SA", "CA", "MS", "TK", "SN", "A3", "ET", "CM", "AV", "ZH", "BR", "AI"
            },
            new string[]
            {
                "DL", "AF", "AM", "KE", "OK", "KL", "SU", "UX", "KQ", "AZ", "RO", "VN", "MU", "CI", "SV", "ME", "AR", "MF", "GA"
            },
            new string[]
            {
                "CX", "AA", "BA", "QF", "AY", "IB", "LA", "JL", "RJ", "S7", "MH", "QR", "UL"
            }
        };

        public static readonly Airline[] InterlineData = new Airline[]
        {
            new Airline("UA", "美联航", new string[]{ "AA", "AC", "AD", "AF", "AH", "AI", "AM", "AR", "AS", "AV", "AX", "AY", "AZ", "A3", "BA", "BB", "BE", "BI", "BM", "BR", "BT", "BW", "B7", "CA", "CI", "CM", "CX", "CZ", "C5", "DL", "EH", "EI", "EN", "ET", "EV", "EW", "EY", "FB", "FI", "FJ", "FM", "FW", "GA", "GF", "GK", "G3", "G7", "HA", "HO", "HU", "HX", "IB", "IQ", "JJ", "JL", "JP", "JQ", "KA", "KC", "KE", "KF", "KG", "KL", "KM", "KQ", "KU", "KX", "LA", "LG", "LH", "LM", "LO", "LP", "LR", "LX", "LY", "MD", "ME", "MF", "MH", "MI", "MK", "MQ", "MS", "MU", "NH", "NQ", "NU", "NX", "NZ", "OA", "OK", "OM", "OO", "OS", "OU", "OZ", "PG", "PK", "PR", "PS", "PX", "PZ", "QF", "RO", "SA", "SK", "SN", "SQ", "SU", "S4", "S5", "S7", "TA", "TG", "TJ", "TK", "TP", "T0", "UA", "UK", "UL", "UN", "UP", "UX", "VA", "VK", "VN", "VS", "VT", "VW", "WK", "WM", "WS", "WY", "XF", "XL", "XQ", "ZH", "2K", "2V", "3K", "3M", "4B", "4C", "4Q", "4U", "7C", "7H", "9B", "9K", "9W" }),
            new Airline("DL", "达美", new string[]{ "AC", "AE", "AF", "AH", "AI", "AM", "AR", "AS", "AT", "AV", "AY", "AZ", "A3", "A5", "A9", "BA", "BB", "BP", "BR", "BT", "B2", "CI", "CM", "CX", "CZ", "EG", "EI", "ET", "FB", "FI", "FJ", "FM", "FZ", "GA", "GF", "GK", "GU", "G3", "HA", "HV", "HX", "HY", "IB", "IC", "JJ", "JK", "JL", "JO", "JU", "J2", "KA", "KC", "KE", "KF", "KL", "KM", "KQ", "KX", "LA", "LG", "LH", "LJ", "LO", "LP", "LR", "LX", "LY", "ME", "MF", "MH", "MI", "MK", "MN", "MS", "MU", "NH", "NU", "NZ", "OH", "OK", "OM", "OS", "OU", "OZ", "PC", "PG", "PR", "PS", "PY", "PZ", "QF", "RJ", "RO", "SA", "SB", "SK", "SN", "SQ", "SU", "SV", "S7", "TA", "TG", "TM", "TN", "TP", "TU", "T0", "UA", "UK", "UL", "UP", "UX", "VA", "VN", "VS", "VT", "WF", "WS", "WX", "WY", "XK", "XL", "YM", "3M", "4C", "4M", "4Q", "9K", "9U", "9W" }),
            new Airline("AA", "美航", new string[]{ "AC", "AD", "AF", "AI", "AM", "AQ", "AS", "AT", "AV", "AY", "AZ", "A3", "BA", "BB", "BR", "BW", "CA", "CI", "CM", "CX", "CZ", "EC", "EG", "EI", "ET", "EY", "FB", "FI", "FJ", "GF", "GK", "G3", "HA", "HP", "HU", "IB", "JC", "JJ", "JL", "JO", "JQ", "JU", "KA", "KE", "KL", "KQ", "KX", "LA", "LF", "LH", "LO", "LP", "LR", "LX", "LY", "ME", "MF", "MH", "MS", "MU", "NH", "NU", "NZ", "OA", "OB", "OK", "OM", "OS", "OU", "OZ", "PR", "PS", "PZ", "QF", "QR", "RJ", "SA", "SK", "SN", "SQ", "SU", "SV", "S7", "TA", "TG", "TJ", "TK", "TN", "TP", "UA", "UK", "UL", "US", "VN", "VS", "VT", "VY", "WS", "WY", "XL", "YX", "3M", "4B", "4C", "4M", "4O", "9B", "9K", "9X" }),
            new Airline("AS", "阿拉斯加", new string[]{ "AA", "AC", "AF", "AM", "AV", "AZ", "BA", "BF", "BR", "B6", "CI", "CM", "CX", "CZ", "DE", "DL", "EI", "EK", "ET", "FI", "FJ", "FZ", "HA", "HP", "HU", "HX", "IG", "JL", "KE", "KL", "LA", "LO", "LP", "LR", "MF", "MH", "MW", "NH", "NZ", "OZ", "PS", "QF", "QR", "SA", "SK", "SQ", "S4", "TA", "TK", "TZ", "UA", "VA", "VW", "WS", "YV", "3M", "4C", "7H", "9K", "9X" }),
            new Airline("AC", "加航", new string[]{ "AA", "AD", "AF", "AH", "AI", "AM", "AR", "AS", "AT", "AV", "AZ", "A3", "BA", "BG", "BR", "BT", "BW", "CA", "CI", "CM", "CX", "CZ", "DL", "EI", "EK", "EN", "ET", "EW", "EY", "FB", "FI", "FJ", "GA", "GF", "GK", "G3", "HA", "HO", "HX", "IB", "JJ", "JL", "JQ", "JU", "JV", "KA", "KE", "KL", "KM", "KQ", "KU", "KX", "LA", "LG", "LH", "LI", "LO", "LR", "LX", "ME", "MH", "MK", "MS", "MU", "NH", "NZ", "OA", "OK", "OS", "OU", "OZ", "PR", "PS", "PZ", "QF", "QK", "QR", "RJ", "SA", "SB", "SK", "SN", "SQ", "SU", "SV", "TA", "TG", "TK", "TN", "TP", "TU", "UA", "UK", "UL", "VA", "VN", "VS", "WF", "WK", "WY", "XL", "YN", "ZH", "2K", "3K", "3M", "4C", "4M", "4O", "4U", "5T", "7C", "7F", "9M" }),
            new Airline("WS", "西捷", new string[]{ "AA", "AF", "AM", "AS", "BA", "DL", "IT", "JJ", "MF", "PB", "PS", "QF", "UA", "4C", "5T", "7F", "9M" }),
            new Airline("AM", "墨西哥", new string[]{"AA", "AC", "AD", "AE", "AF", "AR", "AS", "AV", "AZ", "BA", "BE", "CA", "CI", "CM", "CX", "CZ", "DL", "EQ", "EY", "FM", "G3", "HA", "HR", "HX", "IB", "JJ", "JL", "KA", "KE", "KL", "KQ", "LA", "LH", "LP", "LR", "LX", "LY", "ME", "MF", "MU", "NZ", "OK", "OZ", "PZ", "QF", "RG", "RO", "SA", "SK", "SQ", "SU", "SV", "TA", "TK", "TP", "T0", "UA", "UX", "VA", "VN", "VS", "VW", "WS", "ZH", "0O", "2K", "4C", "4M", "7C", "9B", "9W"}),
            new Airline("CX", "国泰（港龙）", new string[]{ "AA", "AC", "AE", "AF", "AH", "AI", "AM", "AS", "AT", "AV", "AY", "AZ", "A3", "BA", "BG", "BI", "BL", "BP", "BR", "B6", "B7", "CA", "CI", "CM", "CZ", "DL", "EI", "EK", "ET", "EW", "EY", "FI", "FJ", "FM", "FZ", "GA", "GF", "HA", "HM", "HR", "IB", "JC", "JJ", "JL", "JO", "JP", "JQ", "JU", "KA", "KC", "KE", "KL", "KM", "KQ", "KU", "LA", "LG", "LH", "LO", "LP", "LR", "LX", "LY", "ME", "MF", "MH", "MI", "MK", "MS", "MU", "NF", "NH", "NU", "NX", "NZ", "OA", "OK", "OM", "OS", "OU", "OZ", "PG", "PR", "PS", "PX", "PZ", "QF", "QR", "RA", "RJ", "SA", "SB", "SC", "SK", "SN", "SQ", "SU", "SV", "SW", "S7", "TA", "TG", "TK", "TM", "TN", "TP", "TX", "UA", "UL", "UO", "VA", "VN", "VS", "VY", "WS", "WY", "XL", "ZH", "2C", "4M", "4U", "9B" }),
            new Airline("CI", "华航", new string[]{ "AA", "AC", "AE", "AF", "AI", "AM", "AR", "AS", "AV", "AY", "AZ", "BA", "BI", "BR", "BT", "B6", "B7", "CA", "CM", "CX", "CZ", "DL", "EG", "EI", "EK", "EN", "ET", "EY", "FI", "FM", "GA", "GF", "GU", "G3", "HA", "HM", "HO", "HR", "HU", "HX", "IB", "IE", "IT", "JJ", "JL", "JO", "JP", "JU", "KA", "KE", "KL", "KQ", "K6", "LA", "LG", "LH", "LO", "LR", "LX", "LY", "ME", "MF", "MH", "MI", "MK", "MU", "NH", "NU", "NX", "NZ", "OK", "OM", "OS", "OU", "OZ", "PG", "PR", "PS", "PZ", "QF", "QR", "QV", "RA", "RJ", "RO", "SA", "SC", "SK", "SN", "SQ", "SU", "SV", "SY", "S7", "TA", "TG", "TK", "TN", "TP", "TU", "T0", "UA", "UL", "UN", "UX", "VA", "VN", "WF", "WS", "WY", "XL", "ZH", "3U", "4M", "8M", "8U", "9B", "9U", "9W" }),
            new Airline("BR", "长荣", new string[]{ "AA", "AC", "AE", "AF", "AI", "AS", "AV", "AY", "AZ", "A3", "BA", "BT", "B6", "B7", "CA", "CI", "CM", "CX", "CZ", "DL", "EK", "ET", "EW", "EY", "FB", "FM", "GA", "GF", "HA", "HO", "HR", "HU", "HX", "IB", "IE", "JL", "KA", "KE", "KL", "KQ", "LG", "LH", "LO", "LX", "LY", "MD", "MF", "MH", "MI", "MU", "NH", "NU", "NX", "NZ", "OD", "OK", "OM", "OS", "OU", "OZ", "PG", "PR", "PS", "QD", "QF", "QR", "QV", "RJ", "SA", "SC", "SK", "SN", "SQ", "SU", "SV", "TG", "TK", "TP", "UA", "UL", "VA", "VN", "WE", "WF", "WS", "WY", "ZH", "0O", "2K", "3U", "4U", "8M", "9B" }),
            new Airline("NH", "全日空", new string[]{ "AA", "AC", "AD", "AF", "AH", "AI", "AS", "AT", "AV", "AY", "AZ", "A3", "BA", "BG", "BI", "BR", "BT", "B6", "CA", "CI", "CM", "CX", "CZ", "DL", "EK", "EN", "ET", "EW", "EY", "FI", "FJ", "FM", "GA", "GF", "HA", "HO", "HR", "HU", "HX", "HY", "IB", "JD", "JJ", "JK", "JL", "KA", "KC", "KE", "KL", "KM", "KQ", "K6", "LA", "LG", "LH", "LO", "LP", "LR", "LX", "LY", "MD", "MF", "MH", "MI", "MS", "MU", "MX", "NX", "NZ", "OA", "OD", "OK", "OM", "OS", "OU", "OZ", "PG", "PR", "PZ", "QF", "QR", "QV", "RA", "SA", "SC", "SK", "SN", "SQ", "SU", "S7", "TA", "TG", "TK", "TP", "UA", "UK", "UL", "UN", "UX", "VA", "VN", "VS", "VW", "WE", "WF", "XF", "ZH", "2K", "3M", "4M", "4O", "4U", "8M", "9B" }),
            new Airline("JL", "日航", new string[]{ "AA", "AC", "AF", "AH", "AI", "AM", "AS", "AT", "AV", "AY", "AZ", "A3", "BA", "BE", "BI", "BL", "BR", "B6", "B7", "CA", "CI", "CM", "CX", "CZ", "DL", "EI", "EK", "ET", "FI", "FJ", "FM", "GA", "GF", "GK", "G3", "HA", "HR", "HU", "HY", "IB", "IE", "JC", "JJ", "JK", "JO", "JQ", "KA", "KC", "KE", "KL", "KQ", "K6", "LA", "LG", "LH", "LO", "LP", "LR", "LX", "MH", "MI", "MK", "MS", "MU", "NH", "NU", "NX", "NZ", "OA", "OK", "OM", "OS", "OU", "OZ", "PG", "PR", "PX", "PZ", "QF", "QR", "RJ", "RO", "SA", "SB", "SC", "SK", "SQ", "SU", "SV", "S7", "TA", "TG", "TK", "TN", "TP", "T0", "UA", "UB", "UK", "UL", "UX", "VJ", "VN", "WS", "XL", "ZH", "3K", "3U", "4M", "8M", "9B", "9W" }),
            new Airline("KE", "大韩", new string[]{ "AA", "AC", "AE", "AF", "AH", "AI", "AM", "AR", "AS", "AT", "AV", "AY", "AZ", "A3", "BA", "BG", "BI", "BR", "BT", "BW", "B6", "B7", "CA", "CI", "CM", "CX", "CZ", "DL", "EK", "ET", "EY", "FI", "FJ", "FM", "GA", "GF", "G3", "HA", "HO", "HP", "HR", "HU", "HY", "HZ", "IB", "IE", "JJ", "JK", "JL", "JO", "JU", "J2", "KA", "KC", "KL", "KM", "KQ", "KU", "K6", "LA", "LG", "LJ", "LO", "LP", "LR", "LX", "LY", "ME", "MF", "MH", "MI", "MK", "MS", "MU", "NH", "NU", "NX", "NZ", "OA", "OK", "OM", "OS", "OU", "OZ", "PG", "PR", "PS", "PX", "PZ", "QF", "QR", "QV", "RA", "RJ", "RO", "SB", "SC", "SK", "SQ", "SU", "SV", "S7", "TA", "TG", "TK", "TN", "TP", "TU", "UA", "UK", "UL", "UX", "VA", "VN", "VS", "WF", "WS", "WY", "XL", "YX", "ZH", "3B", "3U", "4C", "4M", "8M", "9B" }),
            new Airline("OZ", "韩亚", new string[]{ "AA", "AC", "AE", "AF", "AH", "AI", "AM", "AS", "AT", "AV", "AY", "AZ", "A3", "BA", "BG", "BI", "BR", "BX", "B6", "B7", "CA", "CI", "CM", "CX", "CZ", "DL", "EK", "EQ", "ET", "EY", "FI", "FJ", "FM", "FV", "GA", "GF", "HA", "HO", "HR", "HU", "HX", "HY", "HZ", "IB", "IE", "JJ", "JL", "JP", "KA", "KB", "KC", "KE", "KL", "KM", "KQ", "KU", "K6", "LA", "LG", "LH", "LO", "LP", "LR", "LX", "LY", "ME", "MF", "MH", "MI", "MK", "MS", "MU", "NF", "NH", "NX", "NZ", "OA", "OM", "OS", "OU", "PG", "PR", "PS", "PX", "PZ", "QF", "QR", "QV", "RA", "RJ", "RS", "SA", "SB", "SC", "SK", "SN", "SQ", "SU", "SV", "S7", "TA", "TG", "TK", "TN", "TP", "TU", "T0", "UA", "UK", "UL", "VA", "VN", "VR", "VS", "VY", "WF", "ZH", "2K", "3U", "8M", "9B" }),
            new Airline("7C", "济州", new string[]{ "AC", "AI", "AM", "GA", "GP", "HR", "HX", "JQ", "K6", "OM", "PG", "SQ", "SU", "S7", "UA" }),
            new Airline("LJ", "真航空", new string[]{ }),
            new Airline("BX", "釜山航空", new string[]{ }),
            new Airline("AF", "法航", new string[]{ "AA", "AC", "AD", "AE", "AH", "AI", "AM", "AR", "AS", "AT", "AV", "AY", "AZ", "A3", "A5", "A9", "BA", "BE", "BI", "BL", "BP", "BR", "BT", "BW", "B2", "CA", "CE", "CI", "CM", "CX", "CZ", "DL", "DT", "EI", "EK", "EQ", "ET", "EY", "FA", "FB", "FI", "FJ", "FM", "FZ", "GA", "GF", "GK", "GQ", "G3", "HA", "HC", "HF", "HM", "HU", "HV", "HX", "HY", "IB", "JJ", "JL", "JQ", "JU", "J2", "KA", "KC", "KE", "KL", "KM", "KP", "KQ", "KU", "K6", "LA", "LG", "LH", "LI", "LM", "LO", "LP", "LR", "LX", "LY", "MD", "ME", "MF", "MH", "MI", "MK", "MN", "MS", "MU", "NH", "NI", "NL", "NZ", "OA", "OK", "OM", "OS", "OU", "OZ", "PG", "PJ", "PK", "PR", "PS", "PW", "PX", "PZ", "QF", "QR", "QV", "RC", "RG", "RJ", "RO", "SA", "SB", "SK", "SN", "SQ", "SS", "SU", "SV", "SW", "S4", "TA", "TG", "TK", "TM", "TN", "TO", "TP", "TU", "TX", "T7", "UA", "UK", "UL", "UP", "UU", "UX", "VK", "VN", "VS", "VT", "VW", "V0", "WF", "WM", "WS", "WX", "WY", "W2", "XK", "XL", "YM", "2C", "2J", "2K", "3K", "3S", "4M", "4Z", "6I", "8M", "9B", "9D" }),
            new Airline("KL", "荷航", new string[]{ "AA", "AC", "AD", "AE", "AF", "AH", "AI", "AJ", "AM", "AR", "AS", "AT", "AV", "AY", "AZ", "A3", "A5", "A9", "BA", "BI", "BL", "BP", "BR", "BT", "BW", "B2", "CA", "CI", "CM", "CX", "CZ", "DC", "DL", "EI", "EK", "EQ", "ET", "EY", "FA", "FB", "FI", "FJ", "FM", "FZ", "GA", "GF", "GK", "GQ", "G3", "HA", "HF", "HV", "HX", "HY", "IB", "JJ", "JL", "JQ", "JU", "J2", "KA", "KC", "KE", "KF", "KM", "KP", "KQ", "KU", "K6", "LA", "LG", "LH", "LI", "LM", "LO", "LP", "LR", "LX", "LY", "MD", "ME", "MF", "MH", "MI", "MK", "MN", "MS", "MU", "MX", "NH", "NZ", "OA", "OK", "OM", "OS", "OU", "OZ", "PC", "PG", "PK", "PR", "PS", "PW", "PX", "PY", "PZ", "QF", "QI", "QR", "QV", "RC", "RG", "RJ", "RO", "SA", "SB", "SC", "SK", "SN", "SQ", "SU", "SV", "SW", "TA", "TE", "TG", "TK", "TM", "TN", "TP", "T3", "UA", "UK", "UL", "UM", "UP", "UU", "UX", "U6", "VA", "VN", "VR", "VS", "VW", "WA", "WB", "WF", "WM", "WP", "WS", "WX", "WY", "W2", "XK", "XL", "YM", "YX", "2C", "2K", "3K", "3S", "3U", "4M", "4Z", "8M", "9B", "9U" }),
            new Airline("LH", "汉莎", new string[]{ "AA", "AC", "AD", "AF", "AH", "AI", "AM", "AR", "AS", "AT", "AV", "AY", "AZ", "A3", "BA", "BI", "BL", "BP", "BR", "BT", "B2", "B6", "CA", "CI", "CM", "CX", "CZ", "DE", "DL", "DT", "EK", "EN", "ET", "EW", "EY", "FB", "FI", "FM", "GA", "GF", "G3", "HA", "HO", "HR", "HX", "HY", "IB", "IG", "IY", "JE", "JJ", "JL", "JQ", "JU", "J2", "KA", "KC", "KF", "KL", "KM", "KQ", "KU", "LA", "LG", "LO", "LP", "LR", "LX", "LY", "ME", "MH", "MI", "MK", "MS", "MU", "NH", "NZ", "OA", "OB", "OK", "OM", "OS", "OU", "OZ", "PG", "PR", "PS", "PZ", "QF", "QR", "RJ", "RO", "R7", "SA", "SB", "SC", "SK", "SN", "SQ", "SU", "SV", "SW", "S3", "S7", "TA", "TG", "TK", "TN", "TP", "TR", "TU", "UA", "UK", "UL", "UT", "UU", "UX", "VA", "VN", "VW", "WE", "WF", "WK", "WY", "XL", "XQ", "YM", "ZH", "2K", "3K", "4M", "4O", "4U", "9B", "9U" }),
            new Airline("LX", "瑞航", new string[]{ "AA", "AC", "AD", "AE", "AF", "AI", "AM", "AR", "AV", "AY", "AZ", "A3", "BA", "BR", "CA", "CI", "CM", "CX", "CZ", "DE", "DL", "EK", "EN", "ET", "EW", "EY", "FI", "GA", "GF", "G3", "HX", "IB", "JJ", "JL", "JQ", "JU", "KA", "KE", "KL", "KM", "KQ", "LA", "LG", "LH", "LO", "LP", "LR", "LY", "ME", "MH", "MI", "MK", "MS", "MU", "NH", "NZ", "OA", "OS", "OU", "OZ", "PG", "PK", "PR", "PW", "PZ", "QF", "QR", "SA", "SK", "SN", "SQ", "SU", "SV", "S7", "TA", "TG", "TK", "TP", "TR", "T0", "UA", "UK", "UL", "UU", "UX", "VA", "VF", "VN", "WE", "WF", "WK", "WY", "XL", "ZH", "2K", "3K", "4M", "4U", "9W" }),
            new Airline("AY", "芬航", new string[]{ "AA", "AF", "AI", "AS", "AZ", "A3", "BA", "BR", "BT", "B2", "CA", "CI", "CX", "CZ", "DC", "DE", "DL", "DX", "EI", "EK", "EY", "FB", "FI", "FJ", "FM", "GA", "GK", "HO", "HR", "HU", "HX", "IB", "JC", "JD", "JJ", "JL", "JQ", "JU", "KA", "KE", "KL", "KM", "LA", "LG", "LH", "LO", "LP", "LX", "LY", "MF", "MH", "MI", "MS", "MU", "NH", "NU", "NZ", "OA", "OK", "OS", "OU", "OZ", "PG", "PR", "PS", "PZ", "QF", "QR", "RC", "RJ", "SB", "SK", "SN", "SQ", "SU", "S7", "TG", "TK", "TN", "TP", "UA", "UK", "UL", "UX", "U6", "VN", "VS", "WF", "WS", "W2", "XL", "2N", "3K", "3U", "4M", "9B", "9W" }),
            new Airline("BA", "英航", new string[]{ "AA", "AC", "AE", "AF", "AH", "AI", "AM", "AS", "AT", "AV", "AY", "AZ", "A3", "BG", "BI", "BL", "BM", "BP", "BR", "BT", "BW", "B6", "CA", "CI", "CM", "CX", "CZ", "DL", "DT", "EI", "ET", "EY", "FB", "FI", "FJ", "GA", "GF", "GK", "G3", "HA", "HM", "HX", "IB", "IZ", "JJ", "JL", "JP", "JQ", "JU", "JY", "J2", "KA", "KC", "KE", "KK", "KL", "KM", "KP", "KQ", "KU", "KX", "K6", "LA", "LG", "LH", "LI", "LM", "LO", "LP", "LR", "LX", "LY", "ME", "MH", "MI", "MN", "MS", "MU", "NH", "NU", "NZ", "OA", "OK", "OM", "OS", "OU", "OZ", "PG", "PR", "PS", "PW", "PX", "PZ", "QF", "QR", "RC", "RJ", "RO", "SA", "SB", "SK", "SN", "SQ", "SS", "SU", "SV", "SW", "S7", "TA", "TF", "TG", "TJ", "TK", "TM", "TN", "TP", "TU", "T0", "UA", "UK", "UL", "UP", "UU", "VN", "VS", "VY", "WM", "WS", "WY", "XK", "XL", "2K", "2N", "3K", "3U", "4C", "4M", "4O", "4Z", "9B", "9W" }),
            new Airline("VS", "维珍英国", new string[]{ "AA", "AC", "AF", "AI", "AM", "AY", "AZ", "A3", "BA", "BE", "BP", "BW", "CA", "CX", "CZ", "DL", "EI", "EK", "EY", "FB", "FI", "FJ", "FM", "FZ", "G3", "HA", "HR", "HX", "IB", "JJ", "JU", "KA", "KE", "KL", "KM", "KX", "LA", "LI", "LO", "LP", "LY", "ME", "MH", "MI", "MS", "MU", "NH", "NZ", "OU", "OZ", "PR", "PZ", "QF", "SA", "SK", "SQ", "SU", "SV", "TG", "TK", "TN", "TP", "UA", "UK", "UX", "VA", "VN", "WS", "XL", "4C", "4M", "9W" }),
            new Airline("EK", "阿联酋", new string[]{ "AC", "AD", "AE", "AF", "AH", "AI", "AR", "AS", "AT", "AV", "AW", "AY", "AZ", "A3", "A9", "BG", "BI", "BL", "BP", "BR", "BT", "BV", "BW", "B2", "B6", "CA", "CI", "CM", "CX", "CZ", "DT", "DX", "EI", "ET", "EY", "FB", "FI", "FJ", "FM", "FN", "FZ", "GA", "GF", "GK", "G3", "HA", "HF", "HM", "HR", "HX", "IB", "IE", "JJ", "JL", "JO", "JQ", "KA", "KC", "KE", "KL", "KM", "KP", "KQ", "KU", "K6", "LA", "LG", "LH", "LM", "LO", "LP", "LR", "LX", "ME", "MH", "MI", "MK", "MS", "MU", "NH", "NU", "NX", "NZ", "OA", "OB", "OD", "OK", "OM", "OS", "OU", "OZ", "PC", "PG", "PK", "PR", "PS", "PW", "PX", "PZ", "P0", "QF", "QS", "QV", "RA", "RJ", "RO", "SA", "SB", "SG", "SK", "SQ", "SS", "SU", "SV", "SW", "S7", "TA", "TG", "TK", "TM", "TN", "TP", "TU", "T3", "UK", "UL", "UX", "U6", "VN", "VS", "VY", "V7", "WF", "WS", "WY", "W2", "W3", "XL", "YM", "ZH", "0O", "2J", "3K", "3M", "3U", "4C", "4M", "4O", "7T", "8M", "9B", "9U" }),
            new Airline("EY", "阿提哈德", new string[]{ "AA", "AC", "AD", "AF", "AH", "AI", "AM", "AR", "AT", "AV", "AY", "AZ", "A3", "BA", "BG", "BP", "BR", "BT", "B2", "B6", "CA", "CI", "CM", "CX", "CZ", "EC", "EI", "EK", "ET", "EW", "FI", "FJ", "FM", "GA", "GF", "G3", "HA", "HM", "HO", "HR", "HU", "HX", "HY", "IB", "IY", "JJ", "JQ", "JU", "KA", "KC", "KE", "KK", "KL", "KM", "KQ", "KU", "LA", "LG", "LH", "LO", "LP", "LR", "LX", "L6", "ME", "MF", "MH", "MN", "MS", "MU", "NH", "NZ", "OA", "OD", "OK", "OM", "OS", "OU", "OZ", "PG", "PK", "PR", "PS", "PW", "PX", "PZ", "QF", "QN", "QV", "RJ", "SA", "SB", "SK", "SN", "SQ", "SU", "SV", "SW", "S7", "TA", "TG", "TK", "TM", "TP", "TU", "T0", "UA", "UL", "UN", "UX", "VA", "VN", "VS", "VY", "WB", "WF", "WS", "WY", "XL", "XY", "YM", "ZH", "3L", "3U", "4C", "4M", "4Q", "4U", "8U", "9B", "9U" }),
            new Airline("QR", "卡塔尔", new string[]{ "AA", "AC", "AD", "AE", "AF", "AH", "AI", "AR", "AS", "AT", "AV", "AW", "AY", "AZ", "A3", "BA", "BG", "BI", "BL", "BP", "BR", "BT", "B2", "B6", "CA", "CI", "CM", "CU", "CX", "CY", "CZ", "DT", "DV", "EI", "ET", "FA", "FB", "FI", "FJ", "FM", "FN", "GA", "GF", "GK", "GQ", "G3", "HM", "HO", "HR", "HU", "HX", "HY", "IB", "IE", "IG", "IZ", "JJ", "JL", "JQ", "JU", "J2", "J9", "KA", "KC", "KE", "KL", "KM", "KP", "KQ", "KU", "KX", "K6", "LA", "LG", "LH", "LM", "LO", "LP", "LR", "LX", "ME", "MF", "MH", "MI", "MS", "MU", "NH", "NU", "NX", "NZ", "OA", "OD", "OK", "OM", "OS", "OU", "OZ", "O6", "PC", "PD", "PG", "PR", "PS", "PW", "PX", "PZ", "QF", "QV", "RA", "RG", "RJ", "RO", "SA", "SC", "SK", "SN", "SQ", "SU", "SV", "SW", "S7", "TA", "TG", "TK", "TM", "TP", "TU", "UB", "UK", "UL", "UM", "UX", "U6", "VA", "VJ", "VN", "VY", "WB", "WF", "WS", "WY", "W2", "W3", "XK", "XL", "YM", "ZH", "2C", "3K", "3U", "4M", "4O", "4Q", "4Z", "5H", "6E", "6I", "8M", "8Q", "8U", "9B", "9K" }),
            new Airline("TK", "土航", new string[]{ "AA", "AC", "AD", "AF", "AH", "AI", "AM", "AR", "AS", "AT", "AV", "AY", "AZ", "A3", "BA", "BG", "BI", "BL", "BP", "BR", "BT", "B2", "B6", "CA", "CI", "CM", "CX", "CZ", "DT", "EI", "EK", "ET", "EY", "FB", "FI", "FJ", "FM", "GA", "GF", "GK", "GP", "G3", "HA", "HM", "HO", "HR", "HU", "HX", "HY", "IB", "IG", "IY", "JJ", "JL", "JQ", "JU", "J2", "KA", "KC", "KE", "KL", "KM", "KP", "KQ", "KU", "K6", "LA", "LG", "LH", "LM", "LN", "LO", "LP", "LR", "LX", "LY", "ME", "MF", "MH", "MI", "MK", "MS", "MU", "NH", "NZ", "OA", "OB", "OD", "OK", "OM", "OS", "OU", "OZ", "PG", "PK", "PR", "PS", "PW", "PX", "PY", "PZ", "QF", "QR", "QV", "RA", "RC", "RJ", "RO", "SA", "SC", "SK", "SN", "SQ", "SU", "SV", "SW", "SZ", "S7", "TA", "TG", "TM", "TP", "TU", "T0", "UA", "UK", "UL", "UT", "UX", "U6", "VN", "VR", "VS", "VW", "WB", "WF", "WY", "XL", "XQ", "XY", "YM", "ZB", "ZH", "2J", "2K", "3K", "3U", "4C", "4M", "4O", "6E", "8M", "9B" }),
            new Airline("ET", "埃塞俄比亚航空", new string[]{ "AA", "AC", "AD", "AF", "AH", "AI", "AR", "AS", "AT", "AV", "AW", "AZ", "A3", "BA", "BE", "BP", "BR", "B6", "B8", "CA", "CI", "CM", "CU", "CX", "CZ", "DE", "DL", "DT", "EK", "ET", "EY", "FB", "FJ", "FM", "FZ", "GA", "GF", "GP", "G3", "HF", "HM", "HO", "HR", "HU", "HX", "JJ", "JK", "JQ", "JU", "KA", "KE", "KF", "KK", "KL", "KM", "KP", "KQ", "KU", "LA", "LG", "LH", "LO", "LX", "LY", "ME", "MF", "MH", "MI", "MK", "MS", "MU", "NH", "NZ", "OA", "OB", "OD", "OS", "OU", "OZ", "PG", "PR", "PS", "PW", "PX", "PZ", "P0", "QF", "QR", "RA", "RJ", "SA", "SK", "SN", "SQ", "SU", "SV", "SW", "S7", "TA", "TC", "TG", "TK", "TM", "TP", "TU", "UA", "UK", "UL", "UM", "UX", "VN", "VR", "VU", "WB", "WF", "WY", "W2", "XY", "ZH", "2J", "2K", "3J", "3K", "3U", "4Q", "9B" }),
            new Airline("TP", "葡航", new string[]{ "AA", "AC", "AD", "AF", "AH", "AI", "AM", "AR", "AS", "AT", "AV", "AY", "AZ", "A3", "BA", "BE", "BR", "BT", "B6", "CA", "CI", "CM", "CX", "CZ", "DL", "DT", "EK", "ET", "EY", "FB", "FI", "FM", "GF", "G3", "HU", "HX", "IB", "IG", "JD", "JL", "JP", "JU", "KE", "KF", "KL", "KM", "KP", "LG", "LH", "LO", "LR", "LX", "LY", "MH", "MK", "MS", "MU", "NH", "NI", "NT", "NZ", "OA", "OK", "OS", "OU", "OZ", "O6", "PS", "QF", "QI", "QR", "RJ", "RO", "SA", "SK", "SN", "SP", "SQ", "SU", "SV", "SW", "S4", "S7", "TA", "TG", "TK", "TM", "TU", "T0", "UA", "UL", "UN", "UX", "VS", "V7", "WF", "WS", "WY", "YM", "ZH", "ZI", "0B", "2K", "9B", "9U", "9W" }),
            new Airline("SK", "北欧", new string[]{ "AA", "AC", "AF", "AI", "AM", "AS", "AT", "AV", "AY", "AZ", "A3", "BA", "BM", "BR", "BT", "CA", "CI", "CM", "CX", "DC", "DE", "DL", "DX", "EI", "EK", "EN", "ET", "EY", "FI", "FM", "GF", "GL", "HM", "HX", "IB", "IG", "JJ", "JL", "JU", "KA", "KE", "KL", "KM", "KQ", "KX", "LA", "LG", "LH", "LO", "LP", "LR", "LX", "LY", "ME", "MH", "MI", "MK", "MS", "MU", "NH", "NZ", "OA", "OK", "OS", "OU", "OZ", "PG", "PK", "PR", "PS", "PZ", "QF", "QR", "RC", "RJ", "RO", "SA", "SN", "SQ", "SU", "SV", "SW", "S4", "S7", "TA", "TG", "TK", "TN", "TP", "TU", "T0", "UA", "UL", "UX", "VN", "VS", "WF", "WY", "XL", "ZH", "4M", "6I", "9B", "9W" }),
            new Airline("OS", "奥地利", new string[]{ "AA", "AC", "AF", "AI", "AV", "AY", "AZ", "A3", "A9", "BA", "BR", "BT", "B2", "CA", "CI", "CL", "CM", "CX", "DE", "DL", "EK", "EN", "ET", "EW", "EY", "FB", "FI", "FM", "GF", "HO", "IB", "JJ", "JL", "JU", "J2", "KA", "KC", "KE", "KL", "KM", "LA", "LG", "LH", "LO", "LP", "LR", "LX", "LY", "MH", "MI", "MK", "MS", "MU", "NH", "NZ", "OA", "OU", "OZ", "PG", "PR", "PS", "PZ", "QF", "QR", "RJ", "RO", "SA", "SK", "SN", "SQ", "SU", "SV", "SW", "S7", "TA", "TD", "TG", "TK", "TP", "TR", "TU", "T0", "UA", "VA", "VN", "WE", "WF", "WK", "WY", "XL", "YM", "ZH", "2K", "4M", "4U", "9B" }),
            new Airline("MH", "马航", new string[]{ "AA", "AC", "AF", "AI", "AS", "AT", "AY", "BA", "BG", "BI", "BP", "BR", "B7", "CA", "CI", "CX", "CZ", "DJ", "DL", "EI", "EK", "ET", "EY", "FJ", "FM", "FY", "GA", "GF", "HA", "HR", "HU", "HX", "HY", "IB", "IC", "IE", "IY", "JJ", "JL", "JP", "JQ", "JU", "J2", "KA", "KC", "KE", "KK", "KL", "KQ", "KU", "LA", "LG", "LH", "LO", "LP", "LX", "ME", "MF", "MI", "MK", "MS", "MU", "NH", "NZ", "OK", "OS", "OU", "OZ", "PG", "PK", "PR", "PS", "PX", "QF", "QR", "RA", "RJ", "SA", "SB", "SK", "SN", "SQ", "SU", "SV", "S7", "TG", "TK", "TL", "TP", "TU", "UA", "UK", "UL", "UN", "VA", "VN", "VS", "WB", "WY", "XL", "ZH", "2A", "3K", "4M", "8M", "9B", "9W" }),
            new Airline("CA","国航",  new string[]{ }),
            new Airline("MU","东航/上航",  new string[]{ }),
            new Airline("CZ","南航",  new string[]{ }),
            new Airline("HU","海航",  new string[]{ }),
            new Airline( "MF","厦航", new string[]{ }),
            new Airline("3U","川航",  new string[]{ }),
            new Airline("ZH","深航",  new string[]{ }),
            new Airline("HO","吉祥",  new string[]{ }),
            new Airline("9C","春秋",  new string[]{ }),
            new Airline("JD", "首都", new string[]{ }),
            new Airline("SC","山航",  new string[]{ }),
            new Airline("QW","青岛航",  new string[]{ }),
            new Airline("GJ","长龙",  new string[]{ }),
            new Airline("IJ", "春秋日本", new string[]{ }),
            new Airline("TW", "德威", new string[]{ }),
        }.OrderBy(a => a.Code).ToArray();

        private static bool InSameAlliance(string a, string b)
        {
            foreach (var it in Alliance)
                if (it.Contains(a) && it.Contains(b))
                    return true;
            return false;
        }
        public static InterlineStatus GetIetState(string from, string to)
        {
            if (from == "??" || to == "??")
                return InterlineStatus.Unknown;

            if (from == to)
                return InterlineStatus.Ok;

            for (var i = 0; i < InterlineData.Length; i++)
            {
                if (InterlineData[i].Code != from)
                    continue;

                foreach (var it in InterlineData[i].IET)
                {
                    if (it == to)
                        return InSameAlliance(from, to) ? InterlineStatus.Ok : InterlineStatus.OkOutOfAlliance;
                }

                return InterlineStatus.No;
            }

            return InterlineStatus.Unknown;
        }
    }

    public enum InterlineStatus
    {
        Unknown,
        Ok,
        OkOutOfAlliance,
        No,
    }
}
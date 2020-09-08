using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcrCalculator
{
    public struct AirlineInterlineData
    {
        public string FriendlyName;
        public string Code;
        public string[] IET;

        public AirlineInterlineData(string name, string code, string[] iet)
        {
            FriendlyName = name;
            Code = code;
            IET = iet;
        }
    }
    public static class AirlineDataset
    {
        public static readonly AirlineInterlineData[] InterlineData = new AirlineInterlineData[]
        {
            new AirlineInterlineData("美联航", "UA", new[]{ "AC", "CA", "MU*", "HU*", "CZ*", "MF*", "HO*", "NH", "JL*", "KE*", "OZ", "7C*", "MH*"}),
            new AirlineInterlineData("美航", "AA", new string[]{  }),
            new AirlineInterlineData("达美", "DL", new[]{ "AC*", "MU", "CZ", "MF", "NH*", "JL*", "KE", "OZ*", "AF" , "LJ*", "MH*"}),
            new AirlineInterlineData("加航", "AC", new[]{ "MU*", "CZ*", "NH", "JL*", "KE*", "OZ", "7C*", "MH*"}),
            new AirlineInterlineData("阿拉斯加", "AS", new[]{ "AC*", "MU*", "CZ*", "MF*", "HU*", "NH*", "JL*", "KE*", "OZ*", "MH*" }),
            new AirlineInterlineData("国航", "CA", new string[]{ }),
            new AirlineInterlineData("东航/上航", "MU", new string[]{ }),
            new AirlineInterlineData("南航", "CZ", new string[]{ }),
            new AirlineInterlineData("海航", "HU", new string[]{ }),
            new AirlineInterlineData("厦航", "MF", new string[]{ }),
            new AirlineInterlineData("川航", "3U", new string[]{ }),
            new AirlineInterlineData("深航", "ZH", new string[]{ }),
            new AirlineInterlineData("吉祥", "HO", new string[]{ }),
            new AirlineInterlineData("春秋", "9C", new string[]{ }),
            new AirlineInterlineData("首都", "JD", new string[]{ }),
            new AirlineInterlineData("全日空", "NH", new[]{ "MU*", "CA", "MF*", "CZ*", "HO*", "HU*", "MH" }),
            new AirlineInterlineData("日航", "JL", new[]{ "MU*", "CA*", "MF*", "CZ*", "HU*", "3U*", "MH" }),
            new AirlineInterlineData("大韩", "KE", new[]{ "MU", "MF", "CZ", "CA*", "HO*", "HU*", "SC*", "LJ*", "MH" }),
            new AirlineInterlineData("韩亚", "OZ", new[]{ "MU*", "MF*", "CZ*", "CA*", "HO*", "3U*", "SC*", "BX*", "MH"}),
            new AirlineInterlineData("济州航空", "7C", new string[]{ }),
            new AirlineInterlineData("马航", "MH", new string[]{ }),
            new AirlineInterlineData("真航空", "LJ", new string[]{ }),
            new AirlineInterlineData("釜山航", "BX", new string[]{ }),
            new AirlineInterlineData("山航", "SC", new string[]{ }),
            new AirlineInterlineData("青岛航", "QW", new string[]{ }),
            new AirlineInterlineData("长龙", "GJ", new string[]{ }),
            new AirlineInterlineData("长荣", "BR", new string[]{ }),
            new AirlineInterlineData("华航", "CI", new string[]{ }),
            new AirlineInterlineData("葡航", "TP", new string[]{ })
        };

        public static InterlineStatus GetIetState(string from, string to)
        {
            if (from == "??" || to == "??")
                return InterlineStatus.LackOfData;

            if (from == to)
                return InterlineStatus.Ok;

            for (var i = 0; i < InterlineData.Length; i++)
            {
                if (InterlineData[i].Code != from)
                    continue;

                foreach (var it in InterlineData[i].IET)
                {
                    if (it == to)
                        return InterlineStatus.Ok;
                    if (it == to + "*")
                        return InterlineStatus.OkOutOfAlliance;
                }

                return InterlineStatus.UnknownOrNo;
            }

            return InterlineStatus.LackOfData;
        }
    }

    public enum InterlineStatus
    {
        LackOfData,
        Ok,
        OkOutOfAlliance,
        UnknownOrNo,
    }   
}

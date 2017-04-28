namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class MarketData
    {
        public static Dictionary<int, MarketData> dataMap = new Dictionary<int, MarketData>();

        public static void InsertMarketData(int id, int version, LuaTable data)
        {
            if (dataMap.ContainsKey(id))
            {
                dataMap.Remove(id);
            }
            MarketData data2 = new MarketData {
                id = id,
                marketVersion = version,
                hot = int.Parse((string) data["1"]),
                hotSort = int.Parse((string) data["2"]),
                jewel = int.Parse((string) data["3"]),
                jewelSort = int.Parse((string) data["4"]),
                item = int.Parse((string) data["5"]),
                itemSort = int.Parse((string) data["6"]),
                wing = int.Parse((string) data["7"]),
                wingSort = int.Parse((string) data["8"]),
                mode = int.Parse((string) data["9"]),
                label = int.Parse((string) data["10"]),
                itemId = int.Parse((string) data["11"]),
                itemNumber = int.Parse((string) data["12"]),
                priceOrg = int.Parse((string) data["13"]),
                priceNow = int.Parse((string) data["14"]),
                vipLevel = int.Parse((string) data["15"]),
                totalCount = int.Parse((string) data["16"]),
                startTime = null,
                duration = int.Parse((string) data["18"])
            };
            dataMap.Add(id, data2);
        }

        public int duration { get; protected set; }

        public int hot { get; protected set; }

        public int hotSort { get; protected set; }

        public int id { get; protected set; }

        public int item { get; protected set; }

        public int itemId { get; protected set; }

        public int itemNumber { get; protected set; }

        public int itemSort { get; protected set; }

        public int jewel { get; protected set; }

        public int jewelSort { get; protected set; }

        public int label { get; protected set; }

        public int marketVersion { get; protected set; }

        public int mode { get; protected set; }

        public int priceNow { get; protected set; }

        public int priceOrg { get; protected set; }

        public int[] startTime { get; protected set; }

        public int totalCount { get; protected set; }

        public int vipLevel { get; protected set; }

        public int wing { get; protected set; }

        public int wingSort { get; protected set; }
    }
}


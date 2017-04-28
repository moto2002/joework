namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class UpgradeGuideData : Mogo.GameData.GameData<UpgradeGuideData>
    {
        public static readonly string fileName = "xml/UpgradeGuide";

        public static UpgradeGuideData GetData(int id)
        {
            if (Mogo.GameData.GameData<UpgradeGuideData>.dataMap.ContainsKey(id))
            {
                return Mogo.GameData.GameData<UpgradeGuideData>.dataMap[id];
            }
            return null;
        }

        public static List<int> GetLevelNoEnoughList()
        {
            List<int> list = new List<int>();
            foreach (KeyValuePair<int, UpgradeGuideData> pair in Mogo.GameData.GameData<UpgradeGuideData>.dataMap)
            {
                if (pair.Value.type == 1)
                {
                    list.Add(pair.Key);
                }
            }
            return list;
        }

        public static List<int> GetNoNeedEnergyList()
        {
            List<int> list = new List<int>();
            foreach (KeyValuePair<int, UpgradeGuideData> pair in Mogo.GameData.GameData<UpgradeGuideData>.dataMap)
            {
                if (pair.Value.type == 2)
                {
                    list.Add(pair.Key);
                }
            }
            return list;
        }

        public int describtion { get; protected set; }

        public int icon { get; protected set; }

        public int title { get; protected set; }

        public int type { get; protected set; }
    }
}


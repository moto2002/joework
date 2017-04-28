namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class UpgradePowerData : Mogo.GameData.GameData<UpgradePowerData>
    {
        public static readonly string fileName = "xml/UpgradePower";

        public static UpgradePowerData GetUpgradePowerDataByID(int id)
        {
            if (Mogo.GameData.GameData<UpgradePowerData>.dataMap.ContainsKey(id))
            {
                return Mogo.GameData.GameData<UpgradePowerData>.dataMap[id];
            }
            return null;
        }

        public static List<int> GetUpgradePowerIDList()
        {
            List<int> list = new List<int>();
            foreach (int num in Mogo.GameData.GameData<UpgradePowerData>.dataMap.Keys)
            {
                list.Add(num);
            }
            return list;
        }

        public int floatText { get; protected set; }

        public int hyper { get; protected set; }

        public int icon { get; protected set; }

        public int level { get; protected set; }

        public int name { get; protected set; }

        public int text { get; protected set; }
    }
}


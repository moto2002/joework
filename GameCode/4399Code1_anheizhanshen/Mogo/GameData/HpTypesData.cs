namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class HpTypesData : Mogo.GameData.GameData<HpTypesData>
    {
        public static readonly string fileName = "xml/HpTypes";

        public static int GetHpBottleCD(int vipLevel)
        {
            int key = 0;
            foreach (KeyValuePair<int, int> pair in Mogo.GameData.GameData<PrivilegeData>.dataMap[vipLevel].hpBottles)
            {
                key = pair.Key;
            }
            return Mogo.GameData.GameData<HpTypesData>.dataMap[key].cd;
        }

        public int buffId { get; set; }

        public int cd { get; set; }
    }
}


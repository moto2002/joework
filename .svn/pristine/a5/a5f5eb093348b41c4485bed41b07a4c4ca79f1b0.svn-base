namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class EquipRecommendData : Mogo.GameData.GameData<EquipRecommendData>
    {
        public static readonly string fileName = "xml/EquipRecommend";

        public static EquipRecommendData GetEquipRecommendData(int vocation, int level)
        {
            foreach (EquipRecommendData data in Mogo.GameData.GameData<EquipRecommendData>.dataMap.Values)
            {
                if (((data.vocation == vocation) && (level >= data.level[0])) && (level <= data.level[1]))
                {
                    return data;
                }
            }
            return null;
        }

        public List<int> access { get; protected set; }

        public List<int> accessType { get; protected set; }

        public List<int> goodsid { get; protected set; }

        public List<int> level { get; protected set; }

        public int star { get; protected set; }

        public int vocation { get; protected set; }
    }
}


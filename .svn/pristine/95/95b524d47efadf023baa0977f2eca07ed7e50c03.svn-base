namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class PowerScoreJewelData : Mogo.GameData.GameData<PowerScoreJewelData>
    {
        public static readonly string fileName = "xml/PowerScoreJewel";

        public static int GetJewelScoreByLevel(int level)
        {
            if (Mogo.GameData.GameData<PowerScoreJewelData>.dataMap.ContainsKey(level))
            {
                return Mogo.GameData.GameData<PowerScoreJewelData>.dataMap[level].score;
            }
            return 0;
        }

        public int level { get; protected set; }

        public int score { get; protected set; }
    }
}


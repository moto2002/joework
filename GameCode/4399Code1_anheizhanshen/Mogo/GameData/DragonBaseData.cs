namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class DragonBaseData : Mogo.GameData.GameData<DragonBaseData>
    {
        public static readonly string fileName = "xml/DragonBase";

        public static int[] GetCostItem()
        {
            int[] numArray = new int[2];
            foreach (KeyValuePair<int, int> pair in Mogo.GameData.GameData<DragonBaseData>.dataMap[1].upQualityitemCost)
            {
                numArray[0] = pair.Key;
                numArray[1] = pair.Value;
                return numArray;
            }
            return numArray;
        }

        public int attackTimesPrice { get; protected set; }

        public int clearAttackCDPrice { get; protected set; }

        public int convoyAttackedTimes { get; protected set; }

        public int convoyTimesPrice { get; protected set; }

        public int cutCompleteTimeFiveMinPrice { get; protected set; }

        public int dailyAttackTimes { get; protected set; }

        public int dailyConvoyTimes { get; protected set; }

        public int freshAdversaryPrice { get; protected set; }

        public int goldDragonPrice { get; protected set; }

        public int immediateCompleteConvoyPrice { get; protected set; }

        public int levelNeed { get; protected set; }

        public int revengeTimes { get; protected set; }

        public int upgradeQualityCost { get; protected set; }

        public Dictionary<int, int> upQualityitemCost { get; protected set; }
    }
}


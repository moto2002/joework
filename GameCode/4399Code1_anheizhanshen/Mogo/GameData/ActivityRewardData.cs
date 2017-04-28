namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ActivityRewardData : Mogo.GameData.GameData<ActivityRewardData>
    {
        public static readonly string fileName = "xml/ActivityReward";

        public Dictionary<int, int> items { get; set; }

        public Dictionary<int, int> items2_m { get; set; }

        public int level { get; set; }

        public int wave { get; set; }
    }
}


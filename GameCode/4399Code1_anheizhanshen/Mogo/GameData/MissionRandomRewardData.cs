namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class MissionRandomRewardData : Mogo.GameData.GameData<MissionRandomRewardData>
    {
        public static readonly string fileName = "xml/MissionRandomReward";

        public int difficulty { get; set; }

        public List<int> item1 { get; set; }

        public List<int> item2 { get; set; }

        public List<int> item3 { get; set; }

        public List<int> item4 { get; set; }

        public int mission { get; set; }

        public List<int> num { get; set; }

        public List<int> random { get; set; }
    }
}


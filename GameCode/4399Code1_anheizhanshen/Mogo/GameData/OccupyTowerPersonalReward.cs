namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class OccupyTowerPersonalReward : Mogo.GameData.GameData<OccupyTowerPersonalReward>
    {
        public static readonly string fileName = "xml/Reward_DefecsePvP_Personal";

        public int exp { get; set; }

        public int gold { get; set; }

        public int level { get; set; }

        public int rank { get; set; }

        public Dictionary<int, int> reward { get; set; }
    }
}


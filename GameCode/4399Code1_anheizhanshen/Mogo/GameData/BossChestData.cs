namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class BossChestData : Mogo.GameData.GameData<BossChestData>
    {
        public static readonly string fileName = "xml/MissionBossTreasure";

        public int difficulty { get; set; }

        public int icon { get; set; }

        public int mission { get; set; }

        public int name { get; set; }

        public Dictionary<int, int> reward { get; set; }
    }
}


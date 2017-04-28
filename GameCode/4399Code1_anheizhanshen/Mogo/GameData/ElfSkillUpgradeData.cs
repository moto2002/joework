namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ElfSkillUpgradeData : Mogo.GameData.GameData<ElfSkillUpgradeData>
    {
        public static readonly string fileName = "xml/ElfSkillUpgrade";

        public Dictionary<int, int> consume { get; set; }

        public int id { get; set; }

        public int nameCode { get; set; }

        public int preSkillId { get; set; }
    }
}


namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class ElfSkillData : Mogo.GameData.GameData<ElfSkillData>
    {
        public static readonly string fileName = "xml/ElfSkill";

        public int id { get; set; }

        public int nameCode { get; set; }

        public int skillId { get; set; }

        public int weight { get; set; }
    }
}


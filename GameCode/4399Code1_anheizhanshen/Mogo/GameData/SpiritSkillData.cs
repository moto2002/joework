namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class SpiritSkillData : Mogo.GameData.GameData<SpiritSkillData>
    {
        public static readonly string fileName = "xml/SpiritSkillData";

        public int add_point { get; protected set; }

        public int icon { get; protected set; }

        public string name { get; protected set; }

        public int skillid { get; protected set; }
    }
}


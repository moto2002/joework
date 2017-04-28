namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class GuildLevelData : Mogo.GameData.GameData<GuildLevelData>
    {
        public static readonly string fileName = "xml/GuildLevel";

        public int level { get; set; }

        public int memberCount { get; set; }

        public int skillLevelLimit { get; set; }

        public int upgradeMoney { get; set; }
    }
}


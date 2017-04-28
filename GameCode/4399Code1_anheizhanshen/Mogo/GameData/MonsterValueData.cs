namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class MonsterValueData : Mogo.GameData.GameData<MonsterValueData>
    {
        public static readonly string fileName = "xml/MonsterValue";

        public MonsterValueData()
        {
            this.raidId = 0;
        }

        public static MonsterValueData GetData(int raidId, int difficulty)
        {
            return Mogo.GameData.GameData<MonsterValueData>.dataMap.FirstOrDefault<KeyValuePair<int, MonsterValueData>>(t => ((t.Value.raidId == raidId) && (t.Value.difficulty == difficulty))).Value;
        }

        public int attackBase { get; protected set; }

        public int difficulty { get; protected set; }

        public Dictionary<int, int> equ { get; protected set; }

        public int exp { get; protected set; }

        public int extraAntiDefenceRate { get; protected set; }

        public int extraCritRate { get; protected set; }

        public int extraDefenceRate { get; protected set; }

        public Dictionary<int, int> extraDrop { get; protected set; }

        public int extraHitRate { get; protected set; }

        public Dictionary<int, int> extraRandom { get; protected set; }

        public int extraTrueStrikeRate { get; protected set; }

        public List<int> gold { get; protected set; }

        public int goldChance { get; protected set; }

        public int goldStack { get; protected set; }

        public int hardType { get; protected set; }

        public int hpBase { get; protected set; }

        public int level { get; protected set; }

        public int missRate { get; protected set; }

        public int monsterType { get; protected set; }

        public int raidId { get; protected set; }

        public int raidType { get; protected set; }
    }
}


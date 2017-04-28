namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SpawnPointLevelData : Mogo.GameData.GameData<SpawnPointLevelData>
    {
        public static readonly string fileName = "xml/SpawnPointLevel";

        public List<int> monsterId { get; protected set; }

        public List<int> monsterNumber { get; protected set; }
    }
}


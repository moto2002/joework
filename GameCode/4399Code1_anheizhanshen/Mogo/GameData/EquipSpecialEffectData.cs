namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class EquipSpecialEffectData : Mogo.GameData.GameData<EquipSpecialEffectData>
    {
        public static readonly string fileName = "xml/EquipSpecialEffects";

        public int activeDesp { get; protected set; }

        public int activeScore { get; protected set; }

        public int fxid { get; protected set; }

        public int group { get; protected set; }

        public int groupName { get; protected set; }

        public int icon { get; protected set; }

        public int level { get; protected set; }

        public int name { get; protected set; }

        public Dictionary<int, int> scoreList { get; protected set; }
    }
}


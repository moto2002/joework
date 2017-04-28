namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class WingData : Mogo.GameData.GameData<WingData>
    {
        public static readonly string fileName = "xml/Wing";

        public WingLevelData GetLevelData(int level)
        {
            return WingLevelData.GetLevelData(base.id, level);
        }

        public Dictionary<int, int> activeCost { get; protected set; }

        public int activeDescrip { get; protected set; }

        public int descrip { get; protected set; }

        public int icon { get; protected set; }

        public Dictionary<int, int> limit { get; protected set; }

        public string modes { get; protected set; }

        public int name { get; protected set; }

        public int price { get; protected set; }

        public int subtype { get; protected set; }

        public int type { get; protected set; }

        public Dictionary<int, int> unlock { get; protected set; }

        public Dictionary<int, int> unlockCost { get; protected set; }

        public int unlockDescrip { get; protected set; }
    }
}


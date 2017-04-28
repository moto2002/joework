namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class RuneData : Mogo.GameData.GameData<RuneData>
    {
        public static readonly string fileName = "xml/Rune";

        public static RuneData GetNextLvRune(int type, int subType, int quality, int level)
        {
            foreach (RuneData data in Mogo.GameData.GameData<RuneData>.dataMap.Values)
            {
                if ((((data.type == type) && (data.subtype == subType)) && (data.quality == quality)) && (data.level == level))
                {
                    return data;
                }
            }
            return null;
        }

        public string desc { get; protected set; }

        public int effectDesc { get; protected set; }

        public int effectID { get; protected set; }

        public int expNeed { get; protected set; }

        public int expValue { get; protected set; }

        public int icon { get; protected set; }

        public int level { get; protected set; }

        public int name { get; protected set; }

        public int price { get; protected set; }

        public int quality { get; protected set; }

        public int score { get; protected set; }

        public int subtype { get; protected set; }

        public int type { get; protected set; }
    }
}


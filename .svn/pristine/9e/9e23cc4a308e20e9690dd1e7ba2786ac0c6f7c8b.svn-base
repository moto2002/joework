namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class DragonEventsData : Mogo.GameData.GameData<DragonEventsData>
    {
        public static readonly string fileName = "xml/DragonEvents";

        public int descpt { get; protected set; }

        public string Descpt
        {
            get
            {
                return LanguageData.GetContent(this.descpt);
            }
        }

        public int name { get; protected set; }

        public Dictionary<int, int> upQualityitemCost { get; protected set; }
    }
}


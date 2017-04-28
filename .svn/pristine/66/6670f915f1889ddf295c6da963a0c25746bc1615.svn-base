namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class DragonStationData : Mogo.GameData.GameData<DragonStationData>
    {
        public static readonly string fileName = "xml/DragonStation";

        public int addFactor { get; protected set; }

        public int descpt { get; protected set; }

        public string Descpt
        {
            get
            {
                return LanguageData.GetContent(this.descpt);
            }
        }

        public int name { get; protected set; }

        public string Name
        {
            get
            {
                return LanguageData.GetContent(this.name);
            }
        }

        public int rewardAddition { get; protected set; }
    }
}


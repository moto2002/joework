namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class DragonEventDescData : Mogo.GameData.GameData<DragonEventDescData>
    {
        public static readonly string fileName = "xml/DragonEventDesc";

        public int desc { get; protected set; }
    }
}


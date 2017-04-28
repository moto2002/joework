namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class SpiritLevelData_Mark : Mogo.GameData.GameData<SpiritLevelData_Mark>
    {
        public static readonly string fileName = "xml/SpiritLevelData_Mark";

        public int cost { get; protected set; }

        public int slot_num { get; protected set; }
    }
}


namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class EquipFXResData : Mogo.GameData.GameData<EquipFXResData>
    {
        public static readonly string fileName = "xml/EquipFXRes";

        public string EquipSlot { get; protected set; }

        public string level { get; protected set; }

        public string material { get; protected set; }

        public string particleSys { get; protected set; }
    }
}


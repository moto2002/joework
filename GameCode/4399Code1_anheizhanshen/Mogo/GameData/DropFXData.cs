namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class DropFXData : Mogo.GameData.GameData<DropFXData>
    {
        public static readonly string fileName = "xml/DropFXData";

        public int fx { get; protected set; }

        public byte quality { get; protected set; }

        public DropFxType type { get; protected set; }
    }
}


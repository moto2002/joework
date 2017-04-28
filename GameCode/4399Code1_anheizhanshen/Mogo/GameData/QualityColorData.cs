namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class QualityColorData : Mogo.GameData.GameData<QualityColorData>
    {
        public static readonly string fileName = "xml/QualityColor";

        public string color { get; protected set; }
    }
}


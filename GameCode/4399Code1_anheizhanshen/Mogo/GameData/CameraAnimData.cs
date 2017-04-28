namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class CameraAnimData : Mogo.GameData.GameData<CameraAnimData>
    {
        public static readonly string fileName = "xml/CameraAnim";

        public int xRate { get; set; }

        public float xSwing { get; set; }

        public int yRate { get; set; }

        public float ySwing { get; set; }

        public int zRate { get; set; }

        public float zSwing { get; set; }
    }
}


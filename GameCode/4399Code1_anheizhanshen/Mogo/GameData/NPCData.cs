namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class NPCData : Mogo.GameData.GameData<NPCData>
    {
        public static readonly string fileName = "xml/NPCData";

        public Dictionary<int, int> actionList { get; protected set; }

        public float colliderRange { get; protected set; }

        public int dialogBoxImage { get; protected set; }

        public List<int> idleTimeRange { get; protected set; }

        public int mapx { get; protected set; }

        public int mapy { get; protected set; }

        public int mode { get; protected set; }

        public int name { get; protected set; }

        public List<float> rotation { get; protected set; }

        public int standbyAction { get; protected set; }

        public List<int> thinkInterval { get; protected set; }

        public int tips { get; protected set; }
    }
}


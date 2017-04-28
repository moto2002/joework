namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class MapUIMappingData : Mogo.GameData.GameData<MapUIMappingData>
    {
        public static readonly string fileName = "xml/MapUIMapping";

        public List<int> bossChest { get; protected set; }

        public List<int> chest { get; protected set; }

        public Dictionary<int, int> grid { get; protected set; }

        public Dictionary<int, int> gridImage { get; protected set; }

        public Dictionary<int, int> gridName { get; protected set; }

        public Dictionary<int, int> gridShape { get; protected set; }

        public int name { get; protected set; }
    }
}


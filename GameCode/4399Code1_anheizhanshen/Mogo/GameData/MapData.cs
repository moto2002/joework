namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MapData : Mogo.GameData.GameData<MapData>
    {
        public static readonly string fileName = "xml/map_setting";

        public MapData()
        {
            this.sceneName = "InstanceScene";
            this.enterX = -200;
            this.enterY = -200;
        }

        public static bool IsSceneShowDeadTip(ushort sceneId)
        {
            if (Mogo.GameData.GameData<MapData>.dataMap == null)
            {
                return false;
            }
            if (!Mogo.GameData.GameData<MapData>.dataMap.ContainsKey(sceneId))
            {
                return false;
            }
            return ((Mogo.GameData.GameData<MapData>.dataMap[sceneId].type == MapType.Special) || (Mogo.GameData.GameData<MapData>.dataMap[sceneId].type == MapType.ARENA));
        }

        public Color ambientLight { get; protected set; }

        public List<int> backgroundMusic { get; protected set; }

        public Color cameraColor { get; protected set; }

        public float cameraFar { get; protected set; }

        public LightType characherLight { get; protected set; }

        public List<float> distanceList { get; protected set; }

        public short enterX { get; protected set; }

        public short enterY { get; protected set; }

        public bool fog { get; protected set; }

        public Color fogColor { get; protected set; }

        public FogMode fogMode { get; protected set; }

        public List<int> layerList { get; protected set; }

        public string lightmap { get; protected set; }

        public string lightProbes { get; protected set; }

        public float linearFogEnd { get; protected set; }

        public float linearFogStart { get; protected set; }

        public Dictionary<string, bool> modelName { get; protected set; }

        public Dictionary<int, Vector3> monstors { get; protected set; }

        public List<int> npcList { get; protected set; }

        public string sceneName { get; protected set; }

        public int trapID { get; protected set; }

        public MapType type { get; protected set; }
    }
}


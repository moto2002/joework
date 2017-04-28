namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class MissionData : Mogo.GameData.GameData<MissionData>
    {
        public static readonly string fileName = "xml/Mission";

        public static int GetCGByMission(int mapID)
        {
            KeyValuePair<int, MissionData> pair = Mogo.GameData.GameData<MissionData>.dataMap.FirstOrDefault<KeyValuePair<int, MissionData>>(t => t.Value.mission == mapID);
            if ((pair.Value != null) && (pair.Value.cg != 0))
            {
                return pair.Value.cg;
            }
            return -1;
        }

        public static MissionData GetMissionData(int mission, int difficulty)
        {
            foreach (KeyValuePair<int, MissionData> pair in Mogo.GameData.GameData<MissionData>.dataMap)
            {
                if ((pair.Value.mission == mission) && (pair.Value.difficulty == difficulty))
                {
                    return pair.Value;
                }
            }
            return null;
        }

        public static int GetReviveTimesByMission(int mission, int level)
        {
            KeyValuePair<int, MissionData> pair = Mogo.GameData.GameData<MissionData>.dataMap.FirstOrDefault<KeyValuePair<int, MissionData>>(t => (t.Value.mission == mission) && (t.Value.difficulty == level));
            if (pair.Value != null)
            {
                return pair.Value.reviveTimes;
            }
            return 0;
        }

        public static int GetSceneId(int mission, int difficulty)
        {
            foreach (KeyValuePair<int, MissionData> pair in Mogo.GameData.GameData<MissionData>.dataMap)
            {
                if ((pair.Value.mission == mission) && (pair.Value.difficulty == difficulty))
                {
                    return pair.Value.scene;
                }
            }
            return -1;
        }

        public int cg { get; protected set; }

        public int dayTimes { get; protected set; }

        public int difficulty { get; protected set; }

        public Dictionary<int, int> drop { get; protected set; }

        public int energy { get; protected set; }

        public List<int> events { get; protected set; }

        public int level { get; protected set; }

        public int minimumFight { get; protected set; }

        public int mission { get; protected set; }

        public int passEffect { get; protected set; }

        public int passTime { get; protected set; }

        public List<int> postMissions { get; protected set; }

        public List<int> preloadCG { get; protected set; }

        public Dictionary<int, int> preMissions { get; protected set; }

        public int recommend_level { get; protected set; }

        public int resetMoney { get; protected set; }

        public int reviveTimes { get; protected set; }

        public int scene { get; protected set; }

        public List<int> target { get; protected set; }

        public List<int> task { get; protected set; }
    }
}


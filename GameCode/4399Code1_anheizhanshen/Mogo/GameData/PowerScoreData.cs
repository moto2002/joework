namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PowerScoreData : Mogo.GameData.GameData<PowerScoreData>
    {
        public static readonly string fileName = "xml/PowerScore";

        public static PowerScoreData GetPowerScoreDataByLevel(int level)
        {
            if (Mogo.GameData.GameData<PowerScoreData>.dataMap.ContainsKey(level))
            {
                return Mogo.GameData.GameData<PowerScoreData>.dataMap[level];
            }
            return null;
        }

        public static int GetScoreBodyEnhanceByLevel(int level)
        {
            PowerScoreData powerScoreDataByLevel = GetPowerScoreDataByLevel(level);
            if (powerScoreDataByLevel != null)
            {
                return powerScoreDataByLevel.score[3];
            }
            return 0;
        }

        public static int GetScoreDiamondByLevel(int level)
        {
            PowerScoreData powerScoreDataByLevel = GetPowerScoreDataByLevel(level);
            if (powerScoreDataByLevel != null)
            {
                return powerScoreDataByLevel.score[1];
            }
            return 0;
        }

        public static int GetScoreEquipByLevel(int level)
        {
            PowerScoreData powerScoreDataByLevel = GetPowerScoreDataByLevel(level);
            if (powerScoreDataByLevel != null)
            {
                return powerScoreDataByLevel.score[0];
            }
            return 0;
        }

        public static int GetScoreRuneByLevel(int level)
        {
            PowerScoreData powerScoreDataByLevel = GetPowerScoreDataByLevel(level);
            if (powerScoreDataByLevel != null)
            {
                return powerScoreDataByLevel.score[2];
            }
            return 0;
        }

        public static int GetScoreTongByLevel(int level)
        {
            PowerScoreData powerScoreDataByLevel = GetPowerScoreDataByLevel(level);
            if (powerScoreDataByLevel != null)
            {
                return powerScoreDataByLevel.score[4];
            }
            return 0;
        }

        public int level { get; protected set; }

        public List<int> score { get; protected set; }
    }
}


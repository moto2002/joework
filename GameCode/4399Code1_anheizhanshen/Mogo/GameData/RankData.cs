namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class RankData : Mogo.GameData.GameData<RankData>
    {
        public static readonly string fileName = "xml/RankList";

        public static int GetRandomTipByID(int id)
        {
            if (Mogo.GameData.GameData<RankData>.dataMap.ContainsKey(id))
            {
                int num = Random.Range(0, Mogo.GameData.GameData<RankData>.dataMap[id].tip.Count);
                if (num < Mogo.GameData.GameData<RankData>.dataMap[id].tip.Count)
                {
                    return Mogo.GameData.GameData<RankData>.dataMap[id].tip[num];
                }
            }
            return 0;
        }

        public static RankData GetRankDataByID(int id)
        {
            if (Mogo.GameData.GameData<RankData>.dataMap.ContainsKey(id))
            {
                return Mogo.GameData.GameData<RankData>.dataMap[id];
            }
            return null;
        }

        public static List<int> GetSortRankIDList()
        {
            List<int> list = new List<int>();
            foreach (int num in Mogo.GameData.GameData<RankData>.dataMap.Keys)
            {
                list.Add(num);
            }
            list.Sort(delegate (int p1, int p2) {
                if (Mogo.GameData.GameData<RankData>.dataMap[p1].priority > Mogo.GameData.GameData<RankData>.dataMap[p2].priority)
                {
                    return 1;
                }
                return -1;
            });
            return list;
        }

        public int ifReward { get; protected set; }

        public int name { get; protected set; }

        public int priority { get; protected set; }

        public int rankCount { get; protected set; }

        public int rewardUI { get; protected set; }

        public List<int> tip { get; protected set; }

        public Dictionary<int, int> title { get; protected set; }

        public int type { get; protected set; }
    }
}


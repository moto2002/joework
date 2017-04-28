namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class MissionRewardData : Mogo.GameData.GameData<MissionRewardData>
    {
        public static readonly string fileName = "xml/MissionReward";

        public static void FixCondition()
        {
            fixCondition = new Dictionary<int, List<List<int>>>();
            foreach (KeyValuePair<int, MissionRewardData> pair in Mogo.GameData.GameData<MissionRewardData>.dataMap)
            {
                string[] strArray = pair.Value.condition.Split(new char[] { ';' });
                if ((strArray != null) && (strArray.Length != 0))
                {
                    List<List<int>> list = new List<List<int>>();
                    foreach (string str2 in strArray)
                    {
                        string[] strArray2 = str2.Split(new char[] { ',' });
                        if ((strArray2 != null) && (strArray2.Length == 3))
                        {
                            List<int> item = new List<int>();
                            foreach (string str3 in strArray2)
                            {
                                item.Add(Convert.ToInt32(str3));
                            }
                            if (item.Count == 3)
                            {
                                list.Add(item);
                            }
                        }
                    }
                    if (list.Count > 0)
                    {
                        fixCondition.Add(pair.Key, list);
                    }
                }
            }
        }

        public string condition { get; protected set; }

        public static Dictionary<int, List<List<int>>> fixCondition
        {
            [CompilerGenerated]
            get
            {
                return <fixCondition>k__BackingField;
            }
            [CompilerGenerated]
            protected set
            {
                <fixCondition>k__BackingField = value;
            }
        }

        public Dictionary<int, int> rewards { get; protected set; }
    }
}


namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ActivityTimeData : Mogo.GameData.GameData<ActivityTimeData>
    {
        protected static Dictionary<int, Dictionary<int, string>> activityTimeFormatDataMap = new Dictionary<int, Dictionary<int, string>>();
        public static readonly string fileName = "xml/ActivityTime";

        public static void FormatActivityTimeData()
        {
            foreach (KeyValuePair<int, ActivityTimeData> pair in Mogo.GameData.GameData<ActivityTimeData>.dataMap)
            {
                if (!activityTimeFormatDataMap.ContainsKey(pair.Value.weekDay))
                {
                    activityTimeFormatDataMap.Add(pair.Value.weekDay, new Dictionary<int, string>());
                }
                if (pair.Value.activityTime != null)
                {
                    string[] strArray = pair.Value.activityTime.Split(new char[] { ',' });
                    foreach (string str in strArray)
                    {
                        string[] strArray2 = str.Split(new char[] { '@' });
                        if ((strArray2.Length >= 2) && !activityTimeFormatDataMap[pair.Value.weekDay].ContainsKey(Convert.ToInt32(strArray2[1])))
                        {
                            activityTimeFormatDataMap[pair.Value.weekDay].Add(Convert.ToInt32(strArray2[1]), strArray2[0]);
                        }
                    }
                }
            }
        }

        public string activityTime { get; protected set; }

        public static Dictionary<int, Dictionary<int, string>> ActivityTimeFormatDataMap
        {
            get
            {
                if (activityTimeFormatDataMap.Count == 0)
                {
                    FormatActivityTimeData();
                }
                return activityTimeFormatDataMap;
            }
        }

        public int weekDay { get; protected set; }
    }
}


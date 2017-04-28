namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class GuideXMLData : Mogo.GameData.GameData<GuideXMLData>
    {
        public static readonly string fileName = "xml/guide";
        private static List<int> m_events;
        private static Dictionary<int, List<int>> m_links;

        public static List<int> GetIDByEvent(int events)
        {
            return (from t in Mogo.GameData.GameData<GuideXMLData>.dataMap
                where t.Value.conditionEvent == events
                select t.Key).ToList<int>();
        }

        public int conditionEvent { get; set; }

        public static List<int> ConditionEvent
        {
            get
            {
                if (m_events == null)
                {
                    m_events = (from data in Mogo.GameData.GameData<GuideXMLData>.dataMap select data.Value.conditionEvent).Distinct<int>().ToList<int>();
                }
                return m_events;
            }
        }

        public string event_arg1 { get; set; }

        public string event_arg2 { get; set; }

        public int group { get; protected set; }

        public List<int> guideLevel { get; set; }

        public int guideTimes { get; set; }

        public static Dictionary<int, List<int>> Links
        {
            get
            {
                if (m_links == null)
                {
                    m_links = (from a in Mogo.GameData.GameData<GuideXMLData>.dataMap group a by a.Value.group).ToDictionary<IGrouping<int, KeyValuePair<int, GuideXMLData>>, int, List<int>>(gdc => gdc.Key, gdc => (from x in gdc
                        orderby x.Value.step
                        select x.Value.id).ToList<int>());
                }
                return m_links;
            }
        }

        public string openDialog { get; set; }

        public int openGUI { get; set; }

        public int priority { get; set; }

        public Dictionary<string, string> restriction { get; protected set; }

        public int step { get; protected set; }

        public int target { get; set; }

        public string target_arg1 { get; set; }

        public string target_arg2 { get; set; }

        public int text { get; set; }
    }
}


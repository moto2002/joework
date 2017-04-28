namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SkillBuffData : Mogo.GameData.GameData<SkillBuffData>
    {
        public static readonly string fileName = "xml/SkillBuff";

        public static string GetName(int id)
        {
            string str = "no name";
            if (!Mogo.GameData.GameData<SkillBuffData>.dataMap.ContainsKey(id))
            {
                return str;
            }
            if (!Mogo.GameData.GameData<LanguageData>.dataMap.ContainsKey(Mogo.GameData.GameData<SkillBuffData>.dataMap[id].name))
            {
                return str;
            }
            return LanguageData.GetContent(Mogo.GameData.GameData<SkillBuffData>.dataMap[id].name);
        }

        public Dictionary<int, int> activeSkill { get; protected set; }

        public List<int> appendState { get; protected set; }

        public Dictionary<string, int> attrEffect { get; protected set; }

        public List<int> excludeBuff { get; protected set; }

        public int name { get; protected set; }

        public int notifyEvent { get; protected set; }

        public int removeMode { get; protected set; }

        public List<int> replaceBuff { get; protected set; }

        public int sfx { get; protected set; }

        public int show { get; protected set; }

        public int showPriority { get; protected set; }

        public int tips { get; protected set; }

        public int totalTime { get; protected set; }

        public int vipLevel { get; protected set; }
    }
}


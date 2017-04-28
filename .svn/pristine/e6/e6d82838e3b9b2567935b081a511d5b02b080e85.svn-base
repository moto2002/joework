namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SkillData : Mogo.GameData.GameData<SkillData>
    {
        public static readonly string fileName = "xml/SkillData";
        public int totalActionDuration = 0;

        public static SkillData GetSkillByVLP(int vocation, int posi, int level)
        {
            foreach (KeyValuePair<int, SkillData> pair in Mogo.GameData.GameData<SkillData>.dataMap)
            {
                if ((((pair.Value.limitVocation == vocation) && (pair.Value.weapon == 0)) && (pair.Value.posi == posi)) && (pair.Value.level == level))
                {
                    return pair.Value;
                }
            }
            return null;
        }

        public static SkillData GetSkillByVWLP(int vocation, int weapon, int posi, int level)
        {
            foreach (KeyValuePair<int, SkillData> pair in Mogo.GameData.GameData<SkillData>.dataMap)
            {
                if ((((pair.Value.limitVocation == vocation) && (pair.Value.weapon == weapon)) && (pair.Value.posi == posi)) && (pair.Value.level == level))
                {
                    return pair.Value;
                }
            }
            return null;
        }

        public static SkillData GetStudySkillByVocationAndWeapon(int vocation, int weapon, int posi)
        {
            foreach (KeyValuePair<int, SkillData> pair in Mogo.GameData.GameData<SkillData>.dataMap)
            {
                if (((pair.Value.limitVocation == vocation) && (pair.Value.weapon == weapon)) && (pair.Value.posi == posi))
                {
                    return pair.Value;
                }
            }
            return null;
        }

        public static int GetTotalActionDuration(int skillId)
        {
            SkillData data = Mogo.GameData.GameData<SkillData>.dataMap[skillId];
            if (data.totalActionDuration == 0)
            {
                foreach (int num in data.skillAction)
                {
                    data.totalActionDuration += Mogo.GameData.GameData<SkillAction>.dataMap[num].actionBeginDuration;
                    data.totalActionDuration += Mogo.GameData.GameData<SkillAction>.dataMap[num].actionEndDuration;
                }
            }
            return data.totalActionDuration;
        }

        public int activeBuff { get; protected set; }

        public int activeBuffMode { get; protected set; }

        public int canLearn { get; protected set; }

        public int castRange { get; protected set; }

        public int castTime { get; protected set; }

        public List<int> cd { get; protected set; }

        public List<int> dependBuff { get; protected set; }

        public List<int> dependSkill { get; protected set; }

        public int desc { get; protected set; }

        public float extraHarm { get; protected set; }

        public float extraRate { get; protected set; }

        public int groupID { get; protected set; }

        public int icon { get; protected set; }

        public List<int> independBuff { get; protected set; }

        public int learnLimit { get; protected set; }

        public int level { get; protected set; }

        public int limitLevel { get; protected set; }

        public int limitVocation { get; protected set; }

        public int moneyCost { get; protected set; }

        public int name { get; protected set; }

        public int nextSkill { get; protected set; }

        public int posi { get; protected set; }

        public int pvpCreditCost { get; protected set; }

        public List<int> skillAction { get; protected set; }

        public int skillTreeID { get; protected set; }

        public int vocation { get; protected set; }

        public int weapon { get; protected set; }
    }
}


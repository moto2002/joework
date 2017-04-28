namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class WingLevelData : Mogo.GameData.GameData<WingLevelData>
    {
        public static readonly string fileName = "xml/WingLevel";

        public static WingLevelData GetLevelData(int id, int level)
        {
            foreach (KeyValuePair<int, WingLevelData> pair in Mogo.GameData.GameData<WingLevelData>.dataMap)
            {
                if ((pair.Value.wingId == id) && (pair.Value.level == level))
                {
                    return pair.Value;
                }
            }
            return null;
        }

        public PropertyEffectData GetProperEffectData()
        {
            return Mogo.GameData.GameData<PropertyEffectData>.dataMap.Get<int, PropertyEffectData>(this.propertEffectId);
        }

        public List<string> GetPropertEffect()
        {
            List<string> list = new List<string>();
            PropertyEffectData data = Mogo.GameData.GameData<PropertyEffectData>.dataMap.Get<int, PropertyEffectData>(this.propertEffectId);
            if (data.hpBase > 0)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68bd].Format(new object[] { data.hpBase }));
            }
            if (data.attackBase > 0)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68be].Format(new object[] { data.attackBase }));
            }
            if (data.defenseBase > 0)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68bf].Format(new object[] { data.defenseBase }));
            }
            if (data.crit > 0f)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68c0].Format(new object[] { data.crit }));
            }
            if (data.trueStrike > 0f)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68c1].Format(new object[] { data.trueStrike }));
            }
            if (data.critExtraAttack > 0)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68c2].Format(new object[] { data.critExtraAttack }));
            }
            if (data.antiDefense > 0)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68c3].Format(new object[] { data.antiDefense }));
            }
            if (data.antiCrit > 0)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68c4].Format(new object[] { data.antiCrit }));
            }
            if (data.antiTrueStrike > 0)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68c5].Format(new object[] { data.antiTrueStrike }));
            }
            if (data.extraCritRate > 0)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68c6].Format(new object[] { data.extraCritRate }));
            }
            if (data.extraTrueStrikeRate > 0)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68c7].Format(new object[] { data.extraTrueStrikeRate }));
            }
            if (data.damageReduceRate > 0)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68c8].Format(new object[] { data.damageReduceRate }));
            }
            if (data.pvpAddition > 0)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68c9].Format(new object[] { data.pvpAddition }));
            }
            if (data.pvpAnti > 0)
            {
                list.Add(Mogo.GameData.GameData<LanguageData>.dataMap[0x68ca].Format(new object[] { data.pvpAnti }));
            }
            return list;
        }

        public int buffDesc { get; protected set; }

        public Dictionary<int, int> buffId { get; protected set; }

        public int level { get; protected set; }

        public int levelExpAdd { get; protected set; }

        public int nextLevelExp { get; protected set; }

        public int propertEffectId { get; protected set; }

        public int quality { get; protected set; }

        public int scale { get; protected set; }

        public Dictionary<int, int> trainCost { get; protected set; }

        public int trainDiamondCost { get; protected set; }

        public Dictionary<int, int> trainValue { get; protected set; }

        public Dictionary<int, int> trainWeight { get; protected set; }

        public int wingId { get; protected set; }
    }
}


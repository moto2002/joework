namespace Mogo.GameData
{
    using Mogo.Util;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class PropertyEffectData : Mogo.GameData.GameData<PropertyEffectData>
    {
        public static readonly string fileName = "xml/PropertyEffect";

        public static float GetOneEffect(int _id)
        {
            LoggerHelper.Debug("effectID:" + _id, true, 0);
            PropertyEffectData data = Mogo.GameData.GameData<PropertyEffectData>.dataMap[_id];
            PropertyInfo[] properties = data.GetType().GetProperties(~BindingFlags.Static);
            foreach (PropertyInfo info in properties)
            {
                if ((info.PropertyType == typeof(int)) || (info.PropertyType == typeof(float)))
                {
                    float num;
                    object obj2 = info.GetGetMethod().Invoke(data, null);
                    if (info.PropertyType == typeof(int))
                    {
                        num = (int) obj2;
                    }
                    else
                    {
                        num = (float) obj2;
                    }
                    if ((num > 0f) && (num != data.id))
                    {
                        return num;
                    }
                }
            }
            return 0f;
        }

        public string GetOneEffectStr()
        {
            string str = "";
            if (this.hpBase > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0xc9].Format(new object[] { this.hpBase });
            }
            if (this.hpAddRate > 0f)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[250].Format(new object[] { this.hpAddRate / 100f });
            }
            if (this.attackBase > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0xca].Format(new object[] { this.attackBase });
            }
            if (this.attackAddRate > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0xfb].Format(new object[] { ((float) this.attackAddRate) / 100f });
            }
            if (this.defenseBase > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0xcb].Format(new object[] { ((float) this.defenseBase) / 100f });
            }
            if (this.speedAddRate > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0xfd].Format(new object[] { ((float) this.speedAddRate) / 100f });
            }
            if (this.hit > 0f)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0xcd].Format(new object[] { this.hit });
            }
            if (this.crit > 0f)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0xcc].Format(new object[] { this.crit });
            }
            if (this.trueStrike > 0f)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0xce].Format(new object[] { this.trueStrike });
            }
            if (this.critExtraAttack > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0xcf].Format(new object[] { this.critExtraAttack });
            }
            if (this.antiDefense > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0xd0].Format(new object[] { this.antiDefense });
            }
            if (this.antiTrueStrike > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[210].Format(new object[] { this.antiTrueStrike });
            }
            if (this.damageReduceRate > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0xfc].Format(new object[] { this.damageReduceRate });
            }
            if (this.cdReduce > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0xd4].Format(new object[] { this.cdReduce });
            }
            if (this.extraHitRate > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0x100].Format(new object[] { ((float) this.extraHitRate) / 100f });
            }
            if (this.extraCritRate > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0xff].Format(new object[] { ((float) this.extraCritRate) / 100f });
            }
            if (this.extraTrueStrikeRate > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0x101].Format(new object[] { ((float) this.extraTrueStrikeRate) / 100f });
            }
            if (this.pvpAddition > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0x102].Format(new object[] { this.pvpAddition });
            }
            if (this.pvpAnti > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[0x103].Format(new object[] { this.pvpAnti });
            }
            if (this.extraExpRate > 0)
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap[260].Format(new object[] { ((float) this.extraExpRate) / 100f });
            }
            if (this.extraGoldRate > 0)
            {
                str = Mogo.GameData.GameData<LanguageData>.dataMap[0x105].Format(new object[] { ((float) this.extraGoldRate) / 100f });
            }
            return str;
        }

        public static int GetPVPAddition(int _id)
        {
            if (Mogo.GameData.GameData<PropertyEffectData>.dataMap.ContainsKey(_id))
            {
                return Mogo.GameData.GameData<PropertyEffectData>.dataMap[_id].pvpAddition;
            }
            LoggerHelper.Error("no contains effectID:" + _id, true);
            return 0;
        }

        public static int GetPVPAnti(int _id)
        {
            if (Mogo.GameData.GameData<PropertyEffectData>.dataMap.ContainsKey(_id))
            {
                return Mogo.GameData.GameData<PropertyEffectData>.dataMap[_id].pvpAnti;
            }
            LoggerHelper.Error("no contains effectID:" + _id, true);
            return 0;
        }

        public int airDamage { get; protected set; }

        public int airDefense { get; protected set; }

        public int allElementsDamage { get; protected set; }

        public int allElementsDefense { get; protected set; }

        public int antiCrit { get; protected set; }

        public int antiDefense { get; protected set; }

        public int antiTrueStrike { get; protected set; }

        public int attackAddRate { get; protected set; }

        public int attackBase { get; protected set; }

        public int cdReduce { get; protected set; }

        public float crit { get; protected set; }

        public int critExtraAttack { get; protected set; }

        public int damageReduceRate { get; protected set; }

        public int defenseBase { get; protected set; }

        public int earthDamage { get; protected set; }

        public int earthDefense { get; protected set; }

        public int extraCritRate { get; protected set; }

        public int extraExpRate { get; protected set; }

        public int extraGoldRate { get; protected set; }

        public int extraHitRate { get; protected set; }

        public int extraTrueStrikeRate { get; protected set; }

        public int fireDamage { get; protected set; }

        public int fireDefense { get; protected set; }

        public float hit { get; protected set; }

        public float hpAddRate { get; protected set; }

        public int hpBase { get; protected set; }

        public int pvpAddition { get; protected set; }

        public int pvpAnti { get; protected set; }

        public int speedAddRate { get; protected set; }

        public float trueStrike { get; protected set; }

        public int waterDamage { get; protected set; }

        public int waterDefense { get; protected set; }
    }
}


namespace Mogo.GameData
{
    using Mogo.Util;
    using System;
    using System.Runtime.CompilerServices;

    public class LanguageData : Mogo.GameData.GameData<LanguageData>
    {
        public static readonly string fileName = "xml/ChineseData";

        public LanguageData()
        {
            this.content = string.Empty;
        }

        public string Format(params object[] args)
        {
            return string.Format(this.content, args);
        }

        public static string GetContent(int id)
        {
            if (Mogo.GameData.GameData<LanguageData>.dataMap.ContainsKey(id))
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap.Get<int, LanguageData>(id).content;
            }
            LoggerHelper.Error(string.Format("Language key {0:0} is not exist ", id), true);
            return "***";
        }

        public static string GetContent(int id, params object[] args)
        {
            if (Mogo.GameData.GameData<LanguageData>.dataMap.ContainsKey(id))
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap.Get<int, LanguageData>(id).Format(args);
            }
            LoggerHelper.Error(string.Format("Language key {0:0} is not exist ", id), true);
            return "***";
        }

        public static string GetPVPLevelName(int PVPLevel)
        {
            return Mogo.GameData.GameData<LanguageData>.dataMap.Get<int, LanguageData>((0xbb8 + PVPLevel)).content;
        }

        public string content { get; set; }

        public static string DIAMOND
        {
            get
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap.Get<int, LanguageData>(0x4e24).content;
            }
        }

        public static string EXP
        {
            get
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap.Get<int, LanguageData>(0x4e23).content;
            }
        }

        public static string MONEY
        {
            get
            {
                return Mogo.GameData.GameData<LanguageData>.dataMap.Get<int, LanguageData>(0x4e22).content;
            }
        }
    }
}


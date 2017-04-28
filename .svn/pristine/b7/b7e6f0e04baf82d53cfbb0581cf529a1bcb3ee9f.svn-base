namespace Mogo.GameData
{
    using Mogo.Util;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class UIMapData : Mogo.GameData.GameData<UIMapData>
    {
        public static readonly string fileName = "xml/UIMap";

        public static void FormatDataMapToSoundIDUINameMap()
        {
            soundIDUINameMap = new Dictionary<string, UIMapData>();
            foreach (KeyValuePair<int, UIMapData> pair in Mogo.GameData.GameData<UIMapData>.dataMap)
            {
                if (pair.Value.control == null)
                {
                    LoggerHelper.Warning("UIMap: key " + pair.Key + " is Empty!", true);
                }
                else
                {
                    string key = pair.Value.control.Split(new char[] { '_' })[0];
                    if (soundIDUINameMap.ContainsKey(key))
                    {
                        LoggerHelper.Warning(string.Concat(new object[] { "UIMap: key ", pair.Key, " has same control name: ", key }), true);
                    }
                    else
                    {
                        soundIDUINameMap.Add(key, pair.Value);
                    }
                }
            }
        }

        public string control { get; protected set; }

        public int downSoundID { get; protected set; }

        public static Dictionary<string, UIMapData> soundIDUINameMap
        {
            [CompilerGenerated]
            get
            {
                return <soundIDUINameMap>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <soundIDUINameMap>k__BackingField = value;
            }
        }

        public int upSoundID { get; protected set; }
    }
}


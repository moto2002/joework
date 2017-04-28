namespace Mogo.Util
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security;
    using UnityEngine;

    public class SystemConfig
    {
        public const string ASSET_FILE_EXTENSION = ".u";
        public const string ASSET_FILE_HEAD = "file://";
        public const string CFG_FILE = "cfg.xml";
        public const string CFG_PARENT_KEY = "parent";
        public static List<Mogo.Util.CfgInfo> CfgInfo;
        public static Dictionary<string, string> CfgInfoNew;
        public static readonly string CfgPath;
        public const string CONFIG_SUB_FOLDER = "data/";
        public static readonly string ConfigPath;
        public static string DataPath;
        public const string DEFINE_LIST_FILE_NAME = "entities";
        public static bool IsEditor;
        public const string LOGIN_MARKET_URL_KEY = "LoginMarketData";
        private static string m_CONFIG_FILE_EXTENSION;
        private static string m_resourceFolder;
        public const string MARKET_URL_KEY = "market";
        public const string MOGO_RESOURCE = "/MogoResource.xml";
        public const string NOTICE_CONTENT_KEY = "notice";
        public const string NOTICE_URL_KEY = "NoticeData";
        public static readonly string PackageMd5Path;
        public const string PLATFORM_360 = "360";
        public const string PLATFORM_37WAN = "37wan";
        public const string PLATFORM_37WANBY = "37wan";
        public const string PLATFORM_3G = "3g";
        public const string PLATFORM_ANZHI = "anzhi";
        public const string PLATFORM_BAIDU = "baiduqianbao";
        public const string PLATFORM_CW = "eone";
        public const string PLATFORM_DJ = "91";
        public const string PLATFORM_DK = "DK";
        public const string PLATFORM_DOWNJOY = "dangle";
        public const string PLATFORM_FN = "fn";
        public const string PLATFORM_HUAWEI = "huawei";
        public const string PLATFORM_JINSHAN = "jinshan";
        public const string PLATFORM_KK = "keke";
        public const string PLATFORM_KUGOU = "kugou";
        public const string PLATFORM_LENOVO = "lenovo";
        public const string PLATFORM_MI = "mi";
        public const string PLATFORM_PPS = "pps";
        public const string PLATFORM_PPTV = "pptv";
        public const string PLATFORM_SSJJ = "4399";
        public const string PLATFORM_UC = "uc";
        public const string PLATFORM_VIVO = "vivo";
        public const string PLATFORM_WANPU = "wanpu";
        public const string PLATFORM_YOUMI = "youmi";
        public static Dictionary<string, string> PlatformDic;
        public const string SERVER_GROUP_URL_KEY = "servergroup";
        public const string SERVER_LIST_URL_KEY = "serverlist";
        public static readonly string ServerVersionPath;
        public static readonly string SystemSwitchPath;
        public const string VERSION_URL_KEY = "version";
        public static readonly string VersionPath;
        public static readonly string VoiceMsgPath;
        public const string XML = ".xml";

        static SystemConfig()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("com.zsgl.sy4399", "4399");
            dictionary.Add("com.zsgl.gdt.sy4399", "4399");
            dictionary.Add("com.zsgl.qx.sy4399", "4399");
            dictionary.Add("com.zsgl.dj", "91");
            dictionary.Add("com.zsgl.pptv", "pptv");
            dictionary.Add("com.zsgl.downjoy", "dangle");
            dictionary.Add("com.zsgl.uc", "uc");
            dictionary.Add("com.zsgl.qihoo360", "360");
            dictionary.Add("com.zsgl.mi", "mi");
            dictionary.Add("com.zsgl.pps", "pps");
            dictionary.Add("com.zsgl.anzhi", "anzhi");
            dictionary.Add("com.zsgl.s91", "91");
            dictionary.Add("com.zsgl.DK", "DK");
            dictionary.Add("com.zsgl.cw", "eone");
            dictionary.Add("com.zsgl.nearme.gamecenter", "keke");
            dictionary.Add("com.zsgl.jiubang3g", "3g");
            dictionary.Add("com.zsgl.lenovo", "lenovo");
            dictionary.Add("com.zsgl.HUAWEI", "huawei");
            dictionary.Add("com.zsgl.wanpu", "wanpu");
            dictionary.Add("com.zsgl.baidu", "baiduqianbao");
            dictionary.Add("com.zsgl.sqwan", "37wan");
            dictionary.Add("com.zsgl.sqwanby", "37wan");
            dictionary.Add("com.zsgl.kugou", "kugou");
            dictionary.Add("com.zsgl.youmi", "youmi");
            dictionary.Add("com.zsgl.vivo", "vivo");
            dictionary.Add("com.zsgl.jinshan", "jinshan");
            dictionary.Add("com.zsgl.union", "fn");
            PlatformDic = dictionary;
            VoiceMsgPath = Application.persistentDataPath + "/voiceMsg";
            ConfigPath = Application.persistentDataPath + "/config.xml";
            VersionPath = Application.persistentDataPath + "/version.xml";
            CfgPath = Application.persistentDataPath + "/" + "cfg.xml";
            PackageMd5Path = Application.persistentDataPath + "/packagemd5.xml";
            ServerVersionPath = Application.persistentDataPath + "/server_version.xml";
            SystemSwitchPath = Application.persistentDataPath + "/SystemSwitch.xml";
            CfgInfo = new List<Mogo.Util.CfgInfo>();
            CfgInfoNew = new Dictionary<string, string>();
        }

        public static string GetCfgInfoUrl(string key)
        {
            string str = "";
            if (CfgInfoNew.ContainsKey(key))
            {
                str = CfgInfoNew[key];
            }
            return str;
        }

        public static void Init(Action<bool> callback)
        {
            try
            {
                LoadCfgInfo(callback);
            }
            catch (Exception exception)
            {
                LoggerHelper.Except(exception, null);
            }
        }

        private static void InitConfig()
        {
            if (File.Exists(ConfigPath))
            {
                File.Delete(ConfigPath);
            }
            SaveConfig();
        }

        public static void LoadCfgInfo(Action<bool> callback)
        {
            Action action = null;
            Action action2 = null;
            if (File.Exists(CfgPath))
            {
                CfgInfoNew = LoadCfgInfoList(Utils.LoadFile(CfgPath));
                if (callback != null)
                {
                    if (action == null)
                    {
                        action = () => callback((CfgInfoNew != null) && (CfgInfoNew.Count > 0));
                    }
                    DriverLib.Invoke(action);
                }
            }
            else
            {
                string url = Utils.LoadResource(Utils.GetFileNameWithoutExtention("cfg.xml", '/'));
                string programVerStr = "";
                TextAsset asset = Resources.Load("version") as TextAsset;
                if (!((asset == null) || string.IsNullOrEmpty(asset.text)))
                {
                    programVerStr = "V" + VersionManager.Instance.GetVersionInXML(asset.text).ProgramVersion;
                }
                if (action2 == null)
                {
                    action2 = delegate {
                        if (callback != null)
                        {
                            DriverLib.Invoke(() => callback(false));
                        }
                    };
                }
                Action erraction = action2;
                Action<string> suaction = null;
                suaction = delegate (string res) {
                    Dictionary<string, string> dictionary = LoadCfgInfoList(res);
                    foreach (KeyValuePair<string, string> pair in dictionary)
                    {
                        if (!CfgInfoNew.ContainsKey(pair.Key))
                        {
                            CfgInfoNew.Add(pair.Key, pair.Value);
                        }
                    }
                    if (dictionary.ContainsKey(programVerStr))
                    {
                        CfgInfoNew.Clear();
                        DownloadMgr.Instance.AsynDownLoadText(dictionary[programVerStr], suaction, erraction);
                    }
                    else if (dictionary.ContainsKey("parent"))
                    {
                        DownloadMgr.Instance.AsynDownLoadText(dictionary["parent"], suaction, erraction);
                    }
                    else if (callback != null)
                    {
                        DriverLib.Invoke(() => callback((CfgInfoNew != null) && (CfgInfoNew.Count > 0)));
                    }
                };
                DownloadMgr.Instance.AsynDownLoadText(url, suaction, erraction);
            }
        }

        private static Dictionary<string, string> LoadCfgInfoList(string text)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            SecurityElement element = XMLParser.LoadXML(text);
            if (((element != null) && (element.Children != null)) && (element.Children.Count != 0))
            {
                foreach (SecurityElement element2 in element.Children)
                {
                    dictionary[element2.Tag] = element2.Text;
                }
            }
            return dictionary;
        }

        private static List<T> LoadXML<T>(string path)
        {
            return LoadXMLText<T>(Utils.LoadFile(path));
        }

        private static List<T> LoadXMLText<T>(string text)
        {
            Exception exception;
            List<T> list = new List<T>();
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return list;
                }
                System.Type type = typeof(T);
                Dictionary<int, Dictionary<string, string>> dictionary = XMLParser.LoadIntMap(XMLParser.LoadXML(text), text);
                PropertyInfo[] properties = type.GetProperties(~BindingFlags.Static);
                foreach (KeyValuePair<int, Dictionary<string, string>> pair in dictionary)
                {
                    object obj2 = type.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                    foreach (PropertyInfo info in properties)
                    {
                        if (info.Name == "id")
                        {
                            info.SetValue(obj2, pair.Key, null);
                        }
                        else
                        {
                            try
                            {
                                if (pair.Value.ContainsKey(info.Name))
                                {
                                    object obj3 = Utils.GetValue(pair.Value[info.Name], info.PropertyType);
                                    info.SetValue(obj2, obj3, null);
                                }
                            }
                            catch (Exception exception1)
                            {
                                exception = exception1;
                                LoggerHelper.Debug(string.Concat(new object[] { "LoadXML error: ", pair.Value[info.Name], " ", info.PropertyType }), true, 0);
                                LoggerHelper.Except(exception, null);
                            }
                        }
                    }
                    list.Add((T) obj2);
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                LoggerHelper.Except(exception, null);
                LoggerHelper.Error("error text: \n" + text, true);
            }
            return list;
        }

        public static void SaveCfgInfo()
        {
        }

        public static void SaveConfig()
        {
        }

        private static void SaveXML(object config, string path)
        {
            SecurityElement element = new SecurityElement("root");
            element.AddChild(new SecurityElement("record"));
            SecurityElement element2 = element.Children[0] as SecurityElement;
            PropertyInfo[] properties = config.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                if (info.Name.Contains("GuideTimes"))
                {
                    object obj2 = info.GetGetMethod().Invoke(config, null);
                    string text = "";
                    foreach (KeyValuePair<ulong, string> pair in obj2 as Dictionary<ulong, string>)
                    {
                        text = text + pair.Key.ToString() + ":" + pair.Value + ",";
                    }
                    element2.AddChild(new SecurityElement(info.Name, text));
                }
                else
                {
                    object obj3 = info.GetGetMethod().Invoke(config, null);
                    element2.AddChild(new SecurityElement(info.Name, obj3.ToString()));
                }
            }
            XMLParser.SaveText(path, element.ToString());
        }

        private static void SaveXMLList<T>(string path, List<T> data, string attrName = "record")
        {
            SecurityElement element = new SecurityElement("root");
            int num = 0;
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (T local in data)
            {
                SecurityElement child = new SecurityElement(attrName);
                foreach (PropertyInfo info in properties)
                {
                    System.Type propertyType = info.PropertyType;
                    string text = string.Empty;
                    object obj2 = info.GetGetMethod().Invoke(local, null);
                    if (propertyType.IsGenericType && (propertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
                    {
                        text = typeof(Utils).GetMethod("PackMap").MakeGenericMethod(propertyType.GetGenericArguments()).Invoke(null, new object[] { obj2, ':', ',' }).ToString();
                    }
                    else if (propertyType.IsGenericType && (propertyType.GetGenericTypeDefinition() == typeof(List<>)))
                    {
                        text = typeof(Utils).GetMethod("PackList").MakeGenericMethod(propertyType.GetGenericArguments()).Invoke(null, new object[] { obj2, ',' }).ToString();
                    }
                    else
                    {
                        text = obj2.ToString();
                    }
                    child.AddChild(new SecurityElement(info.Name, text));
                }
                element.AddChild(child);
                num++;
            }
            XMLParser.SaveText(path, element.ToString());
        }

        public static void SetConfig()
        {
            Process.Start("Explorer.exe", Application.persistentDataPath.Replace('/', '\\'));
        }

        public static string AndroidPath
        {
            get
            {
                return (Application.persistentDataPath + "/MogoResources/");
            }
        }

        public static string CONFIG_FILE_EXTENSION
        {
            get
            {
                if (m_CONFIG_FILE_EXTENSION == null)
                {
                    m_CONFIG_FILE_EXTENSION = (SystemSwitch.ReleaseMode || IsEditor) ? ".xml" : string.Empty;
                }
                return m_CONFIG_FILE_EXTENSION;
            }
        }

        public static string ENTITY_DEFS_PATH
        {
            get
            {
                return "entity_defs/";
            }
        }

        public static string IOSPath
        {
            get
            {
                return (Application.persistentDataPath + "/MogoResources/");
            }
        }

        public static bool IsUseOutterConfig
        {
            get
            {
                LoggerHelper.Debug("Application.platform: " + Application.platform, true, 0);
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (Directory.Exists(AndroidPath + "data/"))
                    {
                        return true;
                    }
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (Directory.Exists(IOSPath + "data/"))
                    {
                        return true;
                    }
                }
                else if ((Application.platform == RuntimePlatform.WindowsPlayer) && Directory.Exists(PCPath + "data/"))
                {
                    return true;
                }
                return false;
            }
        }

        public static string OutterPath
        {
            get
            {
                LoggerHelper.Debug("Application.platform: " + Application.platform, true, 0);
                if (Application.platform == RuntimePlatform.Android)
                {
                    return AndroidPath;
                }
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return IOSPath;
                }
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    return PCPath;
                }
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    return PCPath;
                }
                if (Application.platform == RuntimePlatform.OSXPlayer)
                {
                    return IOSPath;
                }
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    return IOSPath;
                }
                return "";
            }
        }

        public static string PCPath
        {
            get
            {
                return (Application.dataPath + "/../MogoResources/");
            }
        }

        public static string ResourceFolder
        {
            get
            {
                if (m_resourceFolder == null)
                {
                    if (SystemSwitch.ReleaseMode)
                    {
                        m_resourceFolder = OutterPath;
                    }
                    else
                    {
                        m_resourceFolder = string.Empty;
                    }
                }
                return m_resourceFolder;
            }
            set
            {
            }
        }
    }
}


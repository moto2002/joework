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
        public const string ACTIVATE_URL = "ActivateUrl";
        public const string ASSET_FILE_EXTENSION = ".u";
        public const string ASSET_FILE_HEAD = "file://";
        public const string ATIVATE_URL_TEMPLATE = "{0}server={1}&dbid={2}&serial_number={3}";
        public const string CFG_FILE = "cfg.xml";
        public static List<Mogo.Util.CfgInfo> CfgInfo;
        public static readonly string CfgPath;
        public const string CONFIG_SUB_FOLDER = "data/";
        public static readonly string ConfigPath;
        public const string DEFINE_LIST_FILE_NAME = "entities";
        public static readonly string LocalServerListPath;
        public const string LOGIN_MARKET_URL_KEY = "LoginMarketData";
        private static LocalSetting m_instance;
        private static string m_resourceFolder;
        private static int m_selectedServerIndex;
        public const string MARKET_URL_KEY = "market";
        public const string MOGO_RESOURCE = "/MogoResource.xml";
        public const string NOTICE_CONTENT_KEY = "notice";
        public const string NOTICE_URL_KEY = "NoticeData";
        public static readonly string PackageMd5Path;
        public const string PLATFORM_360 = "360";
        public const string PLATFORM_ANZHI = "anzhi";
        public const string PLATFORM_DJ = "91";
        public const string PLATFORM_DK = "DK";
        public const string PLATFORM_DOWNJOY = "dangle";
        public const string PLATFORM_MI = "mi";
        public const string PLATFORM_PPS = "pps";
        public const string PLATFORM_PPTV = "pptv";
        public const string PLATFORM_SSJJ = "4399";
        public const string PLATFORM_UC = "uc";
        public static Dictionary<string, string> PlatformDic;
        public const string SERVER_LIST_URL_KEY = "serverlist";
        public static readonly string ServerListPath;
        public static List<ServerInfo> ServersList;
        public static readonly string ServerVersionPath;
        public static readonly string SystemSwitchPath;
        public const string VERSION_URL_KEY = "version";
        public static readonly string VersionPath;
        public const string XML = ".xml";

        static SystemConfig()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("com.ahzs.sy4399", "4399");
            dictionary.Add("com.ahzs.dj", "91");
            dictionary.Add("com.ahzs.pptv", "pptv");
            dictionary.Add("com.ahzs.downjoy", "dangle");
            dictionary.Add("com.ahzs.uc", "uc");
            dictionary.Add("com.ahzs.qihoo360", "360");
            dictionary.Add("com.ahzs.mi", "mi");
            dictionary.Add("com.ahzs.pps", "pps");
            dictionary.Add("com.ahzs.anzhi", "anzhi");
            dictionary.Add("com.ahzs.jiuyi", "91");
            dictionary.Add("com.ahzs.DK", "DK");
            PlatformDic = dictionary;
            ConfigPath = Application.get_persistentDataPath() + "/config.xml";
            ServerListPath = Application.get_persistentDataPath() + "/server.xml";
            LocalServerListPath = Application.get_persistentDataPath() + "/localserver.xml";
            VersionPath = Application.get_persistentDataPath() + "/version.xml";
            CfgPath = Application.get_persistentDataPath() + "/" + "cfg.xml";
            PackageMd5Path = Application.get_persistentDataPath() + "/packagemd5.xml";
            ServerVersionPath = Application.get_persistentDataPath() + "/server_version.xml";
            SystemSwitchPath = Application.get_persistentDataPath() + "/SystemSwitch.xml";
            CfgInfo = new List<Mogo.Util.CfgInfo>();
        }

        public static string GetActivateKeyUrl(ulong dbid, string key)
        {
            ServerInfo selectedServerInfo = GetSelectedServerInfo();
            return string.Format("{0}server={1}&dbid={2}&serial_number={3}", new object[] { GetCfgInfoUrl("ActivateUrl"), selectedServerInfo.ip, dbid, key });
        }

        public static string GetCfgInfoUrl(string key)
        {
            foreach (Mogo.Util.CfgInfo info in CfgInfo)
            {
                if (info.name == key)
                {
                    return info.url;
                }
            }
            return "";
        }

        public static ServerInfo GetRecommentServer()
        {
            foreach (ServerInfo info in ServersList)
            {
                if (info.flag == 5)
                {
                    return info;
                }
            }
            foreach (ServerInfo info in ServersList)
            {
                if (info.flag == 1)
                {
                    return info;
                }
            }
            foreach (ServerInfo info in ServersList)
            {
                if (info.flag == 2)
                {
                    return info;
                }
            }
            foreach (ServerInfo info in ServersList)
            {
                if (info.flag == 4)
                {
                    return info;
                }
            }
            foreach (ServerInfo info in ServersList)
            {
                if (info.flag == 3)
                {
                    return info;
                }
            }
            return null;
        }

        public static ServerInfo GetSelectedServerInfo()
        {
            foreach (ServerInfo info in ServersList)
            {
                if (info.id == Instance.SelectedServer)
                {
                    return info;
                }
            }
            return null;
        }

        public static int GetServerIndexById(int id)
        {
            for (int i = 0; i < ServersList.Count; i++)
            {
                if (ServersList[i].id == id)
                {
                    return i;
                }
            }
            return -1;
        }

        public static ServerInfo GetServerInfoByIndex(int index)
        {
            if ((index >= 0) && (index < ServersList.Count))
            {
                return ServersList[index];
            }
            return null;
        }

        public static bool IfServerInfoExist(int id)
        {
            foreach (ServerInfo info in ServersList)
            {
                if (info.id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Init()
        {
            Exception exception;
            try
            {
                bool flag = LoadCfgInfo();
            }
            catch (Exception exception1)
            {
                exception = exception1;
                LoggerHelper.Except(exception, null);
            }
            try
            {
                LoadConfig();
            }
            catch (Exception exception2)
            {
                exception = exception2;
                LoggerHelper.Except(exception, null);
            }
            return true;
        }

        private static void InitConfig()
        {
            if (File.Exists(ConfigPath))
            {
                File.Delete(ConfigPath);
            }
            SaveConfig();
        }

        public static bool LoadCfgInfo()
        {
            string str;
            if (File.Exists(CfgPath))
            {
                str = Utils.LoadFile(CfgPath);
            }
            else
            {
                string url = Utils.LoadResource(Utils.GetFileNameWithoutExtention("cfg.xml", '/'));
                str = DownloadMgr.Instance.DownLoadText(url);
            }
            CfgInfo = LoadXMLText<Mogo.Util.CfgInfo>(str);
            return ((CfgInfo != null) && (CfgInfo.Count > 0));
        }

        private static void LoadConfig()
        {
            try
            {
                List<LocalSetting> list = LoadXML<LocalSetting>(ConfigPath);
                if ((list == null) || (list.Count == 0))
                {
                    InitConfig();
                }
                else
                {
                    Instance = list[0];
                }
            }
            catch (Exception exception)
            {
                InitConfig();
                LoggerHelper.Except(exception, null);
            }
        }

        public static void LoadServerList()
        {
            try
            {
                List<ServerInfo> list;
                if (File.Exists(LocalServerListPath))
                {
                    list = LoadXML<ServerInfo>(LocalServerListPath);
                }
                else
                {
                    string cfgInfoUrl = GetCfgInfoUrl("serverlist");
                    if (!string.IsNullOrEmpty(cfgInfoUrl))
                    {
                        DownloadMgr.Instance.DownloadFile(cfgInfoUrl, ServerListPath, ServerListPath + "bak");
                    }
                    list = LoadXML<ServerInfo>(ServerListPath);
                }
                if (list.Count != 0)
                {
                    ServersList = list;
                }
                for (int i = 0; i < ServersList.Count; i++)
                {
                    if (ServersList[i].id == Instance.SelectedServer)
                    {
                        SelectedServerIndex = i;
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Except(exception, null);
            }
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
                Type type = typeof(T);
                Dictionary<int, Dictionary<string, string>> dictionary = XMLParser.LoadIntMap(XMLParser.LoadXML(text), text);
                PropertyInfo[] properties = type.GetProperties(~BindingFlags.Static);
                foreach (KeyValuePair<int, Dictionary<string, string>> pair in dictionary)
                {
                    object obj2 = type.GetConstructor(Type.EmptyTypes).Invoke(null);
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
            SaveXMLList<Mogo.Util.CfgInfo>(CfgPath, CfgInfo, "url");
        }

        public static void SaveConfig()
        {
            SaveXMLList<LocalSetting>(ConfigPath, new List<LocalSetting> { Instance }, "record");
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
                    Type propertyType = info.PropertyType;
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
            Process.Start("Explorer.exe", Application.get_persistentDataPath().Replace('/', '\\'));
        }

        public static string AndroidPath
        {
            get
            {
                return (Application.get_persistentDataPath() + "/MogoResources/");
            }
        }

        public static string CONFIG_FILE_EXTENSION
        {
            get
            {
                return (SystemSwitch.ReleaseMode ? ".xml" : string.Empty);
            }
        }

        public static string ENTITY_DEFS_PATH
        {
            get
            {
                return "entity_defs/";
            }
        }

        public static LocalSetting Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new LocalSetting();
                }
                return m_instance;
            }
            set
            {
                m_instance = value;
            }
        }

        public static string IOSPath
        {
            get
            {
                return (Application.get_persistentDataPath() + "/MogoResources/");
            }
        }

        public static bool IsUseOutterConfig
        {
            get
            {
                LoggerHelper.Debug("Application.platform: " + Application.get_platform(), true, 0);
                if (Application.get_platform() == 11)
                {
                    if (Directory.Exists(AndroidPath + "data/"))
                    {
                        return true;
                    }
                }
                else if (Application.get_platform() == 8)
                {
                    if (Directory.Exists(IOSPath + "data/"))
                    {
                        return true;
                    }
                }
                else if ((Application.get_platform() == 2) && Directory.Exists(PCPath + "data/"))
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
                LoggerHelper.Debug("Application.platform: " + Application.get_platform(), true, 0);
                if (Application.get_platform() == 11)
                {
                    return AndroidPath;
                }
                if (Application.get_platform() == 8)
                {
                    return IOSPath;
                }
                if (Application.get_platform() == 2)
                {
                    return PCPath;
                }
                if (Application.get_platform() == 7)
                {
                    return PCPath;
                }
                if (Application.get_platform() == 0)
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
                string str = Application.get_dataPath() + "/../MogoResources/";
                LoggerHelper.Debug("PcPath: " + str, true, 0);
                return str;
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

        public static int SelectedServerIndex
        {
            get
            {
                return m_selectedServerIndex;
            }
            set
            {
                m_selectedServerIndex = value;
            }
        }
    }
}


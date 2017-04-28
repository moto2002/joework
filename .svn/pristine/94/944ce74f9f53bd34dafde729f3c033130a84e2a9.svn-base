using Mogo.Util;
using System;
using System.IO;
using System.Reflection;
using System.Security;

public class SystemSwitch
{
    private static bool m_destroyAllUI = false;
    private static bool m_destroyResource = false;
    private static bool m_releaseMode = true;
    private static bool m_useFileSystem = true;
    private static bool m_useHmf = true;
    private static bool m_usePlatformSDK = true;

    public static void InitSystemSwitch()
    {
        string str;
        if (File.Exists(SystemConfig.SystemSwitchPath))
        {
            str = Utils.LoadFile(SystemConfig.SystemSwitchPath);
        }
        else
        {
            str = Utils.LoadResource(Utils.GetFileNameWithoutExtention(SystemConfig.SystemSwitchPath, '/'));
        }
        if (!string.IsNullOrEmpty(str))
        {
            try
            {
                SecurityElement element = XMLParser.LoadXML(str);
                PropertyInfo[] properties = typeof(SystemSwitch).GetProperties();
                foreach (SecurityElement element2 in element.Children)
                {
                    foreach (PropertyInfo info in properties)
                    {
                        if (element2.Tag == info.Name)
                        {
                            info.SetValue(null, Utils.GetValue(element2.Text, typeof(bool)), null);
                            break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Except(exception, null);
            }
        }
    }

    public static bool DestroyAllUI
    {
        get
        {
            return m_destroyAllUI;
        }
        private set
        {
            m_destroyAllUI = value;
        }
    }

    public static bool DestroyResource
    {
        get
        {
            return m_destroyResource;
        }
        private set
        {
            m_destroyResource = value;
        }
    }

    public static bool ReleaseMode
    {
        get
        {
            return m_releaseMode;
        }
        private set
        {
            m_releaseMode = value;
        }
    }

    public static bool UseFileSystem
    {
        get
        {
            return m_useFileSystem;
        }
        private set
        {
            m_useFileSystem = value;
        }
    }

    public static bool UseHmf
    {
        get
        {
            return m_useHmf;
        }
        private set
        {
            m_useHmf = value;
        }
    }

    public static bool UsePlatformSDK
    {
        get
        {
            return m_usePlatformSDK;
        }
        private set
        {
            m_usePlatformSDK = value;
        }
    }
}


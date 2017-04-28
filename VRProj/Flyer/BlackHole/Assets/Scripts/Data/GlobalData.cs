using System;
using UnityEngine;
using System.Collections;
using System.Security;

public class GlobalData
{
    public static PlayGameRecordMgr m_pgrMgr;

    public static CfgPlayTimeMgr m_ptMgr;

    public static bool m_isFirstEntered = true;

    public static string m_cost;//玩游戏的花费

    /// <summary>
    /// 应用程序内容路径
    /// </summary>
    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
                path = GameConst.Exe_Path;
                break;
            case RuntimePlatform.WindowsPlayer:
                string _path = System.Environment.CurrentDirectory;
                //Debug.Log(_path);
                _path = _path.Substring(0, _path.LastIndexOf(@"\") + 1);
                //Debug.Log(_path);
                path = _path;
                break;
            default:
                // path = GameConst.Exe_Path;
                break;
        }
        return path;
    }

    public static  string GetTimeSpan(TimeSpan ts)
    {
        if (ts.Days > 0)
        {
            return ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分" + ts.Seconds.ToString() + "秒";
        }
        else if (ts.Hours > 0)
        {
            return ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分" + ts.Seconds.ToString() + "秒";
        }
        else if (ts.Minutes > 0)
        {
            return ts.Minutes.ToString() + "分" + ts.Seconds.ToString() + "秒";
        }
        else
        {
            return ts.Seconds.ToString() + "秒";
        }
    }
}

using UnityEngine;
using System.Collections;

public class GameConst
{
    public static string Exe_Path = @"E:\vr_game_output\";//暂时用绝对路径，以后发布后，换成相对路径

    public static string UI_Icon_Path = Application.dataPath + "/StreamingAssets/UI/Icon/";

    public static string UI_ScreenShot_Path =  Application.dataPath + "/StreamingAssets/UI/ScreenShot/";

    public static string Record_Cache_Path = Application.persistentDataPath + "/";

    public static string Cfg_Path = Application.persistentDataPath;

    public static string Record_Name = "record.xml";

    public static string Play_Time = "playtime.xml";
}

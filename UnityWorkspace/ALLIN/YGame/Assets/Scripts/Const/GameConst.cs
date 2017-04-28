using UnityEngine;
using System.Collections;
using ShiHuanJue.Debuger;

public class GameConst 
{
    public const int GameFrameRate = 30;                       //游戏帧频
    public const LogLevel CurrentLogLevel = LogLevel.All;      //log的显示等级       
    public const string AppName = "shihuanjue";                //应用程序名称	
    public const string AppPrefix = AppName + "_";             //应用程序前缀
    public const string ExtName = ".unity3d";                  //素材扩展名
    public const string AssetDirName = "StreamingAssets";      //素材目录 
    public const string WebUrl = @"http://120.26.71.230/GameRes/";  //web服务器
    public const bool UpdateMode = true;//暂时默认为开启                       //更新模式-默认关闭 
    public const bool DebugMode = true;
    public static string UserId = string.Empty;                 //用户ID
    public static int SocketPort = 0;                           //Socket服务器端口
    public static string SocketAddress = string.Empty;          //Socket服务器地址

	
}

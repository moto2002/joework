using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using System.Text;
using ShiHuanJue.Debuger;

public class Util
{
    private static uint uniqueId = 1;
    public static string UniqueString
    {
        get
        {
            return "unique_" + uniqueId++;
        }
    }

    public static string GetDirectoryName(string fileName)
    {
        return fileName.Substring(0, fileName.LastIndexOf('/'));
    }

    public static byte[] LoadByteFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            return File.ReadAllBytes(fileName);
        }
        return null;
    }

    public static string LoadFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            using (StreamReader reader = File.OpenText(fileName))
            {
                return reader.ReadToEnd();
            }
        }
        return string.Empty;
    }

    public static byte[] LoadByteResource(string fileName)
    {
        TextAsset asset = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
        byte[] buffer = asset.bytes;
        Resources.UnloadAsset(asset);
        return buffer;
    }

    public static string LoadFileResource(string fileName)
    {
        TextAsset asset = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
        string str = asset.text;
        Resources.UnloadAsset(asset);
        return str;
    }

    /// <summary>
    /// 生成文件的MD5
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static string BuildFileMd5(string filename)
    {
        string str = null;
        try
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                str = FormatMD5(MD5.Create().ComputeHash(stream));
            }
        }
        catch (Exception exception)
        {
            LogHelper.LogError(exception, null);
        }
        return str;
    }

    /// <summary>
    /// 格式MD5
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static string FormatMD5(byte[] data)
    {
        return BitConverter.ToString(data).Replace("-", "").ToLower();
    }

    /// <summary>
    /// HashToMD5Hex
    /// </summary>
    public static string HashToMD5Hex(string sourceStr)
    {
        byte[] Bytes = Encoding.UTF8.GetBytes(sourceStr);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] result = md5.ComputeHash(Bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
                builder.Append(result[i].ToString("x2"));
            return builder.ToString();
        }
    }

    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    public static string md5(string source)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }

    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

    /// <summary>
    /// 解析Quaternion
    /// </summary>
    /// <param name="_inputString"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ParseQuaternion(string _inputString, out Quaternion result)
    {
        string str = _inputString.Trim();
        result = new Quaternion();
        if (str.Length < 9)
        {
            return false;
        }
        try
        {
            string[] strArray = str.Split(new char[] { ',' });
            if (strArray.Length != 4)
            {
                return false;
            }
            result.x = float.Parse(strArray[0]);
            result.y = float.Parse(strArray[1]);
            result.z = float.Parse(strArray[2]);
            result.w = float.Parse(strArray[3]);
            return true;
        }
        catch (Exception exception)
        {
            LogHelper.LogError("Parse Quaternion error: " + str + exception.ToString());
            return false;
        }
    }

    /// <summary>
    /// 解析Vector3
    /// </summary>
    /// <param name="_inputString"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ParseVector3(string _inputString, out Vector3 result)
    {
        string str = _inputString.Trim();
        result = new Vector3();
        if (str.Length < 7)
        {
            return false;
        }
        try
        {
            string[] strArray = str.Split(new char[] { ',' });
            if (strArray.Length != 3)
            {
                return false;
            }
            result.x = float.Parse(strArray[0]);
            result.y = float.Parse(strArray[1]);
            result.z = float.Parse(strArray[2]);
            return true;
        }
        catch (Exception exception)
        {
            LogHelper.LogError("Parse Vector3 error: " + str + exception.ToString());
            return false;
        }
    }

    /// <summary>
    /// 获取时间
    /// </summary>
    /// <returns>距1970.1.1相差的秒数</returns>
    public static long GetTime()
    {
        TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
        return (long)ts.TotalMilliseconds;
    }

    /// <summary>
    /// 搜索子物体组件-GameObject版
    /// </summary>
    public static T Get<T>(GameObject go, string subnode) where T : Component
    {
        if (go != null)
        {
            Transform sub = go.transform.FindChild(subnode);
            if (sub != null) return sub.GetComponent<T>();
        }
        return null;
    }

    /// <summary>
    /// 搜索子物体组件-Transform版
    /// </summary>
    public static T Get<T>(Transform go, string subnode) where T : Component
    {
        if (go != null)
        {
            Transform sub = go.FindChild(subnode);
            if (sub != null) return sub.GetComponent<T>();
        }
        return null;
    }

    /// <summary>
    /// 搜索子物体组件-Component版
    /// </summary>
    public static T Get<T>(Component go, string subnode) where T : Component
    {
        return go.transform.FindChild(subnode).GetComponent<T>();
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    public static T Add<T>(GameObject go) where T : Component
    {
        if (go != null)
        {
            T[] ts = go.GetComponents<T>();
            for (int i = 0; i < ts.Length; i++)
            {
                if (ts[i] != null)
                    UnityEngine.Object.Destroy(ts[i]);
            }
            return go.gameObject.AddComponent<T>();
        }
        return null;
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    public static T Add<T>(Transform go) where T : Component
    {
        return Add<T>(go.gameObject);
    }

    /// <summary>
    /// 查找子对象
    /// </summary>
    public static GameObject Child(GameObject go, string subnode)
    {
        return Child(go.transform, subnode);
    }

    /// <summary>
    /// 查找子对象
    /// </summary>
    public static GameObject Child(Transform go, string subnode)
    {
        Transform tran = go.FindChild(subnode);
        if (tran == null) return null;
        return tran.gameObject;
    }

    /// <summary>
    /// 取平级对象
    /// </summary>
    public static GameObject Peer(GameObject go, string subnode)
    {
        return Peer(go.transform, subnode);
    }

    /// <summary>
    /// 取平级对象
    /// </summary>
    public static GameObject Peer(Transform go, string subnode)
    {
        Transform tran = go.parent.FindChild(subnode);
        if (tran == null) return null;
        return tran.gameObject;
    }

    /// <summary>
    /// 清除所有子节点
    /// </summary>
    public static void ClearChild(Transform go)
    {
        if (go == null) return;
        for (int i = go.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(go.GetChild(i).gameObject);
        }
    }



    /// <summary>
    /// 应用程序内容路径
    /// </summary>
    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;
            default:
                path = "file://" + Application.dataPath + "/" + AppConst.AssetDirName + "/";
                break;
        }
        return path;
    }

    public static string GetRelativePath()
    {
        //if (Application.isEditor)
        //    return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/") + "/Assets/" + AppConst.AssetDirName + "/";
        //else if (Application.isMobilePlatform || Application.isConsolePlatform)
        //    return "file:///" + DataPath;
        //else // For standalone player.
        //    return "file://" + Application.streamingAssetsPath + "/";

        return "file:///" + DataPath;
    }

    /// <summary>
    /// 网络可用
    /// </summary>
    public static bool NetAvailable
    {
        get
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }

    /// <summary>
    /// 是否是无线
    /// </summary>
    public static bool IsWifi
    {
        get
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }
    }

    /// <summary>
    /// 是否是登录场景
    /// </summary>
    public static bool isLogin
    {
        get { return Application.loadedLevelName.CompareTo("login") == 0; }
    }


    /// <summary>
    /// 取得数据存放目录
    /// </summary>
    public static string DataPath
    {
        get
        {
            string game = AppConst.AppName.ToLower();
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/" + game + "/";
            }
            //if (AppConfig.DebugMode)
            //{
            // return Application.dataPath + "/" + AppConfig.AssetDirName + "/";
            //}
           // return "C:/" + game + "/";
            return Application.persistentDataPath + "/" + game + "/";
        }
    }

    //--------------------------lua

    /// <summary>
    /// 取得Lua路径
    /// </summary>
    public static string LuaPath(string name)
    {
        string path = DataPath;
        string lowerName = name.ToLower();
        if (lowerName.EndsWith(".lua"))
        {
            int index = name.LastIndexOf('.');
            name = name.Substring(0, index);
        }
        name = name.Replace('.', '/');
        path = path + "lua/" + name + ".lua";
        //LogHelper.Debug("----"+path);
        return path;
    }


//    /// <summary>
//    /// 执行Lua方法
//    /// </summary>
//    public static object[] CallMethod(string module, string func, params object[] args)
//    {
//        LuaScriptMgr luaMgr = ApplicationFacade.GetInstance().LuaManager;
//        if (luaMgr == null)
//            return null;
//        string funcName = module + "." + func;
//        funcName = funcName.Replace("(Clone)", "");
//        return luaMgr.CallLuaFunction(funcName, args);
//    }
//
//    /// <summary>
//    /// 清理内存
//    /// </summary>
//    public static void ClearMemory()
//    {
//        GC.Collect();
//        Resources.UnloadUnusedAssets();
//        LuaScriptMgr mgr = LuaScriptMgr.Instance;
//        if (mgr != null && mgr.lua != null) 
//            mgr.LuaGC();
//    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="path">文件创建目录</param>
    /// <param name="filename">文件的名称</param>
    /// <param name="info">写入的内容</param>
    public static void CreateFile(string path, string filename, string info)
    {
        DeleteFile(path, filename);
        FileStream fs = new FileStream(path + filename, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(info.Trim());
        sw.Flush();
        sw.Close();
        fs.Close();
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="path">删除文件的路径</param>
    /// <param name="name">删除文件的名称</param>
    public static void DeleteFile(string path, string name)
    {
        if(File.Exists(path + name))
            File.Delete(path + name);
    }

}
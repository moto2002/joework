﻿using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;


#if UNITY_EDITOR
using UnityEditor;
#endif

    public class Util
    {

        public static int Int(object o)
        {
            return Convert.ToInt32(o);
        }

        public static float Float(object o)
        {
            return (float)Math.Round(Convert.ToSingle(o), 2);
        }

        public static long Long(object o)
        {
            return Convert.ToInt64(o);
        }

        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static string Uid(string uid)
        {
            int position = uid.LastIndexOf('_');
            return uid.Remove(0, position + 1);
        }

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
                    if (ts[i] != null) GameObject.Destroy(ts[i]);
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
        /// 清除所有子节点
        /// </summary>
        public static void ClearChild(Transform go)
        {
            if (go == null) return;
            for (int i = go.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(go.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 清理内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect(); Resources.UnloadUnusedAssets();
            //LuaManager mgr = AppFacade.Instance.GetManager<LuaManager>(ManagerName.Lua);
           // if (mgr != null) mgr.LuaGC();
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
                if (AppConst.DebugMode)
                {
                    return Application.dataPath + "/" + AppConst.AssetDir + "/";
                }
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    int i = Application.dataPath.LastIndexOf('/');
                    return Application.dataPath.Substring(0, i + 1) + game + "/";
                }
                return "c:/" + game + "/";
            }
        }

        public static string GetRelativePath()
        {
            if (Application.isEditor)
                return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/") + "/Assets/" + AppConst.AssetDir + "/";
            else if (Application.isMobilePlatform || Application.isConsolePlatform)
                return "file:///" + DataPath;
            else // For standalone player.
                return "file://" + Application.streamingAssetsPath + "/";
        }

        /// <summary>
        /// 取得行文本
        /// </summary>
        public static string GetFileText(string path)
        {
            return File.ReadAllText(path);
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
                    path = Application.dataPath + "/" + AppConst.AssetDir + "/";
                    break;
            }
            return path;
        }

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
            if (File.Exists(path + name))
                File.Delete(path + name);
        }

    }


using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace BoTing.Framework
{
    public class DebugKit
    {
        //private static bool writeDown;
        private static List<string> logers;
        private static object locker = new object();
        private static string path;
        private static Action logged;
        private static List<string> contentList = new List<string>();

        private static List<string> ContentList
        {
            get
            {
                List<string> tempList = null;
                lock(locker)
                {
                    tempList = new List<string>(tempList);
                }
                return tempList;
            }
        }

        /// <summary>
        /// Application.dataPath and Application.persistentDataPath can only be accessed in Unity Thread.
        /// Do remember to call DebugKit in Unity thread first, not in the work thread you create.
        /// </summary>
        static DebugKit()
        {

#if UNITY_STANDALONE_WIN
            path = Application.dataPath + "/Log/"; 
#else
            path = Application.persistentDataPath + "/Log/";
#endif
            Application.logMessageReceived += OnApplicationLogMessageReceived;

            Initial("GameHall");
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="loger">eg. HallGame|StudGame </param>
        public static void Initial(string loger = "", bool write = true)
        {
            lock (locker)
            {
                //writeDown = write;
                logers = new List<string>() { "Log", "Error", "Exception", "TODO" };

                string[] ls = loger.Split('|');
                for (int i = 0; i < ls.Length; i++)
                {
                    if (ls[i] != "")
                    {
                        logers.Add(ls[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 添加日志输出
        /// </summary>
        public static void AddTagger(string tagger)
        {
            lock (locker)
            {
                if (!logers.Contains(tagger))
                {
                    logers.Add(tagger);
                }
            }
        }

        /// <summary>
        /// 移除日志输出
        /// </summary>
        public static void RemoveTagger(string tagger)
        {
            lock (locker)
            {
                if (logers.Contains(tagger))
                {
                    logers.Remove(tagger);
                }
            }
        }

        public static void RegisterLogEvent(Action callback)
        {
            lock(locker)
            {
                logged += callback;
            }
        }

        public static void UnregisterLogEvent(Action callback)
        {
            lock (locker)
            {
                logged -= callback;
            }
        }

        /// <summary>
        /// 移除所有日志输出
        /// </summary>
        public static void RemoveAll()
        {
            lock (locker)
            {
                logers.Clear();
            }
        }

        public static List<string> TakeContentList()
        {
            List<string> tempList = null;
            lock (locker)
            {
                tempList = contentList;
                contentList = new List<string>();
            }
            return tempList;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public static void Reset()
        {
            Initial();
        }
        
        static public void Log(object message)
        {
            Log("Log", message.ToString());
        }

        public static void Log(string tag, string message)
        {
            lock (locker)
            {
                //#if UNITY_EDITOR
                if (logers.Contains(tag))
                {
                    string time = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");
                    string trace = "";
                    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
                    System.Diagnostics.StackFrame[] sfs = st.GetFrames();
                    for (int u = 2; u < sfs.Length; ++u)
                    {
                        System.Reflection.MethodBase mb = sfs[u].GetMethod();
                        trace += "\t" + mb.DeclaringType.FullName + ":" + mb.Name + "() (at " + mb.DeclaringType.FullName.Replace(".", "/") + ".cs: " + sfs[u].GetFileLineNumber() + ")\r\n";
                    }
                    string logContent = time + "#(" + tag + ") => " + message + "\r\n" +trace;
                    Debug.Log(logContent);
                    contentList.Add(logContent);
                    if (logged != null) logged();
                    if (/*writeDown ||*/tag == LogType.Error.ToString() || tag == LogType.Exception.ToString())
                    {
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        try
                        {
                            using (StreamWriter w = new StreamWriter(path + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".txt", true, Encoding.UTF8))
                            {
                                w.WriteLine(logContent);
                                w.Flush();
                                w.Close();
                            }
                        }
                        catch(Exception ex)
                        {
                            Debug.Log("Write log file exception, message = " + ex.Message);
                        }                        
                    }
                }
                //#endif
            }
        }

        public static void Log(string tag, string format, params object[] args)
        {
            Log(tag, string.Format(format, args));
        }
        
        public static void LogError(string format, params object[] args)
        {
            Log(LogType.Error.ToString(), string.Format(format, args));
        }

        public static void LogException(string format, params object[] args)
        {
            Log(LogType.Exception.ToString(), string.Format(format, args));
        }
        
        public static void LogWarning(string format, params object[] args)
        {
            Log(LogType.Warning.ToString(), string.Format(format, args));
        }

        public static void Todo(string format, params object[] args)
        {
            Log("TODO", string.Format(format, args));
        }

        private static void OnApplicationLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            lock (locker)
            {
                if (type == LogType.Exception)
                {
                    Log(LogType.Exception.ToString(), condition + "\r\n" + stackTrace);
                }
                else if (type == LogType.Error)
                {
                    Log(LogType.Error.ToString(), condition + "\r\n" + stackTrace);
                }
            }
        }
    }
}
namespace Mogo.Util
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    public class LoggerHelper
    {
        public static Mogo.Util.LogLevel CurrentLogLevels = (Mogo.Util.LogLevel.CRITICAL | Mogo.Util.LogLevel.EXCEPT | Mogo.Util.LogLevel.ERROR | Mogo.Util.LogLevel.WARNING | Mogo.Util.LogLevel.INFO | Mogo.Util.LogLevel.DEBUG);
        public static string DebugFilterStr = string.Empty;
        private static ulong index = 0L;
        private static Mogo.Util.LogWriter m_logWriter = new Mogo.Util.LogWriter();
        private const bool SHOW_STACK = true;

        static LoggerHelper()
        {
            Application.RegisterLogCallback(new Application.LogCallback(LoggerHelper.ProcessExceptionReport));
        }

        public static void Critical(object message, bool isShowStack = true)
        {
            if (Mogo.Util.LogLevel.CRITICAL == (CurrentLogLevels & Mogo.Util.LogLevel.CRITICAL))
            {
                Log(string.Concat(new object[] { " [CRITICAL]: ", message, '\n', isShowStack ? GetStacksInfo() : "" }), Mogo.Util.LogLevel.CRITICAL, true);
            }
        }

        public static void Debug(object message, bool isShowStack = true, int user = 0)
        {
            if (!(DebugFilterStr != "") && (Mogo.Util.LogLevel.DEBUG == (CurrentLogLevels & Mogo.Util.LogLevel.DEBUG)))
            {
                object[] objArray = new object[5];
                objArray[0] = " [DEBUG]: ";
                objArray[1] = isShowStack ? GetStackInfo() : "";
                objArray[2] = message;
                objArray[3] = " Index = ";
                index += (ulong) 1L;
                objArray[4] = index;
                Log(string.Concat(objArray), Mogo.Util.LogLevel.DEBUG, true);
            }
        }

        public static void Debug(string filter, object message, bool isShowStack = true)
        {
            if ((!(DebugFilterStr != "") || !(DebugFilterStr != filter)) && (Mogo.Util.LogLevel.DEBUG == (CurrentLogLevels & Mogo.Util.LogLevel.DEBUG)))
            {
                Log(" [DEBUG]: " + (isShowStack ? GetStackInfo() : "") + message, Mogo.Util.LogLevel.DEBUG, true);
            }
        }

        public static void Error(object message, bool isShowStack = true)
        {
            if (Mogo.Util.LogLevel.ERROR == (CurrentLogLevels & Mogo.Util.LogLevel.ERROR))
            {
                Log(string.Concat(new object[] { " [ERROR]: ", message, '\n', isShowStack ? GetStacksInfo() : "" }), Mogo.Util.LogLevel.ERROR, true);
            }
        }

        public static void Except(Exception ex, object message = null)
        {
            if (Mogo.Util.LogLevel.EXCEPT == (CurrentLogLevels & Mogo.Util.LogLevel.EXCEPT))
            {
                Exception innerException = ex;
                while (innerException.InnerException != null)
                {
                    innerException = innerException.InnerException;
                }
                Log(" [EXCEPT]: " + ((message == null) ? "" : (message + "\n")) + ex.Message + innerException.StackTrace, Mogo.Util.LogLevel.CRITICAL, true);
            }
        }

        private static string GetStackInfo()
        {
            StackTrace trace = new StackTrace();
            MethodBase method = trace.GetFrame(2).GetMethod();
            return string.Format("{0}.{1}(): ", method.ReflectedType.Name, method.Name);
        }

        private static string GetStacksInfo()
        {
            StringBuilder builder = new StringBuilder();
            StackFrame[] frames = new StackTrace().GetFrames();
            for (int i = 2; i < frames.Length; i++)
            {
                builder.AppendLine(frames[i].ToString());
            }
            return builder.ToString();
        }

        public static void Info(object message, bool isShowStack = true)
        {
            if (Mogo.Util.LogLevel.INFO == (CurrentLogLevels & Mogo.Util.LogLevel.INFO))
            {
                Log(" [INFO]: " + (isShowStack ? GetStackInfo() : "") + message, Mogo.Util.LogLevel.INFO, true);
            }
        }

        private static void Log(string message, Mogo.Util.LogLevel level, bool writeEditorLog = true)
        {
            string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + message;
            m_logWriter.WriteLog(msg, level, writeEditorLog);
        }

        private static void ProcessExceptionReport(string message, string stackTrace, LogType type)
        {
            Mogo.Util.LogLevel dEBUG = Mogo.Util.LogLevel.DEBUG;
            switch (type)
            {
                case LogType.Error:
                    dEBUG = Mogo.Util.LogLevel.ERROR;
                    break;

                case LogType.Assert:
                    dEBUG = Mogo.Util.LogLevel.DEBUG;
                    break;

                case LogType.Warning:
                    dEBUG = Mogo.Util.LogLevel.WARNING;
                    break;

                case LogType.Log:
                    dEBUG = Mogo.Util.LogLevel.DEBUG;
                    break;

                case LogType.Exception:
                    dEBUG = Mogo.Util.LogLevel.EXCEPT;
                    break;
            }
            if (dEBUG == (CurrentLogLevels & dEBUG))
            {
                Log(string.Concat(new object[] { " [SYS_", dEBUG, "]: ", message, '\n', stackTrace }), dEBUG, false);
            }
        }

        public static void Release()
        {
            m_logWriter.Release();
        }

        public static void UploadLogFile()
        {
            m_logWriter.UploadTodayLog();
        }

        public static void Warning(object message, bool isShowStack = true)
        {
            if (Mogo.Util.LogLevel.WARNING == (CurrentLogLevels & Mogo.Util.LogLevel.WARNING))
            {
                Log(" [WARNING]: " + (isShowStack ? GetStackInfo() : "") + message, Mogo.Util.LogLevel.WARNING, true);
            }
        }

        public static Mogo.Util.LogWriter LogWriter
        {
            get
            {
                return m_logWriter;
            }
        }
    }
}


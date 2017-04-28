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
        public static LogLevel CurrentLogLevels = (LogLevel.CRITICAL | LogLevel.EXCEPT | LogLevel.ERROR | LogLevel.WARNING | LogLevel.INFO | LogLevel.DEBUG);
        public static string DebugFilterStr = string.Empty;
        private static ulong index = 0L;
        private static LogWriter m_logWriter = new LogWriter();
        private const bool SHOW_STACK = true;

        static LoggerHelper()
        {
            Application.RegisterLogCallback(new Application.LogCallback(null, (IntPtr) ProcessExceptionReport));
        }

        public static void Critical(object message, bool isShowStack = true)
        {
            if (LogLevel.CRITICAL == (CurrentLogLevels & LogLevel.CRITICAL))
            {
                Log(string.Concat(new object[] { " [CRITICAL]: ", message, '\n', isShowStack ? GetStacksInfo() : "" }), LogLevel.CRITICAL, true);
            }
        }

        public static void Debug(object message, bool isShowStack = true, int user = 0)
        {
            if (!(DebugFilterStr != "") && (LogLevel.DEBUG == (CurrentLogLevels & LogLevel.DEBUG)))
            {
                object[] objArray = new object[5];
                objArray[0] = " [DEBUG]: ";
                objArray[1] = isShowStack ? GetStackInfo() : "";
                objArray[2] = message;
                objArray[3] = " Index = ";
                index += (ulong) 1L;
                objArray[4] = index;
                Log(string.Concat(objArray), LogLevel.DEBUG, true);
            }
        }

        public static void Debug(string filter, object message, bool isShowStack = true)
        {
            if ((!(DebugFilterStr != "") || !(DebugFilterStr != filter)) && (LogLevel.DEBUG == (CurrentLogLevels & LogLevel.DEBUG)))
            {
                Log(" [DEBUG]: " + (isShowStack ? GetStackInfo() : "") + message, LogLevel.DEBUG, true);
            }
        }

        public static void Error(object message, bool isShowStack = true)
        {
            if (LogLevel.ERROR == (CurrentLogLevels & LogLevel.ERROR))
            {
                Log(string.Concat(new object[] { " [ERROR]: ", message, '\n', isShowStack ? GetStacksInfo() : "" }), LogLevel.ERROR, true);
            }
        }

        public static void Except(Exception ex, object message = null)
        {
            if (LogLevel.EXCEPT == (CurrentLogLevels & LogLevel.EXCEPT))
            {
                Exception innerException = ex;
                while (innerException.InnerException != null)
                {
                    innerException = innerException.InnerException;
                }
                Log(" [EXCEPT]: " + ((message == null) ? "" : (message + "\n")) + ex.Message + innerException.StackTrace, LogLevel.CRITICAL, true);
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
            if (LogLevel.INFO == (CurrentLogLevels & LogLevel.INFO))
            {
                Log(" [INFO]: " + (isShowStack ? GetStackInfo() : "") + message, LogLevel.INFO, true);
            }
        }

        private static void Log(string message, LogLevel level, bool writeEditorLog = true)
        {
            string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + message;
            m_logWriter.WriteLog(msg, level, writeEditorLog);
        }

        private static void ProcessExceptionReport(string message, string stackTrace, LogType type)
        {
            LogLevel dEBUG = LogLevel.DEBUG;
            switch (type)
            {
                case 0:
                    dEBUG = LogLevel.ERROR;
                    break;

                case 1:
                    dEBUG = LogLevel.DEBUG;
                    break;

                case 2:
                    dEBUG = LogLevel.WARNING;
                    break;

                case 3:
                    dEBUG = LogLevel.DEBUG;
                    break;

                case 4:
                    dEBUG = LogLevel.EXCEPT;
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
            if (LogLevel.WARNING == (CurrentLogLevels & LogLevel.WARNING))
            {
                Log(" [WARNING]: " + (isShowStack ? GetStackInfo() : "") + message, LogLevel.WARNING, true);
            }
        }
    }
}


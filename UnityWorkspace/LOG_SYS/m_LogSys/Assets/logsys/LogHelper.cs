using UnityEngine;
using System;

namespace ShiHuanJue.Debuger
{
    public class LogHelper 
    {
        static public LogLevel m_logLevel = LogLevel.All;
        static LogWriter m_logWriter = new LogWriter();

        static LogHelper()
        {
            Application.logMessageReceived += ProcessExceptionReport;
        }

        private static void ProcessExceptionReport(string message, string stackTrace, LogType type)
        {
            LogLevel dEBUG = LogLevel.Debug;
            switch (type)
            {
                case LogType.Error:
                    dEBUG = LogLevel.Error;
                    break;
                case LogType.Assert:
                    dEBUG = LogLevel.Debug;
                    break;
                case LogType.Warning:
                    dEBUG = LogLevel.Warning;
                    break;
                case LogType.Log:
                    dEBUG = LogLevel.Debug;
                    break;
                case LogType.Exception:
                    dEBUG = LogLevel.Exception;
                    break;
            }

            if (dEBUG == (m_logLevel & dEBUG))
            {
                Log(string.Concat(new object[] { " [", dEBUG, "]: ", message, '\n', stackTrace }));
            }
        }


        /// <summary>
        /// 加上时间戳
        /// </summary>
        /// <param name="message"></param>
        private static void Log(string message)
        {
            string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + message;
            m_logWriter.ExcuteWrite(msg);
        }

        static public void Log(object message)
        {
            Log(message, null);
        }
        static public void Log(object message, UnityEngine.Object context)
        {
            if (LogLevel.Debug == (m_logLevel & LogLevel.Debug))
            {
                Debug.Log(message, context);
            }
        }
        static public void LogError(object message)
        {
            LogError(message, null);
        }
        static public void LogError(object message, UnityEngine.Object context)
        {
            if (LogLevel.Error == (m_logLevel & LogLevel.Error))
            {
                Debug.LogError(message, context);
            }
        }
        static public void LogWarning(object message)
        {
            LogWarning(message, null);
        }
        static public void LogWarning(object message, UnityEngine.Object context)
        {
            if (LogLevel.Warning == (m_logLevel & LogLevel.Warning))
            {
                Debug.LogWarning(message, context);
            }
        }
    }
}


namespace Mogo.Util
{
    using System;
    using System.IO;
    using System.Threading;
    using UnityEngine;

    public class LogWriter
    {
        private FileStream m_fs;
        private static readonly object m_locker = new object();
        private string m_logFileName = "log_{0}.txt";
        private string m_logFilePath;
        private string m_logPath = (Application.persistentDataPath + "/log/");
        private Action<string, Mogo.Util.LogLevel, bool> m_logWriter;
        private StreamWriter m_sw;

        public LogWriter()
        {
            if (!Directory.Exists(this.m_logPath))
            {
                Directory.CreateDirectory(this.m_logPath);
            }
            this.m_logFilePath = this.m_logPath + string.Format(this.m_logFileName, DateTime.Today.ToString("yyyyMMdd"));
            try
            {
                this.m_logWriter = new Action<string, Mogo.Util.LogLevel, bool>(this.Write);
                this.m_fs = new FileStream(this.m_logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                this.m_sw = new StreamWriter(this.m_fs);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        }

        public void Release()
        {
            lock (m_locker)
            {
                if (this.m_sw != null)
                {
                    this.m_sw.Close();
                    this.m_sw.Dispose();
                }
                if (this.m_fs != null)
                {
                    this.m_fs.Close();
                    this.m_fs.Dispose();
                }
            }
        }

        public void UploadTodayLog()
        {
        }

        private void Write(string msg, Mogo.Util.LogLevel level, bool writeEditorLog)
        {
            object obj2;
            Monitor.Enter(obj2 = m_locker);
            try
            {
                if (!writeEditorLog)
                {
                    goto Label_0066;
                }
                Mogo.Util.LogLevel level2 = level;
                if (level2 <= Mogo.Util.LogLevel.ERROR)
                {
                    switch (level2)
                    {
                        case Mogo.Util.LogLevel.DEBUG:
                        case Mogo.Util.LogLevel.INFO:
                            Debug.Log(msg);
                            break;

                        case Mogo.Util.LogLevel.WARNING:
                            Debug.LogWarning(msg);
                            break;

                        case Mogo.Util.LogLevel.ERROR:
                            goto Label_005A;
                    }
                    goto Label_0066;
                }
                if ((level2 != Mogo.Util.LogLevel.EXCEPT) && (level2 != Mogo.Util.LogLevel.CRITICAL))
                {
                    goto Label_0066;
                }
            Label_005A:
                Debug.LogError(msg);
            Label_0066:
                if (this.m_sw != null)
                {
                    this.m_sw.WriteLine(msg);
                    this.m_sw.Flush();
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
            finally
            {
                Monitor.Exit(obj2);
            }
        }

        public void WriteLog(string msg, Mogo.Util.LogLevel level, bool writeEditorLog)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                this.m_logWriter(msg, level, writeEditorLog);
            }
            else
            {
                this.m_logWriter.BeginInvoke(msg, level, writeEditorLog, null, null);
            }
        }

        public string LogPath
        {
            get
            {
                return this.m_logPath;
            }
        }
    }
}


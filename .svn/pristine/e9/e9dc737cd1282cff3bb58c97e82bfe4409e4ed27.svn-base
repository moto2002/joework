using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;

namespace ShiHuanJue.Debuger
{
    public class LogWriter
    {
        private string m_logPath = Application.persistentDataPath + "/log/";
        private string m_logFileName = "log_{0}.txt";
        private string m_logFilePath = string.Empty;

        public LogWriter()
        {
            if (!Directory.Exists(m_logPath))
            {
                Directory.CreateDirectory(m_logPath);
            }
            this.m_logFilePath = this.m_logPath + string.Format(this.m_logFileName, DateTime.Today.ToString("yyyyMMdd"));
        }

        public void ExcuteWrite(string content)
        {
            using (StreamWriter writer = new StreamWriter(m_logFilePath, true, Encoding.UTF8))
            {
                writer.WriteLine(content);
            }
        }
    }
}


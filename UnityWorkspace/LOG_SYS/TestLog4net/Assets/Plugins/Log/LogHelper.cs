using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using log4net.Config;
using System.IO;
using System;
using System.Reflection; 
using System.Diagnostics;
using System.Text;

//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Util.Logger
{
    #region [ enum: LogLevel ]
    /// <summary>  
    /// 日志级别  
    /// </summary>  
    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
    #endregion  

    public sealed class LogHelper
    {
        #region [ 单例模式 ]
        private static readonly LogHelper _logger = new LogHelper();
        private static readonly log4net.ILog _Logger4net = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public  string configPath =
#if UNITY_ANDROID
    "Log/app.xml";
#elif UNITY_STANDALONE_WIN
	Application.dataPath + "/" + "log.config";
#endif

        /// <summary>  
        /// 无参私有构造函数  
        /// </summary>  
        private LogHelper()
        {
            Init();

			//读取log的配置
#if UNITY_ANDROID
        byte[] xml = (Resources.Load(_fileName, typeof(TextAsset)) as TextAsset).bytes;
        XmlConfigurator.Configure(new MemoryStream(xml));
#elif UNITY_STANDALONE_WIN

			XmlConfigurator.ConfigureAndWatch(new FileInfo(configPath));

#endif

    
        }

        /// <summary>  
        /// 得到单例  
        /// </summary>  
        public static LogHelper Singleton
        {
            get
            {
                return _logger;
            }
        }
        #endregion

		#region [ Init ] 监听log的回调，然后做自定义的处理,然后用log4net写到文本里面
		private void Init()
		{
			UnityEngine.Application.logMessageReceived += HandleLog;
		}
		
		void HandleLog(string logString, string stackTrace, LogType type)
		{
			LogLevel level = LogLevel.Debug;
			switch (type)
			{
			case LogType.Error:
				level = LogLevel.Error;
				break;
			case LogType.Assert:
				level = LogLevel.Debug;
				break;
			case LogType.Warning:
				level = LogLevel.Warn;
				break;
			case LogType.Log: 
				level = LogLevel.Debug;
				break;
			case LogType.Exception: 
				level = LogLevel.Error;
				break;
			}
			//Log_AddTime(string.Concat(new object[] { " [SYS_", level, "]: ", logString, '\n', stackTrace }), level);
			this.Log (level, logString + "\n" + stackTrace);
		}


		#endregion

		#region [ 析构 ]
		~LogHelper()
		{
			UnityEngine.Application.logMessageReceived -= HandleLog;
		}
		#endregion

		
        #region [ 参数 ]
        public bool IsDebugEnabled
        {
            get { return _Logger4net.IsDebugEnabled; }
        }
        public bool IsInfoEnabled
        {
            get { return _Logger4net.IsInfoEnabled; }
        }
        public bool IsWarnEnabled
        {
            get { return _Logger4net.IsWarnEnabled; }
        }
        public bool IsErrorEnabled
        {
            get { return _Logger4net.IsErrorEnabled; }
        }
        public bool IsFatalEnabled
        {
            get { return _Logger4net.IsFatalEnabled; }
        }
        #endregion

        #region [ 接口方法 ]

		#region [ 通用 ]
		string Log_AddTime(string message)
		{
			string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + message;
			return msg;
		}
		#endregion

        #region [ Debug ]
		public void Debug(object message)
		{
		
		}

        public void Debug(string message)
        {
            if (this.IsDebugEnabled)
            {
				string msg = " [DEBUG]: " + GetStackInfo() + message;
				msg = Log_AddTime(msg);
				UnityEngine.Debug.Log( msg );
            }
        }

        public void Debug(string message, Exception exception)
        {
            if (this.IsDebugEnabled)
            {
                //this.Log(LogLevel.Debug, message, exception);
            }
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (this.IsDebugEnabled)
            {
                //this.Log(LogLevel.Debug, format, args);
            }
        }

        public void DebugFormat(string format, Exception exception, params object[] args)
        {
            if (this.IsDebugEnabled)
            {
               // this.Log(LogLevel.Debug, string.Format(format, args), exception);
            }
        }
        #endregion

        #region [ Info ]
        public void Info(string message)
        {
            if (this.IsInfoEnabled)
            {
                //this.Log(LogLevel.Info, message);

				string msg = " [INFO]: " + GetStackInfo() + message;
				msg = Log_AddTime(msg);
				UnityEngine.Debug.Log(msg);
            }
        }

        public void Info(string message, Exception exception)
        {
            if (this.IsInfoEnabled)
            {
                //this.Log(LogLevel.Info, message, exception);
            }
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (this.IsInfoEnabled)
            {
                //this.Log(LogLevel.Info, format, args);
            }
        }

        public void InfoFormat(string format, Exception exception, params object[] args)
        {
            if (this.IsInfoEnabled)
            {
                //this.Log(LogLevel.Info, string.Format(format, args), exception);
            }
        }
        #endregion

        #region  [ Warn ]

        public void Warn(string message)
        {
            if (this.IsWarnEnabled)
            {
                //this.Log(LogLevel.Warn, message);

				string msg = " [WARN]: " + GetStackInfo() + message;
				msg = Log_AddTime(msg);
				UnityEngine.Debug.LogWarning( msg);
            }
        }

        public void Warn(string message, Exception exception)
        {
            if (this.IsWarnEnabled)
            {
                //this.Log(LogLevel.Warn, message, exception);
            }
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (this.IsWarnEnabled)
            {
                //this.Log(LogLevel.Warn, format, args);
            }
        }

        public void WarnFormat(string format, Exception exception, params object[] args)
        {
            if (this.IsWarnEnabled)
            {
                //this.Log(LogLevel.Warn, string.Format(format, args), exception);
            }
        }
        #endregion

        #region  [ Error ]

        public void Error(string message)
        {
            if (this.IsErrorEnabled)
            {
                //this.Log(LogLevel.Error, message);

				string msg = " [ERROR]: " + GetStackInfo() + message;
				msg = Log_AddTime(msg);
				UnityEngine.Debug.LogError(msg);
			}
        }

        public void Error(string message, Exception exception)
        {
            if (this.IsErrorEnabled)
            {
                //this.Log(LogLevel.Error, message, exception);
            }
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (this.IsErrorEnabled)
            {
                //this.Log(LogLevel.Error, format, args);
            }
        }

        public void ErrorFormat(string format, Exception exception, params object[] args)
        {
            if (this.IsErrorEnabled)
            {
                //this.Log(LogLevel.Error, string.Format(format, args), exception);
            }
        }
        #endregion

        #region  [ Fatal ]

        public void Fatal(string message)
        {
            if (this.IsFatalEnabled)
            {
                //this.Log(LogLevel.Fatal, message);

				string msg = " [FATAL]: " + GetStackInfo() + message;
				msg = Log_AddTime(msg);
				UnityEngine.Debug.LogError(msg);
            }
        }

        public void Fatal(string message, Exception exception)
        {
            if (this.IsFatalEnabled)
            {
                //this.Log(LogLevel.Fatal, message, exception);
            }
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                //this.Log(LogLevel.Fatal, format, args);
            }
        }

        public void FatalFormat(string format, Exception exception, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                //this.Log(LogLevel.Fatal, string.Format(format, args), exception);
            }
        }
        #endregion
        #endregion

        #region [ 内部方法 ]
        /// <summary>  
        /// 输出普通日志  
        /// </summary>  
        /// <param name="level"></param>  
        /// <param name="message"></param>  
        /// <param name="args"></param>  
        private void Log(LogLevel level, string message, params object[] args)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _Logger4net.DebugFormat(message, args);
                    break;
                case LogLevel.Info:
                    _Logger4net.InfoFormat(message, args);
                    break;
                case LogLevel.Warn:
                    _Logger4net.WarnFormat(message, args);
                    break;
                case LogLevel.Error:
                    _Logger4net.ErrorFormat(message, args);
                    break;
                case LogLevel.Fatal:
                    _Logger4net.FatalFormat(message, args);
                    break;
            }
        }

        /// <summary>  
        /// 格式化输出异常信息  
        /// </summary>  
        /// <param name="level"></param>  
        /// <param name="message"></param>  
        /// <param name="exception"></param>  
		private void Log(LogLevel level, string message, Exception exception)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _Logger4net.Debug(message, exception);
                    break;
                case LogLevel.Info:
                    _Logger4net.Info(message, exception);
                    break;
                case LogLevel.Warn:
                    _Logger4net.Warn(message, exception);
                    break;
                case LogLevel.Error:
                    _Logger4net.Error(message, exception);
                    break;
                case LogLevel.Fatal:
                    _Logger4net.Fatal(message, exception);
                    break;
            }
        }
        #endregion

		private string GetStackInfo()
		{
			StackTrace trace = new StackTrace();
			MethodBase method = trace.GetFrame(2).GetMethod();
			return string.Format("{0}.{1}(): ", method.ReflectedType.Name, method.Name);
		}
		
		private string GetStacksInfo()
		{
			StringBuilder builder = new StringBuilder();
			StackFrame[] frames = new StackTrace().GetFrames();
			for (int i = 2; i < frames.Length; i++)
			{
				builder.AppendLine(frames[i].ToString());
			}
			return builder.ToString();
		}
		
		
	}
	
}



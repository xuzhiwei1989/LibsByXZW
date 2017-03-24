#region License
/* XZW.Logs基于Apache log4net 2.0.8进行封装，
 * 由于使用Apache log4net时需要在很多地方配置和声明，
 * 所以才有了对log4net进行二次封装的想法。
 * 使用本库无需在代码中声明配置文件位置，也无需再声明ILog对象，
 * 只需直接引用本库并将XZW.Logs拷贝到相应目录即可。
 * 再次向Apache致敬
 */
#endregion
using System;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using log4net;
using log4net.Config;

namespace XZW.Logs
{
    public static class LogUtils
    {
        public delegate void LogWriteEvent(LogLevel level, string msg);

        public static string ConfigFile { get; set; }

        private static ILog log;

        private static LogWriteEvent logWriteEvent;
        public static event LogWriteEvent WriteEvent
        {
            add
            {
                LogWriteEvent logWriteEvent = LogUtils.logWriteEvent;
                LogWriteEvent logWriteEvent2;
                do
                {
                    logWriteEvent2 = logWriteEvent;
                    LogWriteEvent value2 = (LogWriteEvent)Delegate.Combine(logWriteEvent2, value);
                    logWriteEvent = Interlocked.CompareExchange<LogWriteEvent>(ref LogUtils.logWriteEvent, value2, logWriteEvent2);
                }
                while (logWriteEvent != logWriteEvent2);
            }
            remove
            {
                LogWriteEvent logWriteEvent = LogUtils.logWriteEvent;
                LogWriteEvent logWriteEvent2;
                do
                {
                    logWriteEvent2 = logWriteEvent;
                    LogWriteEvent value2 = (LogWriteEvent)Delegate.Remove(logWriteEvent2, value);
                    logWriteEvent = Interlocked.CompareExchange<LogWriteEvent>(ref LogUtils.logWriteEvent, value2, logWriteEvent2);
                }
                while (logWriteEvent != logWriteEvent2);
            }
        }

        public static ILog Default
        {
            get
            {
                if (LogUtils.log == null)
                {
                    if (File.Exists(LogUtils.ConfigFile))
                    {
                        XmlConfigurator.ConfigureAndWatch(new FileInfo(LogUtils.ConfigFile));
                    }
                    LogUtils.log = LogManager.GetLogger(typeof(LogUtils));
                }
                return LogUtils.log;
            }
        }

        static LogUtils()
        {
            LogUtils.log = null;
            string text = ConfigurationManager.AppSettings["XZW.Logs"];
            if (string.IsNullOrEmpty(text))
            {
                text = "XZW.Logs.config";
            }
            if (!File.Exists(text) && !text.Contains("\\"))
            {
                text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, text);
            }
            LogUtils.ConfigFile = text;
        }

        private static void WriteLog(LogLevel level, string msg, bool isTriggerWriteEvent)
        {
            if (isTriggerWriteEvent && LogUtils.logWriteEvent != null)
            {
                try
                {
                    LogUtils.logWriteEvent(level, msg);
                }
                catch
                {
                }
            }
        }

        public static void Debug(string msg, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Debug(msg);
            LogUtils.WriteLog(LogLevel.Debug, msg, isTriggerWriteEvent);
        }

        public static void Debug(string msg, Exception ex, bool isTriggerWriteEvent = true)
        {
            LogUtils.Debug(msg + ex.Message + ex.StackTrace, isTriggerWriteEvent);
        }

        public static void Info(string msg, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Info(msg);
            LogUtils.WriteLog(LogLevel.Info, msg, isTriggerWriteEvent);
        }

        public static void Info(string msg, Exception ex, bool isTriggerWriteEvent = true)
        {
            LogUtils.Info(msg + ex.Message + ex.StackTrace, isTriggerWriteEvent);
        }

        public static void Warn(string msg, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Warn(msg);
            LogUtils.WriteLog(LogLevel.Warn, msg, isTriggerWriteEvent);
        }

        public static void Warn(string msg, Exception ex, bool isTriggerWriteEvent = true)
        {
            LogUtils.Warn(msg + ex.Message + ex.StackTrace, isTriggerWriteEvent);
        }

        public static void Error(string msg, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Error(msg);
            LogUtils.WriteLog(LogLevel.Error, msg, isTriggerWriteEvent);
        }

        public static void Error(string msg, Exception ex, bool isTriggerWriteEvent = true)
        {
            LogUtils.Error(msg + ex.Message + ex.StackTrace, isTriggerWriteEvent);
        }

        public static void Fatal(string msg, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Fatal(msg);
            LogUtils.WriteLog(LogLevel.Fatal, msg, isTriggerWriteEvent);
        }

        public static void Fatal(string msg, Exception ex, bool isTriggerWriteEvent = true)
        {
            LogUtils.Fatal(msg + ex.Message + ex.StackTrace, isTriggerWriteEvent);
        }
    }
}

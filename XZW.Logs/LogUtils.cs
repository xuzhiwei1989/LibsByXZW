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
using log4net.Core;
using log4net.Util;

namespace XZW.Logs
{
    public delegate void LogWriteEvent(LogData data);

    public static class LogUtils
    {

        public static string ConfigFile { get; set; }

        private static ILog log;

        private static LogWriteEvent writeEvent;
        public static event LogWriteEvent WriteEvent
        {
            add
            {
                LogWriteEvent writeEvent1 = LogUtils.writeEvent;
                LogWriteEvent writeEvent2;
                do
                {
                    writeEvent2 = writeEvent1;
                    LogWriteEvent value2 = (LogWriteEvent)Delegate.Combine(writeEvent2, value);
                    writeEvent1 = Interlocked.CompareExchange<LogWriteEvent>(ref LogUtils.writeEvent, value2, writeEvent2);
                }
                while (writeEvent1 != writeEvent2);
            }
            remove
            {
                LogWriteEvent logWriteEvent = LogUtils.writeEvent;
                LogWriteEvent logWriteEvent2;
                do
                {
                    logWriteEvent2 = logWriteEvent;
                    LogWriteEvent value2 = (LogWriteEvent)Delegate.Remove(logWriteEvent2, value);
                    logWriteEvent = Interlocked.CompareExchange<LogWriteEvent>(ref LogUtils.writeEvent, value2, logWriteEvent2);
                }
                while (logWriteEvent != logWriteEvent2);
            }
        }

        private static ILog Default
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
                    LogUtils.log.WriteEvent = new LogWriteEvent(LogUtils.OnWriteEvent);
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

        private static void OnWriteEvent(LogData data)
        {
            if (LogUtils.writeEvent != null)
            {
                LogUtils.writeEvent(data);
            }
        }
        
        public static void Debug(string msg, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Debug(msg, isTriggerWriteEvent);
        }

        public static void Debug(string msg, Exception ex, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Debug(msg, ex, isTriggerWriteEvent);
        }

        public static void Info(string msg, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Info(msg, isTriggerWriteEvent);
        }

        public static void Info(string msg, Exception ex, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Info(msg, ex, isTriggerWriteEvent);
        }

        public static void Warn(string msg, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Warn(msg, isTriggerWriteEvent);
        }

        public static void Warn(string msg, Exception ex, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Warn(msg, ex, isTriggerWriteEvent);
        }

        public static void Error(string msg, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Error(msg, isTriggerWriteEvent);
        }

        public static void Error(string msg, Exception ex, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Error(msg, ex, isTriggerWriteEvent);
        }

        public static void Fatal(string msg, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Fatal(msg, isTriggerWriteEvent);
        }

        public static void Fatal(string msg, Exception ex, bool isTriggerWriteEvent = true)
        {
            LogUtils.Default.Fatal(msg, ex, isTriggerWriteEvent);
        }
    }
}

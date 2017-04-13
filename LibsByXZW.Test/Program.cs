using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LibsByXZW.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            XZW.Logs.LogUtils.WriteEvent += LogUtils_WriteEvent;
            testLog();
        }

        static void LogUtils_WriteEvent(XZW.Logs.LogData data)
        {
            Console.WriteLine(JsonConvert.SerializeObject(data));
        }

        private static void testLog()
        {
            XZW.Logs.LogUtils.Debug("Debug");
            XZW.Logs.LogUtils.Info("Info");
            XZW.Logs.LogUtils.Warn("Warn");
            XZW.Logs.LogUtils.Error("Error");
            XZW.Logs.LogUtils.Fatal("Fatal");

            try
            {
                int a = 0;
                int i = 5 / a;
            }
            catch(Exception ex)
            {
                XZW.Logs.LogUtils.Debug("Debug", ex);
                XZW.Logs.LogUtils.Info("Info", ex);
                XZW.Logs.LogUtils.Warn("Warn", ex);
                XZW.Logs.LogUtils.Error("Error", ex);
                XZW.Logs.LogUtils.Fatal("Fatal", ex);
            }
        }
    }
}

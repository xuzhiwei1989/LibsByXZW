using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XZW.Logs
{
    [Serializable]
    public class LogData
    {
        public string LoggerName
        {
            get;
            set;
        }

        public string Level
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string ThreadName
        {
            get;
            set;
        }

        public int ThreadId
        {
            get;
            set;
        }

        public DateTime Time
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Identity
        {
            get;
            set;
        }

        public Exception Exception
        {
            get;
            set;
        }

        public string AppDomain
        {
            get;
            set;
        }

        public string ClassName
        {
            get;
            set;
        }

        public string MethodName
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public string LineNumber
        {
            get;
            set;
        }
    }
}

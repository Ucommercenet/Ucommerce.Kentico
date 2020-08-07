using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.EventLog;
using NHibernate;

namespace UCommerce.Kentico.Logging
{
    public class Logger : IInternalLogger
    {
        public void Error(object message)
        {
            EventLogProvider.LogException("", "ERROR", new Exception(message.ToString()));
        }

        public void Error(object message, Exception exception)
        {
            EventLogProvider.LogException("", "ERROR", exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            EventLogProvider.LogInformation("", "FATAL", string.Format(format, args));
        }

        public void Fatal(object message)
        {
            EventLogProvider.LogInformation("", "FATAL", message.ToString());
        }

        public void Fatal(object message, Exception exception)
        {
            EventLogProvider.LogInformation("", "FATAL", message.ToString());
        }


        
        public void Debug(object message)
        {
            //Removed because Nhibernate doesn't respect the isXXXEnabled settings
            //EventLogProvider.LogInformation("", "DEBUG", message.ToString());
        }

        public void Debug(object message, Exception exception)
        {
            //Removed because Nhibernate doesn't respect the isXXXEnabled settings
            //EventLogProvider.LogInformation("", "DEBUG", message.ToString());
        }

        public void DebugFormat(string format, params object[] args)
        {
            //Removed because Nhibernate doesn't respect the isXXXEnabled settings
            //EventLogProvider.LogInformation("", "DEBUG", string.Format(format, args));
        }

        public void Info(object message)
        {
            //Removed because Nhibernate doesn't respect the isXXXEnabled settings
            //EventLogProvider.LogInformation("", "INFORMATION", message.ToString());
        }

        public void Info(object message, Exception exception)
        {
            //Removed because Nhibernate doesn't respect the isXXXEnabled settings
            //EventLogProvider.LogInformation("", "INFORMATION", message.ToString());
        }

        public void InfoFormat(string format, params object[] args)
        {
            //Removed because Nhibernate doesn't respect the isXXXEnabled settings
            //EventLogProvider.LogInformation("", "INFORMATION", string.Format(format, args));
        }

        public void Warn(object message)
        {
            EventLogProvider.LogInformation("", "WARN", message.ToString());
        }

        public void Warn(object message, Exception exception)
        {
            EventLogProvider.LogInformation("", "WARN", message.ToString());
        }

        public void WarnFormat(string format, params object[] args)
        {
            EventLogProvider.LogInformation("", "WARN", string.Format(format, args));
        }

        public bool IsErrorEnabled
        {
            get { return true; }
        }

        public bool IsFatalEnabled
        {
            get { return true; }
        }
        public bool IsDebugEnabled
        {
            get { return false; }
        }
        public bool IsInfoEnabled
        {
            get { return false; }
        }
        public bool IsWarnEnabled
        {
            get { return true; }
        }
    }
}

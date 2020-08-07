using CMS.EventLog;
using System;
using UCommerce.Installer;

namespace UCommerce.Kentico.Installer
{
    public class KenticoInstallerLoggingService: IInstallerLoggingService
    {
        public void Log<T>(string customMessage)
        {
            EventLogProvider.LogInformation(typeof(T).FullName, "INFORMATION", customMessage);
        }

        public void Log<T>(Exception exception)
        {
            EventLogProvider.LogException(typeof(T).FullName, "EXCEPTION", exception);
        }

        public void Log<T>(Exception exception, string customMessage)
        {
            EventLogProvider.LogException(typeof(T).FullName, "EXCEPTION", exception, additionalMessage: customMessage);
        }
    }
}

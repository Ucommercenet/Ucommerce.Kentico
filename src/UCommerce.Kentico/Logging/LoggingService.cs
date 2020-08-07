using System;
using CMS.EventLog;
using UCommerce.Infrastructure.Logging;

namespace UCommerce.Kentico.Logging
{
    /// <summary>
    /// Kentico implementation of <see cref="ILoggingService"/>.
    /// </summary>
    public class LoggingService : ILoggingService
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
            EventLogProvider.LogException(typeof(T).FullName, "EXCEPTION", exception, additionalMessage:customMessage);
        }
    }
}

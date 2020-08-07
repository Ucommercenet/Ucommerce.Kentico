using Castle.DynamicProxy;
using CMS.Helpers;
using CMS.SiteProvider;
using UCommerce.Infrastructure.Logging;

namespace UCommerce.Kentico.Security
{
    /// <summary>
    /// Interceptor for Castle components that will log a screen action for Kentico, so the ScreenLock timer is reset.
    /// </summary>
    public class LogScreenLockInterceptor : IInterceptor
    {
        private readonly ILoggingService _loggingService;
        public bool Debug { get; set; }
        private bool _hasRunForCurrentRequest;

        public LogScreenLockInterceptor(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            if (!SecurityHelper.IsScreenLockEnabled(SiteContext.CurrentSite.SiteName))
                return;
            if (_hasRunForCurrentRequest)
                return;

            SecurityHelper.LogScreenLockAction();
            _hasRunForCurrentRequest = true;

            if (Debug)
            {
                var invocationTargetTypeName = invocation.InvocationTarget.GetType().Name;

                _loggingService.Log<LogScreenLockInterceptor>("ScreenLock Action has been logged by " +
                                                              invocationTargetTypeName);
            }
        }
    }
}

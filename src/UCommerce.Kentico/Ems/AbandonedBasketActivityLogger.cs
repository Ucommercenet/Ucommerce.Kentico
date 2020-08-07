using System;
using CMS.Activities;
using CMS.Core;
using UCommerce.Infrastructure.Logging;
using UCommerce.Kentico.Ems.Initializers;

namespace UCommerce.Kentico.Ems
{
    public class AbandonedBasketActivityLogger : IAbandonedBasketActivityLogger
    {
        private readonly ILoggingService _loggingService;

        public AbandonedBasketActivityLogger(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void MarkBasketAsAbandoned(int orderId, Guid orderGuid, int contactId, int siteId)
        {
            _loggingService.Log<AbandonedBasketActivityLogger>(string.Format("Activity logged: AbandonedBasket: {0} - {1}", orderId, orderGuid));
            var mActivityLogService = Service.Entry<IActivityLogService>();
            var activityInitializer = new AbandonedBasketActivityInitializer(orderId, orderGuid, contactId, siteId);
            mActivityLogService.LogWithoutModifiersAndFilters(activityInitializer);
        }
    }
}

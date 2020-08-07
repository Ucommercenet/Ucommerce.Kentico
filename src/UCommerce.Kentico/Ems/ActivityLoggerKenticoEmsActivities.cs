using CMS.Activities;
using UCommerce.Infrastructure.Components.Windsor;
using UCommerce.Infrastructure.Logging;
using UCommerce.Kentico.Ems.Initializers;
using UCommerce.Kentico.Infrastructure;

namespace UCommerce.Kentico.Ems
{
    public class ActivityLoggerKenticoEmsActivities : IActivityLogger
    {
        private readonly ILoggingService _loggingService;
        
        [Mandatory]
        public IKenticoServiceProvider KenticoServiceProvider { get; set; }

        [Mandatory]
        public IKenticoHttpContextProvider KenticoHttpContextProvider { get; set; }

        public ActivityLoggerKenticoEmsActivities(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void ProductAddedToBasket(int quantity, string displayName, int productId)
        {
            _loggingService.Log<ActivityLoggerKenticoEmsActivities>(
                $"Activity logged: ProductAddedToBasket: {displayName} - Q:{quantity} - PID:{productId}");
            
            //var activityLogService = Service.Entry<IActivityLogService>();
            var activityLogService = KenticoServiceProvider.Resolve<IActivityLogService>();
            var activityInitializer = new ProductAddedToBasketActivityInitializer(quantity, displayName, productId);
            activityLogService.Log(activityInitializer, KenticoHttpContextProvider.HttpRequestBase());
        }

        public void ProductPurchased(int contactId, int siteId, int quantity, string displayName, int productId)
        {
            _loggingService.Log<ActivityLoggerKenticoEmsActivities>(
                $"Activity logged: ProductPurchased: {displayName} - Q:{quantity} - PID:{productId}");

            //var activityLogService = Service.Entry<IActivityLogService>();
            var activityLogService = KenticoServiceProvider.Resolve<IActivityLogService>();
            var activityInitializer = new ProductPurchasedActivityInitializer(contactId, siteId, quantity, displayName, productId);
            activityLogService.LogWithoutModifiersAndFilters(activityInitializer);
        }

        public void OrderPurchased(int contactId, int siteId, decimal amount, int orderId, string orderNumber)
        {
            _loggingService.Log<ActivityLoggerKenticoEmsActivities>(
                $"Activity logged: OrderPurchased: {orderNumber} - A:{amount}");

            //var activityLogService = Service.Entry<IActivityLogService>();
            var activityLogService = KenticoServiceProvider.Resolve<IActivityLogService>();
            var activityInitializer = new OrderPurchasedActivityInitializer(contactId, siteId, amount, orderId, orderNumber);
            activityLogService.LogWithoutModifiersAndFilters(activityInitializer);
        }

        public void ProductRemovedFromBasket(int quantity, string displayName, int productId)
        {
            _loggingService.Log<ActivityLoggerKenticoEmsActivities>(
                $"Activity logged: ProductRemovedFromBasket: {displayName} - Q:{quantity} - PID:{productId}");

            //var activityLogService = Service.Entry<IActivityLogService>();
            var activityLogService = KenticoServiceProvider.Resolve<IActivityLogService>();

            var activityInitializer = new ProductRemovedFromBasketActivityInitializer(quantity, displayName, productId);

            activityLogService.Log(activityInitializer, KenticoHttpContextProvider.HttpRequestBase());
        }
    }
}

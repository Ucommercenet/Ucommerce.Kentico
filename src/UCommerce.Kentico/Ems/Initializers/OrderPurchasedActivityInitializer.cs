using System.Globalization;
using CMS.Activities;
using UCommerce.Kentico.Ems.Activities;

namespace UCommerce.Kentico.Ems.Initializers
{
    /// <summary>
    /// Initializer for the 'Order purchased' activity.
    /// </summary>
    public class OrderPurchasedActivityInitializer : IActivityInitializer
    {
        private readonly int _contactId;
        private readonly int _siteId;
        private readonly decimal _amount;
        private readonly int _orderId;
        private readonly string _orderNumber;

        private readonly ActivityTitleBuilder _titleBuilder = new ActivityTitleBuilder();

        public OrderPurchasedActivityInitializer(int contactId, int siteId, decimal amount, int orderId, string orderNumber)
        {
            _contactId = contactId;
            _siteId = siteId;
            _amount = amount;
            _orderId = orderId;
            _orderNumber = orderNumber;
        }

        public string ActivityType => UCommerceActivityTypes.PURCHASE_MADE;

        public string SettingsKeyName => UCommerceActivitySettingKeys.PURCHASE_MADE;

        public void Initialize(IActivityInfo activity)
        {
            activity.ActivityValue = _amount.ToString(CultureInfo.InvariantCulture);
            activity.ActivityItemID = _orderId;
            activity.ActivityTitle = _titleBuilder.CreateTitle(ActivityType, _orderNumber);

            activity.ActivityContactID = _contactId;
            activity.ActivitySiteID = _siteId;
        }
    }
}
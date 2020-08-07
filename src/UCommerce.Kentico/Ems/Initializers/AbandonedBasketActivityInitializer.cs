using System;
using CMS.Activities;
using UCommerce.Kentico.Ems.Activities;

namespace UCommerce.Kentico.Ems.Initializers
{
    /// <summary>
    /// Initializer for the 'Basket is abandoned' activity.
    /// </summary>
    public class AbandonedBasketActivityInitializer : IActivityInitializer
    {
        private readonly int _orderId;
        private readonly Guid _orderGuid;
        private readonly int _contactId;
        private readonly int _siteId;

        private readonly ActivityTitleBuilder _titleBuilder = new ActivityTitleBuilder();

        public AbandonedBasketActivityInitializer(int orderId, Guid orderGuid, int contactId, int siteId)
        {
            _orderId = orderId;
            _orderGuid = orderGuid;
            _contactId = contactId;
            _siteId = siteId;
        }

        public string ActivityType => UCommerceActivityTypes.BASKET_ABANDONED;

        public string SettingsKeyName => UCommerceActivitySettingKeys.BASKET_ABANDONED;

        public void Initialize(IActivityInfo activity)
        {
            activity.ActivityValue = _orderGuid.ToString();
            activity.ActivityItemID = _orderId;
            activity.ActivityItemDetailID = 0;
            activity.ActivityTitle = _titleBuilder.CreateTitle(ActivityType, _orderGuid.ToString());

            activity.ActivityContactID = _contactId;
            activity.ActivitySiteID = _siteId;
        }
    }
}

using CMS.Activities;
using UCommerce.Kentico.Ems.Activities;

namespace UCommerce.Kentico.Ems.Initializers
{
    /// <summary>
    /// Initializer for the 'Product purchased' activity.
    /// </summary>
    public class ProductPurchasedActivityInitializer : IActivityInitializer
    {
        private readonly int _contactId;
        private readonly int _siteId;
        private readonly int _quantity;
        private readonly string _productName;
        private readonly int _productId;

        private readonly ActivityTitleBuilder _titleBuilder = new ActivityTitleBuilder();

        public ProductPurchasedActivityInitializer(int contactId, int siteId, int quantity, string productName, int productId)
        {
            _contactId = contactId;
            _siteId = siteId;
            _quantity = quantity;
            _productName = productName;
            _productId = productId;
        }

        public string ActivityType => UCommerceActivityTypes.PRODUCT_PURCHASED;

        public string SettingsKeyName => UCommerceActivitySettingKeys.PRODUCT_PURCHASED;

        public void Initialize(IActivityInfo activity)
        {
            activity.ActivityValue = _quantity.ToString();
            activity.ActivityItemID = _productId;
            activity.ActivityTitle = _titleBuilder.CreateTitle(ActivityType, _productName);

            activity.ActivityContactID = _contactId;
            activity.ActivitySiteID = _siteId;
        }
    }
}

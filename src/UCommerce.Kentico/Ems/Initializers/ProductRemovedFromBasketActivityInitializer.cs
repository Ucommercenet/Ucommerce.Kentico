using CMS.Activities;
using UCommerce.Kentico.Ems.Activities;

namespace UCommerce.Kentico.Ems.Initializers
{
    /// <summary>
    /// Initialized for the 'Product removed from basket' activity.
    /// </summary>
    public class ProductRemovedFromBasketActivityInitializer: IActivityInitializer
    {
        private readonly int _quantity;
        private readonly string _productName;
        private readonly int _productId;

        private readonly ActivityTitleBuilder _titleBuilder = new ActivityTitleBuilder();

        public ProductRemovedFromBasketActivityInitializer(int quantity, string productName, int productId)
        {
            _quantity = quantity;
            _productName = productName;
            _productId = productId;
        }

        public string ActivityType => UCommerceActivityTypes.PRODUCT_REMOVED_FROM_BASKET;

        public string SettingsKeyName => UCommerceActivitySettingKeys.PRODUCT_REMOVED_FROM_BASKET;

        public void Initialize(IActivityInfo activity)
        {
            activity.ActivityValue = _quantity.ToString();
            activity.ActivityItemID = _productId;
            activity.ActivityTitle = _titleBuilder.CreateTitle(ActivityType, _productName);
        }
    }
}

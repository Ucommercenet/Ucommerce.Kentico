namespace UCommerce.Kentico.Ems
{
    /// <summary>
    /// The settings key names to be used when getting the settings values in code.
    /// </summary>
    public static class UcommerceSettingsKeys
    {
        // Purchase conversion.
        public const string UcommercePurchaseConversionNameSettingsKey = "UcommercePurchaseConversionName";
        public const string UcommercePurchaseConversionValueSettingsKey = "UcommercePurchaseConversionValue";

        // Add to basket conversion.
        public const string UcommerceAddToBasketConversionNameSettingsKey = "UcommerceAddToBasketConversionName";
        public const string UcommerceAddToBasketConversionValueSettingsKey = "UcommerceAddToBasketConversionValue";

        // Remove from basket conversion.
        public const string UcommerceRemoveFromBasketConversionNameSettingsKey = "UcommerceRemoveFromBasketConversionName";
        public const string UcommerceRemoveFromBasketConversionValueSettingsKey = "UcommerceRemoveFromBasketConversionValue";

        // Abandoned basket
        public const string UcommerceAbandonedBasketsMarkAbandonedBasketAfterPeriodHours = "UcommerceAbandonedBasketPeriod";
    }
}

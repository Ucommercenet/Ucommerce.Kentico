using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.SiteProvider;
using CMS.WebAnalytics;
using UCommerce.EntitiesV2;
using UCommerce.Kentico.Globalization;
using UCommerce.Pipelines;
using UCommerce.Pipelines.UpdateLineItem;

namespace UCommerce.Kentico.Ems.Tasks
{
    public class LogCustomConversionQuantityChanged : IPipelineTask<IPipelineArgs<UpdateLineItemRequest, UpdateLineItemResponse>>
    {
        private readonly IGetConversionValue _conversionValueService;
        private readonly IKenticoLocalizationContext _kenticoLocalizationContext;

        public LogCustomConversionQuantityChanged(IGetConversionValue conversionValueService, IKenticoLocalizationContext kenticoLocalizationContext)
        {
            _conversionValueService = conversionValueService;
            _kenticoLocalizationContext = kenticoLocalizationContext;
        }

        public PipelineExecutionResult Execute(IPipelineArgs<UpdateLineItemRequest, UpdateLineItemResponse> subject)
        {
            string siteName = SiteContext.CurrentSiteName;
            string aliasPath = DocumentContext.CurrentAliasPath;

            // Items are being removed from the basket
            if (subject.Request.Quantity < subject.Request.OrderLine.Quantity)
            {
                LogRemoveFromBasketConversion(subject, siteName, aliasPath);
            }
            
            // Items are being added to the basket
            if(subject.Request.Quantity > subject.Request.OrderLine.Quantity)
            {
                LogAddToBasketConversion(subject, siteName, aliasPath);
            }

            return PipelineExecutionResult.Success;
        }

        private void LogAddToBasketConversion(IPipelineArgs<UpdateLineItemRequest, UpdateLineItemResponse> subject, string siteName, string aliasPath)
        {
            string addToBasketConversionName = GetAddToBasketConversionName();

            if (!string.IsNullOrEmpty(addToBasketConversionName) &&
                AnalyticsHelper.IsLoggingEnabled(siteName, aliasPath, LogExcludingFlags.CheckAll))
            {
                double conversionValue = GetConversionValue(subject.Request.OrderLine,
                    subject.Request.Quantity - subject.Request.OrderLine.Quantity);

                // Log the add to basket conversion according to parameters.
                HitLogProvider.LogConversions(siteName, _kenticoLocalizationContext.PreferredCultureCode, addToBasketConversionName, 0,
                    1, conversionValue);
            }
        }

        private void LogRemoveFromBasketConversion(IPipelineArgs<UpdateLineItemRequest, UpdateLineItemResponse> subject, string siteName, string aliasPath)
        {
            string removeFromBasketConversionName = GetRemoveFromBasketConversionName();

            if (!string.IsNullOrEmpty(removeFromBasketConversionName) &&
                AnalyticsHelper.IsLoggingEnabled(siteName, aliasPath, LogExcludingFlags.CheckAll))
            {
                double conversionValue = GetConversionValue(subject.Request.OrderLine,
                    subject.Request.OrderLine.Quantity - subject.Request.Quantity);

                // Logs the conversion according to the specified parameters.
                HitLogProvider.LogConversions(siteName, _kenticoLocalizationContext.PreferredCultureCode,
                    removeFromBasketConversionName, 0, 1, conversionValue);
            }
        }

        protected virtual string GetAddToBasketConversionName()
        {
            return SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + "." + UcommerceSettingsKeys.UcommerceRemoveFromBasketConversionNameSettingsKey);
        }

        private string GetRemoveFromBasketConversionName()
        {
            return SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + "." + UcommerceSettingsKeys.UcommerceRemoveFromBasketConversionNameSettingsKey);
        }

        protected virtual double GetConversionValue(OrderLine orderLine, int quantity)
        {
            string val = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + "." + UcommerceSettingsKeys.UcommerceAddToBasketConversionValueSettingsKey);

            return _conversionValueService.GetConversionValue(val, (double)(orderLine.Price * quantity));
        }
    }
}


using CMS.DataEngine;
using CMS.WebAnalytics;
using CMS.DocumentEngine;
using CMS.SiteProvider;
using UCommerce.EntitiesV2;
using UCommerce.Kentico.Globalization;
using UCommerce.Pipelines;
using UCommerce.Pipelines.AddToBasket;

namespace UCommerce.Kentico.Ems.Tasks
{
    public class LogCustomConversionAddToBasketTask : IPipelineTask<IPipelineArgs<AddToBasketRequest, AddToBasketResponse>>
    {
        private readonly IGetConversionValue _conversionValueService;
        private readonly IKenticoLocalizationContext _kenticoLocalizationContext;

        public LogCustomConversionAddToBasketTask(IGetConversionValue conversionValueService, IKenticoLocalizationContext kenticoLocalizationContext)
        {
            _conversionValueService = conversionValueService;
            _kenticoLocalizationContext = kenticoLocalizationContext;
        }

        public PipelineExecutionResult Execute(IPipelineArgs<AddToBasketRequest, AddToBasketResponse> subject)
        {
            string siteName = SiteContext.CurrentSiteName;
            string aliasPath = DocumentContext.CurrentAliasPath;

            string conversionName = GetConversionName();

            // Checks that web analytics are enabled in the site's settings.
            // Confirms that the current IP address, alias path and URL extension are not excluded from web analytics tracking.
            if (!string.IsNullOrEmpty(conversionName) && AnalyticsHelper.IsLoggingEnabled(siteName, aliasPath, LogExcludingFlags.CheckAll))
            {
                double conversionValue = GetConversionValue(subject.Response.OrderLine);
                // Logs the conversion according to the specified parameters.
                HitLogProvider.LogConversions(siteName, _kenticoLocalizationContext.PreferredCultureCode, conversionName, 0, 1, conversionValue);
            }

            return PipelineExecutionResult.Success;
        }

        protected virtual string GetConversionName()
        {
            string name = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + "." + UcommerceSettingsKeys.UcommerceAddToBasketConversionNameSettingsKey);

            return name;
        }

        protected virtual double GetConversionValue(OrderLine orderLine)
        {
            string val = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + "." + UcommerceSettingsKeys.UcommerceAddToBasketConversionValueSettingsKey);

            double conversionValue =
                _conversionValueService.GetConversionValue(val, (double)(orderLine.Price));

            return conversionValue;
        }
    }
}

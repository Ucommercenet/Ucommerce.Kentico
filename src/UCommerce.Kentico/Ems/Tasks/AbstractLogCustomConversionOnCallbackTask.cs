using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.SiteProvider;
using CMS.WebAnalytics;
using UCommerce.EntitiesV2;
using UCommerce.Pipelines;

namespace UCommerce.Kentico.Ems.Tasks
{
    /// <summary>
    /// Use this class, when you need to log custom conversions on a call back from a payment gateway.
    /// </summary>
    public abstract class AbstractLogCustomConversionOnCallbackTask : IPipelineTask<PurchaseOrder>
    {
        private readonly IGetConversionValue _conversionValueService;

        protected AbstractLogCustomConversionOnCallbackTask(IGetConversionValue conversionValueService)
        {
            _conversionValueService = conversionValueService;
        }

        public PipelineExecutionResult Execute(PurchaseOrder subject)
        {
            // Need to get the site and alias path like this, 
            // because this code is run on a callback from the payment gateway.
            string siteName = GetSiteName(subject);
            string aliasPath = GetAliasPath(siteName);

            string conversionName = GetConversionName(siteName);

            // Checks that web analytics are enabled in the site's settings.
            // Confirms that the current IP address, alias path and URL extension are not excluded from web analytics tracking.
            if (!string.IsNullOrEmpty(conversionName) && AnalyticsHelper.IsLoggingEnabled(siteName, aliasPath, LogExcludingFlags.CheckAll))
            {
                double conversionValue = GetConversionValue(subject, siteName);

                // Logs the conversion according to the specified parameters.
                LogConversion(subject, siteName, conversionName, conversionValue);
            }

            return PipelineExecutionResult.Success;
        }

        /// <summary>
        /// Override this method to do the actual logging.
        /// </summary>
        protected abstract void LogConversion(PurchaseOrder order, string siteName, string conversionName, double conversionValue);

        protected virtual string GetSiteName(PurchaseOrder order)
        {
            int siteId = int.Parse(order[SetSiteIdOnBasketTask.KenticoSiteIdProperty]);

            SiteInfo site = SiteInfoProvider.GetSiteInfo(siteId);

            return site.SiteName;
        }

        protected virtual string GetAliasPath(string siteName)
        {
            SiteInfo site = SiteInfoProvider.GetSiteInfo(siteName);

            var defaultPath = PageInfoProvider.GetDefaultAliasPath(site.DomainName, site.SiteName);

            return defaultPath;
        }
        protected virtual string GetConversionName(string sitename)
        {
            string name = SettingsKeyInfoProvider.GetValue(sitename + "." + UcommerceSettingsKeys.UcommercePurchaseConversionNameSettingsKey);

            return name;
        }

        protected virtual double GetConversionValue(PurchaseOrder order, string sitename)
        {
            string val = SettingsKeyInfoProvider.GetValue(sitename + "." + UcommerceSettingsKeys.UcommercePurchaseConversionValueSettingsKey);

            double orderValue = order.OrderTotal.HasValue ? (double)order.OrderTotal.Value : 0.0d;
            double conversionValue = _conversionValueService.GetConversionValue(val, orderValue);

            return conversionValue;
        }

        protected virtual double GetDoubleValue(string val)
        {
            if (double.TryParse(val, out var v))
            {
                return v;
            }

            // Default value, if all else fails.
            return 1;
        }

        protected virtual bool IsPercentage(string val)
        {
            val = val.Trim();
            return (val.EndsWith("%"));
        }
    }
}

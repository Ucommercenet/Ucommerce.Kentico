using System.Linq;
using CMS.Base;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using CMS.WebAnalytics;
using UCommerce.EntitiesV2;
using UCommerce.Kentico.Globalization;
using UCommerce.Pipelines;

namespace UCommerce.Kentico.Ems.Tasks
{
    /// <summary>
    /// This class handles the logging of the "Purchase Made" custom conversion.
    /// </summary>
    public class LogCustomConversionPurchaseTask : AbstractLogCustomConversionOnCallbackTask
    {
        private readonly IKenticoLocalizationContext _kenticoLocalizationContext;

        public LogCustomConversionPurchaseTask(IGetConversionValue conversionValueService, IKenticoLocalizationContext kenticoLocalizationContext) : base(conversionValueService)
        {
            _kenticoLocalizationContext = kenticoLocalizationContext;
        }

        protected override void LogConversion(PurchaseOrder order, string siteName, string conversionName, double conversionValue)
        {
            // Logs the conversion according to the specified parameters.
            HitLogProvider.LogConversions(siteName, _kenticoLocalizationContext.PreferredCultureCode, conversionName, 0, 1, conversionValue);
        }
    }
}

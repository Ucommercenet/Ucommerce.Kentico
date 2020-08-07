using System.Linq;
using CMS.Base;
using CMS.Helpers;
using CMS.WebAnalytics;
using Newtonsoft.Json;
using UCommerce.EntitiesV2;
using UCommerce.Kentico.Globalization;

namespace UCommerce.Kentico.Ems.Tasks
{
    // ReSharper disable once InconsistentNaming
    public class LogCustomConversionsForABTestingTask : AbstractLogCustomConversionOnCallbackTask
    {
        private readonly IKenticoLocalizationContext _kenticoLocalizationContext;

        public LogCustomConversionsForABTestingTask(IGetConversionValue conversionValueService, IKenticoLocalizationContext kenticoLocalizationContext) : base(conversionValueService)
        {
            _kenticoLocalizationContext = kenticoLocalizationContext;
        }

        protected override void LogConversion(PurchaseOrder order, string siteName, string conversionName, double conversionValue)
        {
            // If any AB test cookies are set, this means that we do not have to log these conversions manually.
            if (CookieHelper.GetDistinctCookieNames().Any(x => x.StartsWithCSafe("CMSAB")))
            {
                return;
            }

            // No ABTest cookies are set. This indicates that this task is being run as a result of
            // a Payment Gateway callback. We take the names we need to log the conversionNames

            // Log conversions from AB tests.
            string statisticsCodes = order[SetAbTestInformationOnBasketTask.KenticoAbCookieDataName];
            if (!string.IsNullOrEmpty(statisticsCodes))
            {
                var codesDto = JsonConvert.DeserializeObject<StatisticsCodesDto>(statisticsCodes);

                foreach (var part in codesDto.StatisticsCodes)
                {
                    HitLogProvider.LogHit(part, siteName, _kenticoLocalizationContext.PreferredCultureCode, conversionName, 0, 1, conversionValue);
                }
            }
        }
    }
}

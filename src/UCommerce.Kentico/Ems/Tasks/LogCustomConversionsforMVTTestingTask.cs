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
    public class LogCustomConversionsforMVTTestingTask: AbstractLogCustomConversionOnCallbackTask
    {
        private readonly IKenticoLocalizationContext _localizationContext;

        public LogCustomConversionsforMVTTestingTask(IGetConversionValue conversionValueService, IKenticoLocalizationContext localizationContext) : base(conversionValueService)
        {
            _localizationContext = localizationContext;
        }

        protected override void LogConversion(PurchaseOrder order, string siteName, string conversionName, double conversionValue)
        {
            // If any MVT test cookies are set, this means that we do not have to log these conversions manually.
            if (CookieHelper.GetDistinctCookieNames().Any(x => x.StartsWithCSafe("CMSMVT", true)))
            {
                return;
            }

            // No MVT cookies are set. This indicates that this task is being run as a result of
            // a Payment Gateway callback. So instead we take the names we need to log them from the orderproperties

            // Log conversions from MVT tests.
            string statisticCodes = order[SetMvtTestInformationOnBasketTask.KenticoMvtCookieDataName];
            if (!string.IsNullOrEmpty(statisticCodes))
            {
                var codesDto = JsonConvert.DeserializeObject<StatisticsCodesDto>(statisticCodes);

                foreach (var part in codesDto.StatisticsCodes)
                {
                    HitLogProvider.LogHit(part,siteName, _localizationContext.PreferredCultureCode, conversionName, 0, 1, conversionValue);
                }
            }
        }
    }
}

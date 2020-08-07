using System.Collections.Generic;
using System.Linq;
using CMS.Base;
using CMS.Helpers;
using Newtonsoft.Json;
using UCommerce.EntitiesV2;
using UCommerce.Pipelines;

namespace UCommerce.Kentico.Ems.Tasks
{
    public class SetMvtTestInformationOnBasketTask: IPipelineTask<PurchaseOrder>
    {
        public const string KenticoMvtCookieDataName = "_KenticoMVTCookieDataName";
        public PipelineExecutionResult Execute(PurchaseOrder subject)
        {
            var cookieNames = CookieHelper.GetDistinctCookieNames().Where(x => x.StartsWithCSafe("CMSMVT", true)).ToList();

            if (cookieNames.Any())
            {
                StatisticsCodesDto codesDto = new StatisticsCodesDto();
                foreach (var cookieName in cookieNames)
                {
                    var codes = ConvertToStatisticsCode(cookieName, CookieHelper.GetValue(cookieName));

                    foreach (var code in codes)
                    {
                        codesDto.StatisticsCodes.Add(code);
                    }
                }

                string serializedObject = JsonConvert.SerializeObject(codesDto);
                subject[KenticoMvtCookieDataName] = serializedObject;
            }
            else
            {
                subject[KenticoMvtCookieDataName] = string.Empty;
            }

            return PipelineExecutionResult.Success;
        }

        protected virtual IList<string> ConvertToStatisticsCode(string cookieName, string cookieValue)
        {
            var cookieNameShort = cookieName.Substring("CMSMVT".Length);

            var mvtsession = $"mvtconversion;{cookieNameShort};{cookieValue}";

            return new List<string>{mvtsession};
        }
    }
}

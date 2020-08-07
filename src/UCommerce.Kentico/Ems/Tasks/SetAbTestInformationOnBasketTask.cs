using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CMS.Base;
using CMS.Helpers;
using Newtonsoft.Json;
using UCommerce.EntitiesV2;
using UCommerce.Pipelines;

namespace UCommerce.Kentico.Ems.Tasks
{
    /// <summary>
    /// This task stores enough data on the Ucommerce basket, to be able to log all the necessary conversions
    /// for supporting A/B testing split of the conversions.
    /// </summary>
    /// <remarks>
    /// The key to undertand this functionality is the class "CMS.OnlineMarketing.ABHandlers" from the CMS.OnlineMarketing assembly.
    /// </remarks>
    public class SetAbTestInformationOnBasketTask : IPipelineTask<PurchaseOrder>
    {
        public const string KenticoAbCookieDataName = "_KenticoABCookieDataName";

        public PipelineExecutionResult Execute(PurchaseOrder subject)
        {
            var cookieNames = CookieHelper.GetDistinctCookieNames().Where(x => x.StartsWithCSafe("CMSAB")).ToList();

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
                subject[KenticoAbCookieDataName] = serializedObject;
            }
            else
            {
                subject[KenticoAbCookieDataName] = string.Empty;
            }

            return PipelineExecutionResult.Success;
        }

        protected virtual IList<string> ConvertToStatisticsCode(string testName, string cookieValue)
        {
            var testNameShort = testName.Substring("CMSAB".Length);
            var cookie = JsonConvert.DeserializeObject<AbCookieValue>(cookieValue);

            if (cookie.ExcludedFromTest) return new List<string>();

            var absessionfirst = "absessionconversionfirst;" + testNameShort + ";" + cookie.VariantName;
            var abconversion = "abconversion;" + testNameShort + ";" + cookie.VariantName;

            return new List<string> {absessionfirst, abconversion};
        }

        /// <summary>
        /// This class mirrors the internal class called "CMS.OnlineMarketing.ABCookieValue"
        /// from the "CMS.OnlineMarketing" assembly.
        /// </summary>
        public class AbCookieValue
        {
            /// <summary>AB variant name.</summary>
            [DefaultValue("")]
            public string VariantName { get; set; }

            /// <summary>
            /// Indicates whether is visitor included in AB test or not.
            /// If not, Control is always shown and no AB conversions and AB visits are logged.
            /// </summary>
            [DefaultValue(false)]
            public bool ExcludedFromTest { get; set; }

            /// <summary>Conversions performed during this test.</summary>
            public IList<string> Conversions { get; set; }

            /// <summary>Constructor. Sets properties to empty values.</summary>
            public AbCookieValue()
            {
                VariantName = string.Empty;
                Conversions = new List<string>();
            }
        }
    }
}

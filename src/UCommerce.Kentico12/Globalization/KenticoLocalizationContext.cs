using CMS.Localization;
using UCommerce.Kentico.Globalization;

namespace UCommerce.Kentico12.Globalization
{
    public class KenticoLocalizationContext : IKenticoLocalizationContext
    {
        public string PreferredCultureCode
        {
            get { return LocalizationContext.PreferredCultureCode; }
        }
    }
}
using CMS.Localization;

namespace UCommerce.Kentico.Globalization
{
    public class KenticoLocalizationContext : IKenticoLocalizationContext
    {
        public string PreferredCultureCode
        {
            get { return LocalizationContext.PreferredCultureCode; }
        }
    }
}

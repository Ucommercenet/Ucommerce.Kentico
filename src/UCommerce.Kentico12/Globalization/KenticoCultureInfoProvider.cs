using CMS.Localization;
using UCommerce.Kentico.Globalization;
using CultureInfo = UCommerce.Kentico.Globalization.CultureInfo;

namespace UCommerce.Kentico12.Globalization
{
    public class KenticoCultureInfoProvider : IKenticoCultureInfoProvider
    {
        public CultureInfo GetCultureInfoForCulture(string currentDocumentCulture)
        {
            var kenticoCultureInfo = CultureInfoProvider.GetCultureInfoForCulture(currentDocumentCulture);
            return new CultureInfo()
            {
                CultureAlias = kenticoCultureInfo.CultureAlias,
                CultureCode = kenticoCultureInfo.CultureCode,
                CultureName = kenticoCultureInfo.CultureName
            };
        }
    }
}
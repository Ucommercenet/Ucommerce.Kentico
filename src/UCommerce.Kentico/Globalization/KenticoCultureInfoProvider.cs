using CMS.Localization;
using CultureInfo = UCommerce.Kentico.Globalization.CultureInfo;

namespace UCommerce.Kentico.Globalization
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

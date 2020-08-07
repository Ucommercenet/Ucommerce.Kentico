using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Localization;
using CMS.SiteProvider;
using UCommerce.Infrastructure.Globalization;

namespace UCommerce.Kentico.Globalization
{
    public class LanguageService : ILanguageService
    {
        protected virtual IList<Language> AllLanguages { get; set; }

        /// <summary>
        /// Gets all configured languages from Kentico site(s).
        /// </summary>
        /// <returns></returns>
        /// <remarks>This method converts from Kentico cultures to uCommerce <see cref="Language"/>list.</remarks>
        public IList<Language> GetAllLanguages()
        {
            if (AllLanguages == null)
            {
                //Gets a culture for each language configured across Kentico sites. 
                var cultures =
                    CultureInfoProvider.GetCultures()
                        .Source(sourceItem => sourceItem.Join<CultureSiteInfo>("CultureID", "CultureID")).ToList() // Get CultureInfos from Kentico.
                        .GroupBy(x => x.CultureCode).Select(x => x.First()).ToList(); // Disctinct the list on the CultureCode.

                AllLanguages = cultures.Select(ConvertCultureInfoToLanguage).ToList();
            }

            return AllLanguages;
        }

        protected virtual Language ConvertCultureInfoToLanguage(CMS.Localization.CultureInfo cultureInfo)
        {
            return new Language(cultureInfo.CultureName, cultureInfo.CultureCode);
        }
    }
}

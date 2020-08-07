using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.Helpers;
using UCommerce.Catalog;
using UCommerce.EntitiesV2;
using UCommerce.Infrastructure.Globalization;
using UCommerce.Kentico.Globalization;

namespace UCommerce.Kentico.Catalog
{
    public class KenticoUrlService : UrlService
    {
        private readonly IKenticoCultureInfoProvider _kenticoCultureInfoProvider;

        public KenticoUrlService(ILocalizationContext localizationContext, IKenticoCultureInfoProvider kenticoCultureInfoProvider) : base(localizationContext)
        {
            _kenticoCultureInfoProvider = kenticoCultureInfoProvider;
        }

        protected override string GetUrlInternal(ProductCatalog catalog, Category category, Product product)
        {
            var parts = new List<string>();

            if (KenticoUrlLanguagePrefixIsEnabled())
            {
                string currentDocumentCulture = CMS.DocumentEngine.DocumentContext.CurrentDocument.DocumentCulture;
                CultureInfo cultureInfo = _kenticoCultureInfoProvider.GetCultureInfoForCulture(currentDocumentCulture);
                
                if (cultureInfo != null)
                {
                    RequestContext.CurrentURLLangPrefix = !String.IsNullOrEmpty(cultureInfo.CultureAlias) ? cultureInfo.CultureAlias : cultureInfo.CultureCode;
                }

                parts.Add(RequestContext.CurrentURLLangPrefix);
            }

            parts.Add(GetCatalogDescription(catalog));
            parts.AddRange(GetParentCategoryEnumerator(category).Reverse().Select(cat => GetCategoryDescription(cat)));
            parts.Add(GetProductDescription(product));
            parts.Add(catalog != null ? string.Format("c-{0}", catalog.ProductCatalogId) : string.Empty);
            parts.Add(category != null ? string.Format("c-{0}", category.CategoryId) : string.Empty);
            parts.Add(product != null ? string.Format("p-{0}", product.ProductId) : string.Empty);

            var urlEncodedParts = parts
                .Where(p => !string.IsNullOrEmpty(p))
                .Select(p => UrlEncode(p))
                .Where(p => !string.IsNullOrEmpty(p));

            var url = "/" + string.Join("/", urlEncodedParts);

            if (product != null)
            {
                if (product.IsVariant)
                {
                    url += "#" + product.ProductId;
                }
            }
            return url;
        }

        protected override IEnumerable<Category> GetParentCategoryEnumerator(Category category)
        {
            var currentCategory = category;
            while (currentCategory != null)
            {
                yield return currentCategory;
                currentCategory = currentCategory.ParentCategory;
            }
        }

        /// <summary>
        /// Finds out if language prefix in URLs is enabled in Kentico 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// In Kentico we need an additional check for language prefix in URLs e.g.
        /// mydomain.com/en-gb/ 
        /// </remarks>
        protected virtual bool KenticoUrlLanguagePrefixIsEnabled()
        {
            var useLangPrefixForUrls = SettingsKeyInfoProvider.GetBoolValue("CMSUseLangPrefixForUrls");
            var allowUrlsWithoutLanguagePrefixes = SettingsKeyInfoProvider.GetBoolValue("CMSAllowUrlsWithoutLanguagePrefixes");
            return (useLangPrefixForUrls && !allowUrlsWithoutLanguagePrefixes);
        }
    }
}
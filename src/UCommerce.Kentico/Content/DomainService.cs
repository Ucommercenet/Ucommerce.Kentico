using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CMS.SiteProvider;
using UCommerce.Content;

namespace UCommerce.Kentico.Content
{
    /// <summary>
    /// Kentico implementation of <see cref="IDomainService"/>.
    /// </summary>
    public class DomainService : IDomainService
    {
        /// <summary>
        /// Per request cached list of domains.
        /// </summary>
        protected virtual IList<Domain> Domains { get; set; }

        public virtual IEnumerable<Domain> GetDomains()
        {
            if (Domains == null)
            {
                Domains = SiteInfoProvider.GetSites()
                    .Select(x => ConvertKenticoSiteToUCommerceDomain(x)) // Convert Kentico sites to uCommerce domains.
                    .GroupBy(x => x.DomainId).Select(x => x.First()) // Select distinct on the DomainId.
                    .ToList();
            }

            return Domains;
        }

        public virtual Domain GetDomain(string domainName)
        {
            var domain =
                SiteInfoProvider.GetSites()
                    .WhereEquals("SiteDomainName", domainName)
                    .Select(x => ConvertKenticoSiteToUCommerceDomain(x))
                    .FirstOrDefault();

            return domain;
        }

        public virtual Domain GetCurrentDomain()
        {
            if (SiteContext.CurrentSite == null) return null;

            var domain = ConvertKenticoSiteToUCommerceDomain(SiteContext.CurrentSite);
            return domain;
        }

        protected virtual Domain ConvertKenticoSiteToUCommerceDomain(SiteInfo site)
        {
            var domain = new Domain(site.DomainName, site.DomainName, CultureInfo.CreateSpecificCulture(site.DefaultVisitorCulture));
            return domain;
        }

    }
}

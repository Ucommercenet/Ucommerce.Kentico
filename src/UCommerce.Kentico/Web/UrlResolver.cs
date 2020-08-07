using System.Web;
using UCommerce.Presentation.Web;

namespace UCommerce.Kentico.Web
{
    /// <summary>
    /// Kentico implementation of <see cref="IUrlResolver"/>.
    /// </summary>
    public class UrlResolver : IUrlResolver
    {
        public string ResolveUrl(string localUrl)
        {
            var url = VirtualPathUtility.ToAbsolute("~/CMSModules/uCommerce");
            
            if (localUrl.StartsWith(url)) return localUrl;

            return string.Format("{0}{1}", url, localUrl);
        }
    }
}

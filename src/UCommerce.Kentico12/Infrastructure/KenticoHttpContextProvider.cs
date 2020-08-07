using System.Web;
using UCommerce.Kentico.Infrastructure;

namespace UCommerce.Kentico12.Infrastructure
{
    public class KenticoHttpContextProvider: IKenticoHttpContextProvider
    {
        public HttpRequestBase HttpRequestBase()
        {
            return CMS.Helpers.CMSHttpContext.Current.Request;
        }
    }
}

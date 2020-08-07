using System.Web;
using CMS.Helpers;

namespace UCommerce.Kentico.Infrastructure
{
    public class KenticoHttpContextProvider: IKenticoHttpContextProvider
    {
        public HttpRequestBase HttpRequestBase()
        {
            return CMSHttpContext.Current.Request;
        }
    }
}

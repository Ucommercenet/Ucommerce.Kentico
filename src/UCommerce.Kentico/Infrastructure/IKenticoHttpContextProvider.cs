using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UCommerce.Kentico.Infrastructure
{
    public interface IKenticoHttpContextProvider
    {
        HttpRequestBase HttpRequestBase();
    }
}

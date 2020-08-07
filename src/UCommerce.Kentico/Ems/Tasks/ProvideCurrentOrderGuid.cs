using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UCommerce.Api;
using UCommerce.EntitiesV2;
using UCommerce.Kentico.Macros.Fields;

namespace UCommerce.Kentico.Ems.Tasks
{
    public class ProvideCurrentOrderGuid: IProvideCurrentOrderGuid
    {
        /// <summary>
        /// Default implementation of <see cref="IProvideCurrentOrderGuid"/> that will try to get the orderGuid from the HttpContext.Current.Items collection.
        /// This is used by the Ucommerce.CurrentOrder() macro method.
        /// </summary>
        /// <returns>Current OrderGuid during checkout.</returns>
        public string GetCurrentOrderGuid()
        {
            if (HttpContext.Current.Items["orderGuid"] == null)
            {
                return null;
            }

            return HttpContext.Current.Items["orderGuid"].ToString();
        }
    }
}

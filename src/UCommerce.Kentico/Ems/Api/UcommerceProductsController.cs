using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.WebAnalytics.Web.UI.Internal;
using CMS.WebApi;
using UCommerce.EntitiesV2;
using UCommerce.Extensions;
using ObjectFactory = UCommerce.Infrastructure.ObjectFactory;

namespace UCommerce.Kentico.Ems.Api
{
    [AllowOnlyEditor]
    [HandleExceptions]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UcommerceProductsController: ApiController, ISelectorController<BaseSelectorViewModel>
    {        
        public BaseSelectorViewModel Get(string objType, int ID)
        {
            var product = Product.Get(ID);
            if (product == null)
                ThrowBadRequest();

            return new BaseSelectorViewModel
            {
                ID = product.ProductId,
                Text = product.Name
            };
        }

        public IEnumerable<BaseSelectorViewModel> Get(string objType, string name = "", int pageIndex = 0, int pageSize = 10)
        {
            var selectableProductsProvider = ObjectFactory.Instance.Resolve<IProvideSelectableProducts>();
            var products = selectableProductsProvider.GetProductsForSelection(name);
            
            products = products
                .Skip(pageIndex  * pageSize)
                .Take(pageSize);
                
            return products.Select(i => new BaseSelectorViewModel
            {
                ID = i.ProductId,
                Text = $"{i.Sku} - {i.Name}"
            }).ToList();
        }

        private void ThrowBadRequest()
        {
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Specified object is not found or you are not allowed to read the data.")
            });
        }

    }
}

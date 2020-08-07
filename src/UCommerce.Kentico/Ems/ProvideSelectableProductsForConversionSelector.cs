using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCommerce.EntitiesV2;

namespace UCommerce.Kentico.Ems
{
    /// <summary>
    /// Implementation used to provide products that will be selectable for conversions based on Ucommerce activities.
    /// This implementation provides all sellable products filtered by a search term that is provided through the Kentico product picker.
    /// </summary>
    public class ProvideSelectableProductsForConversionSelector: IProvideSelectableProducts
    {
        /// <summary>
        /// Method used to get the product selection for conversions based on Ucommerce activities.
        /// </summary>
        /// <param name="searchTerm">The selection will be filtered by this term, if the product SKU or name contain it.</param>
        /// <returns></returns>
        public IQueryable<Product> GetProductsForSelection(string searchTerm)
        {
            var products = Product.All().Where(x => x.VariantSku != "" || !x.Variants.Any());

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                products = products.Where(x => x.Name.Contains(searchTerm) 
                                            || x.Sku.Contains(searchTerm));
            }

            return products;
        }
    }
}

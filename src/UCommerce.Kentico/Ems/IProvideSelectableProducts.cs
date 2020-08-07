using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCommerce.EntitiesV2;

namespace UCommerce.Kentico.Ems
{
    /// <summary>
    /// Interface that provides a set of selectable products.
    /// Can be overridden to change the default selection available when defining a conversion that relies on a Ucommerce activity where a products are selectable.
    /// </summary>
    interface IProvideSelectableProducts
    {
        /// <summary>
        /// This method should return the products available for selection filtered by a search term provided through the Kentico product picker.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        IQueryable<Product> GetProductsForSelection(string searchTerm);
    }
}

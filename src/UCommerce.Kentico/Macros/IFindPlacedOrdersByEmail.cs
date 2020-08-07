using System.Collections.Generic;
using UCommerce.EntitiesV2;

namespace UCommerce.Kentico.Macros
{
    /// <summary>
    /// Service for finding a basket based on an OrderGuid.
    /// </summary>
    public interface IFindPlacedOrdersByEmail
    {
        /// <summary>
        /// Returns all <see cref="PurchaseOrder"/> that matches the given email value.
        /// </summary>
        /// <param name="email">The email to match.</param>
        /// <returns>The matching <see cref="IList{PurchaseOrder}"/> object.</returns>
        IList<PurchaseOrder> Find(string email);
    }
}

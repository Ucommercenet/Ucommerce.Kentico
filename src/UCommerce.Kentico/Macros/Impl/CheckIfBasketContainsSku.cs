using System;
using System.Linq;
using UCommerce.Api;

namespace UCommerce.Kentico.Macros.Impl
{
    /// <summary>
    /// Service implementing the <see cref="ICheckIfBasketContainsId"/> interface.
    /// </summary>
    public class CheckIfBasketContainsId : ICheckIfBasketContainsId
    {
        /// <summary>
        /// Check current user's basket for a specific id.
        /// </summary>
        /// <param name="id">The id to check for.</param>
        /// <returns>True, if the current user's basket contains the id, false otherwise.</returns>
        public bool BasketContainsId(string id)
        {
            if (TransactionLibrary.HasBasket())
            {
                var basket = TransactionLibrary.GetBasket();
                if (basket == null) return false;

                return basket.PurchaseOrder.OrderLines.Any(x => x["productId"] != null && x["productId"].Equals(id));
            }

            return false;
        }
    }
}

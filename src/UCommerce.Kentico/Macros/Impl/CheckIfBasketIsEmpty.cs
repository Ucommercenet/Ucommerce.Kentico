using System.Linq;
using UCommerce.Api;

namespace UCommerce.Kentico.Macros.Impl
{
    /// <summary>
    /// Service implementing the <see cref="ICheckIfBasketIsEmpty"/> interface.
    /// </summary>
    public class CheckIfBasketIsEmpty : ICheckIfBasketIsEmpty
    {
        /// <summary>
        /// Check if the current user's basket is empty.
        /// </summary>
        public virtual bool IsBasketEmpty()
        {
            if (TransactionLibrary.HasBasket())
            {
                var basket = TransactionLibrary.GetBasket();
                if (basket == null) return true;

                return !basket.PurchaseOrder.OrderLines.Any();
            };
            return true;
        }
    }
}

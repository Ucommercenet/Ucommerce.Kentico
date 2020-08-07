using UCommerce.EntitiesV2;

namespace UCommerce.Kentico.Macros.Impl
{
    /// <summary>
    /// Implements the <see cref="ICheckIfBasketHasAtLeastXProducts"/> service.
    /// </summary>
	public class BasketHasAtLeastXProducts : ICheckIfBasketHasAtLeastXProducts
	{
        /// <summary>
        /// Returns true, if the combined number of products in the basket exceeds <see cref="numberOfProducts"/>.
        /// </summary>
        /// <param name="numberOfProducts">The number of products to exceed.</param>
        /// <param name="basket">The basket to check the number of products in.</param>
        /// <returns>True, if basket contains more products than <see cref="numberOfProducts"/></returns>
		public bool BasketContainsMoreThanXProducts(int numberOfProducts, PurchaseOrder basket)
		{
			//Check how it is represented in Kentico. Might need checks and parsing
			if (numberOfProducts <= 0 || basket == null)
			{
				return false;
			}

		    var productCountInBasket = GetNumberOfProductsInBasket(basket);

		    if (productCountInBasket == 0)
		    {
		        return false;
		    }

		    return (productCountInBasket >= numberOfProducts);
		}

	    protected virtual int GetNumberOfProductsInBasket(PurchaseOrder basket)
	    {
            var productCountInBasket = 0;

            foreach (var orderLine in basket.OrderLines)
            {
                productCountInBasket += orderLine.Quantity;
            }

	        return productCountInBasket;
	    }
    }
}

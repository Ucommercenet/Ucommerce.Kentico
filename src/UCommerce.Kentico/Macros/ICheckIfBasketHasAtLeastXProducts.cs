using UCommerce.EntitiesV2;

namespace UCommerce.Kentico.Macros
{
    /// <summary>
    /// Service for checking if a basket has at least this number of products.
    /// </summary>
	public interface ICheckIfBasketHasAtLeastXProducts
	{
		bool BasketContainsMoreThanXProducts(int numberOfProducts, PurchaseOrder basket);
	}
}

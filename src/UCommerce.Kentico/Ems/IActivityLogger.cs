using System.ComponentModel;
using UCommerce.EntitiesV2;

namespace UCommerce.Kentico.Ems
{
    /// <summary>
    /// Interface for registering EMS activites in Kentico.
    /// </summary>
    public interface IActivityLogger
    {
        /// <summary>
        /// Registers when a product is added to a basket.
        /// </summary>
        /// <param name="quantity">The quantity of items added.</param>
        /// <param name="displayName">The display name of the product added.</param>
        /// <param name="productId">The ID of the product added.</param>
        void ProductAddedToBasket(int quantity, string displayName, int productId);

        /// <summary>
        /// Registers when a product was purchased.
        /// </summary>
        /// <param name="contactId">The ID of the Kentico contact.</param>
        /// <param name="siteId">The ID of the Kentico site.</param>
        /// <param name="quantity">The quantity of items purchased.</param>
        /// <param name="displayName">The display name of the product purchased.</param>
        /// <param name="productId">The ID of the product purchased.</param>
        void ProductPurchased(int contactId, int siteId, int quantity, string displayName, int productId);

        /// <summary>
        /// Registers when an order is purchased.
        /// </summary>
        /// <remarks>
        /// Typically called, when the order is checked out.
        /// </remarks>
        /// <param name="contactId">The ID of the Kentico contact.</param>
        /// <param name="siteId">The ID of the Kentico site.</param>
        /// <param name="amount">The total amount of the order purchased.</param>
        /// <param name="orderId">The ID of the order that was purchased.</param>
        /// <param name="orderNumber">The order number of the order purchased.</param>
        void OrderPurchased(int contactId, int siteId, decimal amount, int orderId, string orderNumber);

        /// <summary>
        /// Registers when a product is removed from a basket.
        /// </summary>
        /// <param name="quantity">The quantity of items removed.</param>
        /// <param name="displayName">The display name of the product removed.</param>
        /// <param name="productId">The ID of the product removed.</param>
        void ProductRemovedFromBasket(int quantity, string displayName, int productId);
    }
}

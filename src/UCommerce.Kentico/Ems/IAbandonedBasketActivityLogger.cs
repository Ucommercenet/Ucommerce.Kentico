using System;

namespace UCommerce.Kentico.Ems
{
    /// <summary>
    /// Log the AbandonedBasket activity for a basket.
    /// </summary>
    public interface IAbandonedBasketActivityLogger
    {
        /// <summary>
        /// Log the AbandonedBasket activity for a given basket.
        /// </summary>
        /// <param name="orderId">The id of the abandoned basket.</param>
        /// <param name="orderGuid">The Guid of the abandoned basket.</param>
        /// <param name="contactId">The ID of the Contact.</param>
        /// <param name="siteId">The ID of the Kentico site.</param>
        void MarkBasketAsAbandoned(int orderId, Guid orderGuid, int contactId, int siteId);
    }
}

using System;
using UCommerce.EntitiesV2;

namespace UCommerce.Kentico.Macros
{
    /// <summary>
    /// Service for finding a <see cref="PurchaseOrder"/> based on an OrderGuid.
    /// </summary>
    public interface IFindPurchaseOrderByOrderGuid
    {
        /// <summary>
        /// Returns the <see cref="PurchaseOrder"/> that matches the given OrderGuid value.
        /// </summary>
        /// <param name="orderGuid">The OrderGuid to match.</param>
        /// <returns>The matching <see cref="PurchaseOrder"/> object.</returns>
        PurchaseOrder FindPurchaseOrder(Guid orderGuid);
    }
}

using System;
using UCommerce.EntitiesV2;

namespace UCommerce.Kentico.Macros
{
    /// <summary>
    /// Service for finding a basket based on an OrderGuid.
    /// </summary>
    public interface IFindBasketByOrderGuid
    {
        /// <summary>
        /// Returns the <see cref="PurchaseOrder"/> that matches the given OrderGuid value.
        /// </summary>
        /// <param name="orderGuid">The OrderGuid to match.</param>
        /// <returns>The matching <see cref="PurchaseOrder"/> object, if it is in the state "Basket".</returns>
        PurchaseOrder Find(Guid orderGuid);
    }
}

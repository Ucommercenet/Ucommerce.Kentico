using System;
using UCommerce.EntitiesV2;

namespace UCommerce.Kentico.Macros.Impl
{
    /// <summary>
    /// Implements the <see cref="IFindBasketByOrderGuid"/> service.
    /// </summary>
    public class FindBasketByOrderGuid : IFindBasketByOrderGuid
    {
        private readonly IFindPurchaseOrderByOrderGuid _orderFinder;

        public FindBasketByOrderGuid(IFindPurchaseOrderByOrderGuid orderFinder)
        {
            _orderFinder = orderFinder;
        }

        /// <summary>
        /// Returns a basket with a matching orderGuid, if found.
        /// </summary>
        /// <param name="orderGuid">The orderGuid to match.</param>
        /// <returns>The matching <see cref="PurchaseOrder"/> object, if it is a basket.</returns>
        public virtual PurchaseOrder Find(Guid orderGuid)
        {
            var order = _orderFinder.FindPurchaseOrder(orderGuid);
            if (order != null && order.OrderStatus.OrderStatusId == 1) return order;

            return null;
        }
    }
}

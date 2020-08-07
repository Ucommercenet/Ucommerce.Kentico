using System;
using UCommerce.EntitiesV2;

namespace UCommerce.Kentico.Macros.Impl
{
    /// <summary>
    /// Implements the <see cref="IFindPurchaseOrderByOrderGuid"/> service.
    /// </summary>
    public class FindPurchaseOrderByOrderGuid : IFindPurchaseOrderByOrderGuid
    {
        private readonly IRepository<PurchaseOrder> _orderRepository;

        public FindPurchaseOrderByOrderGuid(IRepository<PurchaseOrder> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Finds a matching <see cref="PurchaseOrder"/> object from a repository.
        /// </summary>
        /// <param name="orderGuid">The order Guid to match on.</param>
        /// <returns>The macthing <see cref="PurchaseOrder"/> or null if no match was found.</returns>
        public PurchaseOrder FindPurchaseOrder(Guid orderGuid)
        {
            return _orderRepository.SingleOrDefault(x => x.OrderGuid == orderGuid);
        }
    }
}

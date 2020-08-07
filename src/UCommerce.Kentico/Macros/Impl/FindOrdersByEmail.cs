using System.Collections.Generic;
using System.Linq;
using UCommerce.EntitiesV2;
using UCommerce.EntitiesV2.Queries.Orders;

namespace UCommerce.Kentico.Macros.Impl
{
    /// <summary>
    /// Implements the <see cref="IFindPlacedOrdersByEmail"/> service.
    /// </summary>
    public class FindPlacedOrdersByEmail : IFindPlacedOrdersByEmail
    {
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;

        public FindPlacedOrdersByEmail(IRepository<PurchaseOrder> purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        /// <summary>
        /// Returns a list of order that has been placed for the specified email.
        /// </summary>
        /// <param name="email">The email to match.</param>
        /// <returns>The matching <see cref="IList{PurchaseOrder}"/> object.</returns>
        public IList<PurchaseOrder> Find(string email)
        {
            return _purchaseOrderRepository.Select(new PlacedOrdersByEmailQuery(email)).ToList();
        }
    }
}

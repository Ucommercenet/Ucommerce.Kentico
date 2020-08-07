using System;
using System.Collections.Generic;
using UCommerce.EntitiesV2;
using UCommerce.Kentico.Queries;

namespace UCommerce.Kentico.Ems
{
    public class FindAbandonedBasketService : IFindAbandonedBaskets
    {
        private readonly IRepository<AbandonedBasketDataView> _repository;

        public FindAbandonedBasketService(IRepository<AbandonedBasketDataView> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Set this value, if you onty wish to have baskets above a certain value marked as abandoned.
        /// </summary>
        public decimal AmountNeededToBeAbandoned { get; set; }

        public IEnumerable<AbandonedBasketDataView> FindAbandonedBaskets(DateTime fromTime, DateTime toTime, int siteId)
        {
            var abandonedBaskets = _repository.Select(new FindAbandonedBaskets(fromTime, toTime, AmountNeededToBeAbandoned, siteId));

            return abandonedBaskets;
        }
    }
}

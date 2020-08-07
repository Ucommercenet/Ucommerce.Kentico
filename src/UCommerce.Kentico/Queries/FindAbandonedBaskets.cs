using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Transform;
using UCommerce.EntitiesV2.Queries;
using UCommerce.Kentico.Ems.Tasks;

namespace UCommerce.Kentico.Queries
{
    /// <summary>
    /// Specialised query finding abandoned baskets in the Ucommerce database.
    /// </summary>
    /// <remarks>
    /// This query only looks for baskets associated with a specific SiteID.
    /// The reason for this, is that Kentico activity triggers are tied to specific sites.
    /// So we need to "filter" the query to support multi site functionality.
    /// </remarks>
    public class FindAbandonedBaskets : ICannedQuery<AbandonedBasketDataView>
    {
        private readonly DateTime _from;
        private readonly DateTime _to;
        private readonly decimal _minimumAmount;
        private readonly int _siteId;

        public FindAbandonedBaskets(DateTime fromTimestamp, DateTime toTimestamp, decimal minimumAmount, int siteId)
        {
            _from = fromTimestamp;
            _to = toTimestamp;
            _minimumAmount = minimumAmount;
            _siteId = siteId;

            // If the timestamp is too old, MSSQL will fail.
            // Initial value used by Kentico is too old if no check is made.
            if (_from.Year < 1800)
            {
                _from = _from.AddYears(1800 - _from.Year);
            }
        }

        public IEnumerable<AbandonedBasketDataView> Execute(ISession session)
        {
            string sql = @"select o.OrderId, o.OrderGuid, p.Value as ContactIdString, p2.Value as SiteIdString
                             from uCommerce_PurchaseOrder as o
                               inner join uCommerce_OrderProperty as p
                               on o.OrderId = p.OrderId
                               inner join uCommerce_OrderProperty as p2
                               on o.OrderId = p2.OrderId
                             where o.OrderStatusId = 1
                               and o.ModifiedOn >= '{0}'
                               and o.ModifiedOn < '{1}'
                               and o.OrderTotal > {2}
                               and p.[Key] = '{3}'
                               and p2.[Key] = '{4}'";

            var fullSql = string.Format(sql,
                _from.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                _to.ToString("yyyy-MM-dd HH:mm:ss.fff"), 
                _minimumAmount, 
                SetContactIdOnBasketTask.KenticoContactIdProperty,
                SetSiteIdOnBasketTask.KenticoSiteIdProperty);

            // If site id is zero, then all sites are to be considered.
            // If site id is not zero, only baskets for the specific site is to be returned.
            if (_siteId != 0)
            {
                fullSql += string.Format(" and p2.[Value] = {0}", _siteId);
            }

            var query = session.CreateSQLQuery(fullSql);

            query.SetResultTransformer(new AliasToBeanResultTransformer(typeof(AbandonedBasketDataView)));

            return query.List<AbandonedBasketDataView>();
        }
    }
}

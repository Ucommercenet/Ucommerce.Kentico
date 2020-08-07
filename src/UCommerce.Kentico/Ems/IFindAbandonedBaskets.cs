using System;
using System.Collections.Generic;
using UCommerce.Kentico.Queries;

namespace UCommerce.Kentico.Ems
{
    /// <summary>
    /// Service responsible for finding the abandoned baskets.
    /// </summary>
    public interface IFindAbandonedBaskets
    {
        /// <summary>
        /// Find the abandoned baskets in the period between <para>fromTime</para> and <para>toTime</para> for site <para>siteId</para>.
        /// </summary>
        /// <param name="fromTime">Look only at baskets that was updated after this time.</param>
        /// <param name="toTime">Look only at baskets that was updated before this time.</param>
        /// <param name="siteId">Look only at baskets for this site. A value of zero indicates all sites.</param>
        /// <returns>A list of abandoned baskets.</returns>
        IEnumerable<AbandonedBasketDataView> FindAbandonedBaskets(DateTime fromTime, DateTime toTime, int siteId);
    }
}

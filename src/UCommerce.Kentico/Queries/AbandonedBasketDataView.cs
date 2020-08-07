using System;

namespace UCommerce.Kentico.Queries
{
    /// <summary>
    /// DTO for identifying abandoned baskets.
    /// </summary>
    public class AbandonedBasketDataView
    {
        /// <summary>
        /// The OrderId of the abandoned basket.
        /// </summary>
        public int OrderId;

        /// <summary>
        /// The GUID of the abandoned basket.
        /// </summary>
        public Guid OrderGuid;

        /// <summary>
        /// The Kentico ContactID, for the basket owner.
        /// </summary>
        public string ContactIdString;

        /// <summary>
        /// The Kentico SiteID.
        /// </summary>
        public string SiteIdString;

        /// <summary>
        /// Returns the contact id as an int.
        /// </summary>
        /// <returns>The contact id as an int.</returns>
        public int GetContactId()
        {
            return int.Parse(ContactIdString);
        }

        /// <summary>
        /// Returns the site id as an int.
        /// </summary>
        /// <returns>The site id as an int.</returns>
        public int GetSiteId()
        {
            return int.Parse(SiteIdString);
        }
    }
}

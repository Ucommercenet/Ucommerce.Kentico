namespace UCommerce.Kentico.Macros
{
    /// <summary>
    /// Interface encapsulating the functionality checking the current user's basket for a specific id.
    /// </summary>
    public interface ICheckIfBasketContainsId
    {
        /// <summary>
        /// Check current user's basket for a specific id.
        /// </summary>
        /// <param name="id">The id to check for.</param>
        /// <returns>True, if the current user's basket contains the id, false otherwise.</returns>
        bool BasketContainsId(string id);
    }
}

namespace UCommerce.Kentico.Macros
{
    /// <summary>
    /// Interface encapsulating the check for if basket is empty.
    /// </summary>
    public interface ICheckIfBasketIsEmpty
    {
        /// <summary>
        /// Check is the current user's basket is empty.
        /// </summary>
        /// <returns>True, if the basket is empty or non-existing, false otherwise.</returns>
        bool IsBasketEmpty();
    }
}

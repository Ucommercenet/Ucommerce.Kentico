using CMS.Activities;
using UCommerce.EntitiesV2; // Used in documentation.

namespace UCommerce.Kentico.Macros
{
    /// <summary>
    /// Interface encapsulating the check if a particular <see cref="ActivityInfo"/> is linked to a particular <see cref="Product"/>
    /// </summary>
    public interface ICheckActivityLinkedToProduct
    {
        /// <summary>
        /// Returns true, if the identifier given matches the <see cref="Product"/> linked to the <see cref="ActivityInfo"/>
        /// </summary>
        /// <param name="activity">The <see cref="ActivityInfo"/> potentially holding a reference to a <see cref="Product"/>.</param>
        /// <param name="identifier">The identifier of the <see cref="Product"/>.</param>
        /// <returns>True, if the <see cref="ActivityInfo"/> is linked to the <see cref="Product"/></returns>
        bool ActivityLinkedToProduct(ActivityInfo activity, string identifier);
    }
}

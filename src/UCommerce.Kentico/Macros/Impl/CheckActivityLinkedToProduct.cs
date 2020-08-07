using System;
using CMS.Activities;
using UCommerce.EntitiesV2;

namespace UCommerce.Kentico.Macros.Impl
{
    /// <summary>
    /// Service implementing the <see cref="ICheckActivityLinkedToProduct"/> interface.
    /// </summary>
    public class CheckActivityLinkedToProduct : ICheckActivityLinkedToProduct
    {
        /// <summary>
        /// Check if the identifier matches a given product.
        /// </summary>
        /// <param name="activity">The activity logged.</param>
        /// <param name="identifier">The identifier of the product.</param>
        /// <returns>True, if the identifier matches the product linked to the <see cref="ActivityInfo"/></returns>
        public virtual bool ActivityLinkedToProduct(ActivityInfo activity, string identifier)
        {
            if (activity == null) return false;

            Guid productIdentifier;
            if (!Guid.TryParse(identifier, out productIdentifier))
            {
                return false; // Could not interpret the identifier as a Guid.
            }

            var product = FindProductFromProductIdAndVariantId(activity.ActivityItemID, activity.ActivityItemDetailID);
            if (product == null) return false; // No product could be found.

            return productIdentifier == product.Guid;
        }

        protected virtual Product FindProductFromProductIdAndVariantId(int productId, int variantId)
        {
            if (variantId != 0)
            {
                var variant = Product.Get(variantId);
                if (variant == null || !variant.IsVariant) return null; // No variant could be found.

                return variant.ParentProduct;
            }

            return Product.Get(productId);
        }
    }
}

using CMS;
using UCommerce.EntitiesV2;
using UCommerce.Kentico.Macros.Fields;

[assembly: RegisterExtension(typeof(OrderAddressMacroFields), typeof(OrderAddress))]

namespace UCommerce.Kentico.Macros.Fields
{
    /// <summary>
    /// This class registers all the public properties of the <see cref="OrderAddress"/> object to the Kentico Macro engine.
    /// </summary>
    /// <remarks>
    /// Please see the remarks for <see cref="OrderLineMacroFields"/>.
    /// </remarks>
    public class OrderAddressMacroFields : GenericFieldContainer<OrderAddress>
    {
    }
}

using CMS;
using UCommerce.EntitiesV2;
using UCommerce.Kentico.Macros.Fields;

[assembly: RegisterExtension(typeof(PurchaseOrderMacroFields), typeof(PurchaseOrder))]

namespace UCommerce.Kentico.Macros.Fields
{
    /// <summary>
    /// This class registers all the public properties of the <see cref="PurchaseOrder"/> object to the Kentico Macro engine.
    /// </summary>
    /// <remarks>
    /// Please see the remarks for <see cref="OrderLineMacroFields"/>.
    /// </remarks>
    public class PurchaseOrderMacroFields : GenericFieldContainer<PurchaseOrder>
    {
    }
}

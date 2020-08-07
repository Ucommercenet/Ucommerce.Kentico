using CMS.Base;
using CMS.MacroEngine;

namespace UCommerce.Kentico.Macros
{
    /// <summary>
    /// Macro Namespace class.
    /// </summary>
    [Extension(typeof(MacroMethodsBasket))]
    [Extension(typeof(MacroMethodsOrders))]
    public class UcommerceMacroNamespace : MacroNamespace<UcommerceMacroNamespace>
    {
    }
}

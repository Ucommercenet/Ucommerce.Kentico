using CMS;
using UCommerce.EntitiesV2;
using UCommerce.Kentico.Macros.Fields;

[assembly: RegisterExtension(typeof(OrderLineMacroFields), typeof(OrderLine))]

namespace UCommerce.Kentico.Macros.Fields
{
    /// <summary>
    /// This class registers all the public properties of the <see cref="OrderLine"/> object, as custom fields in the Kentico Macro engine.
    /// </summary>
    /// <remarks>
    /// If you have a Kentico Macro that returns <see cref="OrderLine"/> objects, you can reference the properties as though they were fields in a Transformation.
    /// 
    /// For example:
    /// 
    /// The macro expression "{% ApplyTransformation(Ucommerce.GetBasketOrderLines(ActivityValue), "AvenueClothing.Transformations.OrderLinesText") #%}"
    /// calls the macro "UCommerce.GetBasketOrderLines(...)", that returns an IEnumerable of OrderLine object.
    /// 
    /// Inside the transformation "AvenueClothing.Transformations.OrderLinesText", you can now use:
    /// {% Quantity %} to display the value of the Quantity property for OrderLine object.
    /// </remarks>
    public class OrderLineMacroFields : GenericFieldContainer<OrderLine> {}
}

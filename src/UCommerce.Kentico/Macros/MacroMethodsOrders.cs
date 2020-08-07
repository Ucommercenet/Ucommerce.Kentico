using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.PortalEngine;
using UCommerce.EntitiesV2;
using UCommerce.Infrastructure;
using UCommerce.Kentico.Macros.Fields;

namespace UCommerce.Kentico.Macros
{
    /// <summary>
    /// This class holds all the defined macros that involve Ucommerce Purchase orders.
    /// </summary>
    public class MacroMethodsOrders : MacroMethodContainer
    {
        [MacroMethodParam(0, "email", typeof(string), "Email address.")]
        [MacroMethod(typeof(IList<GenericDataContainer<PurchaseOrder>>), "Returns all placed order by email.", 0)]
        public static object PlacedOrderByEmail(EvaluationContext context, params object[] parameters)
        {
            switch (parameters.Length)
            {
                case 1:
                    if (PortalContext.ViewMode.IsDesign() || PortalContext.ViewMode.IsEdit())
                    {
                        return FakeOrders();
                    }

                    var basketFinder = ObjectFactory.Instance.Resolve<IFindPlacedOrdersByEmail>();

                    string emailAddress = ValidationHelper.GetString(parameters[0], string.Empty);
                    if (string.IsNullOrEmpty(emailAddress)) return new List<GenericDataContainer<PurchaseOrder>>();

                    List<GenericDataContainer<PurchaseOrder>> placedOrdersByEmail = basketFinder.Find(emailAddress).Select(x => new GenericDataContainer<PurchaseOrder>(x)).ToList();

                    return placedOrdersByEmail;
                default:
                    // No other overloads are supported.
                    throw new NotSupportedException();
            }
        }

        private static IList<GenericDataContainer<PurchaseOrder>> FakeOrders()
        {
            var purchaseOrder = new PurchaseOrder
            {
                OrderStatus = new OrderStatus(2), // New Order status id.
                BillingAddress = new OrderAddress
                {
                    Country = new Country()
                },
                BillingCurrency = new Currency(),
                Customer = new Customer(),
                OrderLines = {new OrderLine(), new OrderLine()}
            };

            var orders = new List<GenericDataContainer<PurchaseOrder>>
            {
                new GenericDataContainer<PurchaseOrder>(purchaseOrder)
            };

            return orders;
        }
}
}

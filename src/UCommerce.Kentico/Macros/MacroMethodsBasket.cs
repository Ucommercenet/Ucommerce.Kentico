using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.PortalEngine;
using UCommerce.Api;
using UCommerce.EntitiesV2;
using UCommerce.Infrastructure;
using UCommerce.Kentico.Ems;
using UCommerce.Kentico.Macros.Fields;

namespace UCommerce.Kentico.Macros
{
	/// <summary>
	/// This class holds all the defined macros that involve a Ucommerce basket.
	/// </summary>
	public class MacroMethodsBasket : MacroMethodContainer
	{
		[MacroMethod(typeof(bool), "Check if the current basket is empty.", 0)]
		public static object IsBasketEmpty(EvaluationContext context, params object[] parameters)
		{
			switch (parameters.Length)
			{
				case 0:
					var checkBasketIsEmptyService = ObjectFactory.Instance.Resolve<ICheckIfBasketIsEmpty>();
					return checkBasketIsEmptyService.IsBasketEmpty();

				default:
					// No other overloads are supported.
					throw new NotSupportedException();
			}
		}

	    [MacroMethodParam(0, "id", typeof(string), "The id to look for in the basket.")]
        [MacroMethod(typeof(bool), "Check if the current basket contains a specific id.", 1)]
		public static object BasketContainsId(EvaluationContext context, params object[] parameters)
		{
			switch (parameters.Length)
			{
				case 1:
					var checkIfBasketContainsIdService = ObjectFactory.Instance.Resolve<ICheckIfBasketContainsId>();
					return checkIfBasketContainsIdService.BasketContainsId(parameters[0] as string);

				default:
					// No other overloads are supported.
					throw new NotSupportedException();
			}
		}

	    [MacroMethod(typeof(GenericDataContainer<PurchaseOrder>), "The current basket.", 0)]
        public static object CurrentBasket(EvaluationContext context, params object[] parameters)
	    {
            switch (parameters.Length)
            {
                case 0:
                    if (PortalContext.ViewMode.IsDesign() || PortalContext.ViewMode.IsEdit())
                    {
                        return FakeBasket();
                    }

                    if (TransactionLibrary.HasBasket())
                    {
                        var basket = TransactionLibrary.GetBasket();
                        if (basket == null) return string.Empty;

                        return new GenericDataContainer<PurchaseOrder>(basket.PurchaseOrder);
                    }

                    return string.Empty;

                default:
                    // No other overloads are supported.
                    throw new NotSupportedException();
            }
        }

        [MacroMethod(typeof(GenericDataContainer<PurchaseOrder>), "Method that returns order data during checkout. Can only be used for confirmation emails sent by the Ucommerce email service.", 0)]
	    public static object CurrentOrder(EvaluationContext context, params object[] parameters)
        {
            var currentOrderGuidProvider = ObjectFactory.Instance.Resolve<IProvideCurrentOrderGuid>();
            var currentOrderGuid = currentOrderGuidProvider.GetCurrentOrderGuid();
            if (currentOrderGuid != null)
            {
                return new GenericDataContainer<PurchaseOrder>(TransactionLibrary.GetPurchaseOrder(Guid.Parse(HttpContext.Current.Items["orderGuid"].ToString())));
            }

            return FakeBasket();
        }

		[MacroMethodParam(0, "orderGuid", typeof(object), "Order Guid.")]
		[MacroMethod(typeof(PurchaseOrder), "Returns a basket.", 1)]
		public static object GetBasket(EvaluationContext context, params object[] parameters)
		{
			switch (parameters.Length)
			{
				case 1:
					var basketFinder = ObjectFactory.Instance.Resolve<IFindBasketByOrderGuid>();
					Guid orderGuid = ValidationHelper.GetGuid(parameters[0], Guid.Empty);
					var order = basketFinder.Find(orderGuid);

					if (order == null) return string.Empty;

					return order;

                default:
				    // No other overloads are supported.
                    throw new NotSupportedException();
			}
		}


		[MacroMethodParam(0, "orderGuid", typeof(object), "Order Guid.")]
		[MacroMethod(typeof(IEnumerable<OrderLine>), "Returns order lines of the basket.", 1)]
		public static object GetBasketOrderLines(EvaluationContext context, params object[] parameters)
		{
			switch (parameters.Length)
			{
				case 1:
					var basketFinder = ObjectFactory.Instance.Resolve<IFindBasketByOrderGuid>();

                    Guid orderGuid = ValidationHelper.GetGuid(parameters[0], Guid.Empty);
					var order = basketFinder.Find(orderGuid);

					if (order == null) return string.Empty;

					return order.OrderLines.AsEnumerable();

                default:
				    // No other overloads are supported.
                    throw new NotSupportedException();
			}
		}

		[MacroMethodParam(0, "numberOfProducts", typeof(int), "The minimum number of products in the basket.")]
		[MacroMethod(typeof(bool), "Check if the current basket contains at least the specified number of products.", 1)]
        public static object BasketHasAtLeastXProducts(EvaluationContext context, params object[] parameters)
		{
			switch (parameters.Length)
			{
				case 1:
					var basketHasAtLeastXProducts = ObjectFactory.Instance.Resolve<ICheckIfBasketHasAtLeastXProducts>();

			        int x = ValidationHelper.GetInteger(parameters[0], 0);
			        var basket = TransactionLibrary.GetBasket();
			        if (basket == null) return false;

                    return basketHasAtLeastXProducts.BasketContainsMoreThanXProducts(x, basket.PurchaseOrder);

				default:
					// No other overloads are supported.
					throw new NotSupportedException();
			}
		}

	    private static GenericDataContainer<PurchaseOrder> FakeBasket()
	    {
	        var basket = new PurchaseOrder
	        {
	            OrderStatus = new OrderStatus(1), // Basket status id.
                BillingAddress = new OrderAddress
                {
                    Country = new Country()
                },
                BillingCurrency = new Currency(),
                Customer = new Customer(),
                OrderLines = { new OrderLine(), new OrderLine()}
            };

	        return new GenericDataContainer<PurchaseOrder>(basket);
	    }
	}
}

using UCommerce.EntitiesV2;
using UCommerce.Pipelines;
using UCommerce.Pipelines.AddToBasket;

namespace UCommerce.Kentico.Ems.Tasks
{
    /// <summary>
    /// Responsible for adding the product id to the order line properties. 
    /// </summary>
    public class AddProductIdToOrderLineTask : IPipelineTask<IPipelineArgs<AddToBasketRequest, AddToBasketResponse>>
    {
        public const string ProductIdProperty = "productId";

        public PipelineExecutionResult Execute(IPipelineArgs<AddToBasketRequest, AddToBasketResponse> subject)
        {
            var product = subject.Request.Product;

            if (product != null)
            {
                subject.Response.OrderLine[ProductIdProperty] = product.ProductId.ToString();
            }

            return PipelineExecutionResult.Success;
        }
    }
}

using UCommerce.Pipelines;
using UCommerce.Pipelines.AddToBasket;

namespace UCommerce.Kentico.Ems.Tasks
{
    public class RegisterProductAddedToBasketActivityTask : IPipelineTask<IPipelineArgs<AddToBasketRequest, AddToBasketResponse>>
    {
        private readonly IActivityLogger _activityLogger;

        public RegisterProductAddedToBasketActivityTask(IActivityLogger activityLogger)
        {
            _activityLogger = activityLogger;
        }

        public PipelineExecutionResult Execute(IPipelineArgs<AddToBasketRequest, AddToBasketResponse> subject)
        {
            var product = subject.Request.Product;

            if (product != null)
            {
                _activityLogger.ProductAddedToBasket(subject.Request.Quantity, subject.Response.OrderLine.ProductName, product.ProductId);
            }

            return PipelineExecutionResult.Success;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UCommerce.EntitiesV2;
using UCommerce.EntitiesV2.Queries.Catalog;
using UCommerce.Pipelines;
using UCommerce.Pipelines.UpdateLineItem;

namespace UCommerce.Kentico.Ems.Tasks
{
    /// <summary>
    /// This task will log either an AddToBasket or a RemovedFromBasket activity when the UpdateLineItem pipeline runs depending on the quantity of the orderline being updated.
    /// </summary>
    public class RegisterQuantityChangedActivitiesTask : IPipelineTask<IPipelineArgs<UpdateLineItemRequest, UpdateLineItemResponse>>
    {
        private readonly IActivityLogger _activityLogger;
        private readonly IRepository<ProductIdDto> _productRepository;

        public RegisterQuantityChangedActivitiesTask(IActivityLogger activityLogger, IRepository<ProductIdDto> productRepository)
        {
            _activityLogger = activityLogger;
            _productRepository = productRepository;
        }

        public PipelineExecutionResult Execute(IPipelineArgs<UpdateLineItemRequest, UpdateLineItemResponse> subject)
        {
            var queryResult = new List<ProductIdDto>();

            if (string.IsNullOrEmpty(subject.Request.OrderLine.VariantSku))
            {
                queryResult = _productRepository.Select(new ProductIdBySkuQuery(subject.Request.OrderLine.Sku)).ToList();
            }
            else
            {
                queryResult = _productRepository
                    .Select(new ProductIdBySkuAndVariantSkuQuery(subject.Request.OrderLine.Sku,
                        subject.Request.OrderLine.VariantSku)).ToList();
            }

            if (!queryResult.Any())
            {
                return PipelineExecutionResult.Success;
            }

            if (subject.Request.Quantity < subject.Request.OrderLine.Quantity)
            {
                LogProductRemovedFromBasket(subject.Request.OrderLine.Quantity - subject.Request.Quantity, subject.Request.OrderLine.ProductName,
                    queryResult.FirstOrDefault().ProductId);
            }

            if(subject.Request.Quantity > subject.Request.OrderLine.Quantity)
            {
                LogProductAddedToBasket(subject.Request.Quantity - subject.Request.OrderLine.Quantity, subject.Request.OrderLine.ProductName, queryResult.FirstOrDefault().ProductId);
            }

            return PipelineExecutionResult.Success;
        }

        protected virtual void LogProductRemovedFromBasket(int quantity, string productName, int productId)
        {
            _activityLogger.ProductRemovedFromBasket(quantity, productName, productId);
        }

        protected virtual void LogProductAddedToBasket(int quantity, string productName, int productId)
        {
            _activityLogger.ProductAddedToBasket(quantity, productName, productId);
        }
    }
}

using System.Linq;
using UCommerce.EntitiesV2;
using UCommerce.Kentico.Queries;
using UCommerce.Pipelines;

namespace UCommerce.Kentico.Ems.Tasks
{
    public class RegisterOrderPurchasedActivityTask : IPipelineTask<PurchaseOrder>
    {
        private readonly IActivityLogger _activityLogger;
        private readonly IRepository<ProductIdAndVariantIdView> _productIdRepository;

        public RegisterOrderPurchasedActivityTask(IActivityLogger activityLogger, IRepository<ProductIdAndVariantIdView> productIdRepository)
        {
            _activityLogger = activityLogger;
            _productIdRepository = productIdRepository;
        }

        public PipelineExecutionResult Execute(PurchaseOrder subject)
        {
            int contactId = ReadContactId(subject);
            if (contactId < 1) { return PipelineExecutionResult.Success; }

            int siteId = ReadSiteId(subject);
            if (siteId < 1) { return PipelineExecutionResult.Success; }

            foreach (var line in subject.OrderLines)
            {
                int productId;
                if (int.TryParse(line.GetOrderProperty("productId")?.Value, out productId))
                    _activityLogger.ProductPurchased(contactId, siteId, line.Quantity, line.ProductName, productId);
            }

            _activityLogger.OrderPurchased(contactId, siteId, subject.OrderTotal ?? 0.0m, subject.OrderId, subject.OrderNumber);
            
            return PipelineExecutionResult.Success;
        }

        protected virtual int ReadContactId(PurchaseOrder basket)
        {
            var contactIdAsString = basket[SetContactIdOnBasketTask.KenticoContactIdProperty];
            int contactId;

            if (!int.TryParse(contactIdAsString, out contactId))
            {
                return -1;
            }
            
            return contactId;
        }

        protected virtual int ReadSiteId(PurchaseOrder basket)
        {
            var siteIdAsString = basket[SetSiteIdOnBasketTask.KenticoSiteIdProperty];
            int siteId;

            if (!int.TryParse(siteIdAsString, out siteId))
            {
                return -1;
            }

            return siteId;
        }
    }
}

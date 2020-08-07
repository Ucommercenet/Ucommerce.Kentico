using System.Web;
using UCommerce.EntitiesV2;
using UCommerce.Pipelines;
using Guid = System.Guid;

namespace UCommerce.Kentico.Ems.Tasks
{
    public class SetOrderGuidOnHttpContextItems: IPipelineTask<PurchaseOrder>
    {
        public PipelineExecutionResult Execute(PurchaseOrder subject)
        {
            if (subject.OrderGuid != Guid.Empty)
            {
                HttpContext.Current.Items["orderGuid"] = subject.OrderGuid;
            }

            return PipelineExecutionResult.Success;
        }
    }
}

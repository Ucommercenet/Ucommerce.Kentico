using CMS.SiteProvider;
using UCommerce.EntitiesV2;
using UCommerce.Pipelines;

namespace UCommerce.Kentico.Ems.Tasks
{
    /// <summary>
    /// Responsible for saving the Kentico SiteId on a basket, to be used in later processing outside web context.
    /// </summary>
    public class SetSiteIdOnBasketTask : IPipelineTask<PurchaseOrder>
    {
        public const string KenticoSiteIdProperty = "_KenticoSiteId";

        /// <summary>
        /// Sets the "_KenticoContactId" dynamic order property on the <see cref="PurchaseOrder"/> object.
        /// </summary>
        public PipelineExecutionResult Execute(PurchaseOrder subject)
        {
            subject[KenticoSiteIdProperty] = SiteContext.CurrentSiteID.ToString();

            return PipelineExecutionResult.Success;
        }
    }
}

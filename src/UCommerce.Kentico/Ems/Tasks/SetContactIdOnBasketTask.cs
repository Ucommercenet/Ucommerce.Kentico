using CMS.ContactManagement;
using UCommerce.EntitiesV2;
using UCommerce.Pipelines;

namespace UCommerce.Kentico.Ems.Tasks
{
    /// <summary>
    /// Responsible for saving the Kentico ContactId on a basket, to be used in later processing outside web context.
    /// </summary>
    public class SetContactIdOnBasketTask : IPipelineTask<PurchaseOrder>
    {
        public const string KenticoContactIdProperty = "_KenticoContactId";

        /// <summary>
        /// Sets the "_KenticoContactId" dynamic order property on the <see cref="PurchaseOrder"/> object.
        /// </summary>
        public PipelineExecutionResult Execute(PurchaseOrder subject)
        {
            var currentContact = ContactManagementContext.GetCurrentContact();
            if (currentContact == null) { return PipelineExecutionResult.Success; }

            subject[KenticoContactIdProperty] = currentContact.ContactID.ToString();

            return PipelineExecutionResult.Success;
        }
    }
}

using CMS.Scheduler;
using System;
using CMS.ContactManagement;
using CMS.DataEngine;
using UCommerce.Kentico.Ems.Activities;
using UCommerce.Kentico.Queries;
using ObjectFactory = UCommerce.Infrastructure.ObjectFactory;

namespace UCommerce.Kentico.Ems.Tasks
{
    public class MarkBasketsAsAbandonedTask : ITask
    {
        public string Execute(TaskInfo task)
        {
            try
            {
                DateTime toTimestamp = DateTime.Now.AddHours(0.0 - SettingsKeyInfoProvider.GetDoubleValue(UcommerceSettingsKeys.UcommerceAbandonedBasketsMarkAbandonedBasketAfterPeriodHours, task.TaskSiteID));
                DateTime fromTimestamp = task.TaskLastRunTime == TaskInfoProvider.NO_TIME ? TaskInfoProvider.NO_TIME : toTimestamp.Subtract(DateTime.Now.Subtract(task.TaskLastRunTime));

                var finder = ObjectFactory.Instance.Resolve<IFindAbandonedBaskets>();
                var abandonedBaskets = finder.FindAbandonedBaskets(fromTimestamp, toTimestamp, task.TaskSiteID);

                var activityLogger = ObjectFactory.Instance.Resolve<IAbandonedBasketActivityLogger>();

                foreach (AbandonedBasketDataView basket in abandonedBaskets)
                {
                    if (ContactHasEmail(basket.GetContactId()))
                    {
                        activityLogger.MarkBasketAsAbandoned(basket.OrderId, basket.OrderGuid, basket.GetContactId(), basket.GetSiteId());
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected virtual bool ContactHasEmail(int contactId)
        {
            ContactInfo contact = ContactInfoProvider.GetContactInfo(contactId);
            return (contact != null && !string.IsNullOrWhiteSpace(contact.ContactEmail));
        }
    }
}

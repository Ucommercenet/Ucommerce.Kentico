using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS;
using CMS.Activities;
using CMS.DataEngine;
using CMS.WebAnalytics.Web.UI;
using UCommerce.Kentico.Ems.Activities;

[assembly: RegisterImplementation(typeof(IActivityTypeService), typeof(UcommerceActivityTypeService))]
namespace UCommerce.Kentico.Ems.Activities
{
    public class UcommerceActivityTypeService: IActivityTypeService
    {
        private ICollection<string> _ucommerceConversionActivities = new[] {
            UCommerceActivityTypes.PRODUCT_ADDED_TO_BASKET,
            UCommerceActivityTypes.PRODUCT_REMOVED_FROM_BASKET,
            UCommerceActivityTypes.PRODUCT_PURCHASED,
            UCommerceActivityTypes.PURCHASE_MADE,
            UCommerceActivityTypes.BASKET_ABANDONED
        };

        public IEnumerable<ActivityTypeViewModel> GetActivityTypeViewModels(ICollection<string> allowedActivities, bool isSiteContentOnly)
        {
            var activities = ActivityTypeInfoProvider.GetActivityTypes()
                .WhereTrue(nameof(ActivityTypeInfo.ActivityTypeEnabled))
                .Where(
                    new WhereCondition()
                    .WhereIn(nameof(ActivityTypeInfo.ActivityTypeName), allowedActivities)
                    .Or()
                    .WhereIn(nameof(ActivityTypeInfo.ActivityTypeName), _ucommerceConversionActivities)
                );

            if (isSiteContentOnly)
            {
                activities = activities.WhereFalse(nameof(ActivityTypeInfo.ActivityTypeIsHiddenInContentOnly));
            }

            return activities.ToList()
                .Select(type => new ActivityTypeViewModel
                {
                    Type = type.ActivityTypeName,
                    DisplayName = type.ActivityTypeDisplayName
                });
        }
    }
}

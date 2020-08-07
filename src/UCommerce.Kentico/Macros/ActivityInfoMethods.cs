using System;
using CMS.Activities;
using CMS.MacroEngine;
using UCommerce.Infrastructure;

namespace UCommerce.Kentico.Macros
{
    /// <summary>
    /// Online Marketing Methods - holds macros for data tied to an Activity.
    /// </summary>
    /// <remarks>
    /// Currently supports links to Products.
    /// </remarks>
    public class ActivityInfoMethods : MacroMethodContainer
    {
        /// <summary>
        /// Returns if activity is linked to product given in parameter.
        /// </summary>
        /// <param name="context">Evaluation context with child resolver</param>
        /// <param name="parameters">Method parameters</param>
        [MacroMethodParam(0, "activity", typeof(ActivityInfo), "Activity info object.")]
        [MacroMethodParam(1, "objectIdentifier", typeof(string), "Product GUID.")]
        [MacroMethod(typeof(bool), "Returns if activity is linked to given product by GUID.", 2, Name = "LinkedToProduct")]
        public static object ActivityLinkedToProduct(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length == 2)
            {
                var linkedToProductChecker = ObjectFactory.Instance.Resolve<ICheckActivityLinkedToProduct>();
                return linkedToProductChecker.ActivityLinkedToProduct(parameters[0] as ActivityInfo, parameters[1] as string);
            }

            throw new NotSupportedException();
        }
    }
}

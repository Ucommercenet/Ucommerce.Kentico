using System;
using UCommerce.Pipelines;
using UCommerce.Pipelines.SaveEmailProfile;

namespace UCommerce.Kentico.UI
{
    /// <summary>
    /// Pipeline task used in Kentico to prevent NHibernate from throwing "not-null property references a null or transient value" error for the 
    /// emailProfileInformation fields since that data now comes from Kentico.
    /// </summary>
    public class PreventMandatoryEmailProfileInformationFromBeingNull : IPipelineTask<IPipelineArgs<SaveEmailProfileInformationRequest, SaveEmailProfileInformationResponse>>
    {
        public PipelineExecutionResult Execute(IPipelineArgs<SaveEmailProfileInformationRequest, SaveEmailProfileInformationResponse> subject)
        {
            if (subject.Response.EmailProfileInformation != null)
            {
                subject.Response.EmailProfileInformation.FromName = String.Empty;
                subject.Response.EmailProfileInformation.FromAddress = String.Empty;
            }

            return PipelineExecutionResult.Success;
        }
    }
}

using System.Linq;
using UCommerce.Pipelines;
using UCommerce.Presentation.UI;

namespace UCommerce.Kentico.UI
{
    /// <summary>
    /// Removes the common tab in the edit email profil ui because Kentico handles the configuration. 
    /// </summary>
    public class RemoveCommonTabOnEmailProfileUI : IPipelineTask<SectionGroup>
    {
        public PipelineExecutionResult Execute(SectionGroup subject)
        {
            //If the view is not the one that we want to hook into, then do nothing
            if (subject.GetViewName() != Constants.UI.Pages.Settings.EmailProfileType)
            {
                return PipelineExecutionResult.Success;
            }

            var commonTab = subject.Sections.SingleOrDefault(x =>
                x.OriginalName == Constants.UI.Sections.Settings.EmailProfileType.Common);

            if (commonTab != null)
            {
                subject.Sections.Remove(commonTab);
                subject.Controls.Remove(commonTab);
            }

            return PipelineExecutionResult.Success;
        }
    }
}

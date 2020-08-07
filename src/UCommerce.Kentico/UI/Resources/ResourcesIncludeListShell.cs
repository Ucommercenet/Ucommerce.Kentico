using System.Web.UI;
using ClientDependency.Core;
using UCommerce.Presentation.UI.Resources;

namespace UCommerce.Kentico.UI.Resources
{
    [ClientDependency(ClientDependencyType.Css, "css/Kentico/Kentico10/ucommerce.css", "UCommerce", Priority = 10)]
    [ClientDependency(ClientDependencyType.Css, "css/Kentico/Kentico10/bootstrap.min.css", "UCommerce", Priority = 10)]
    [ClientDependency(ClientDependencyType.Css, "css/fonts/css/font-awesome.min.css", "UCommerce", Priority = 10)]
    [ClientDependency(ClientDependencyType.Css, "css/fonts/css/uCommerce-icon-font.css", "UCommerce", Priority = 10)]
    [ClientDependency(ClientDependencyType.Css, "css/Kentico/Kentico10/shell.css", "UCommerce", Priority = 200)]
    public class ResourcesIncludeListShell : Control, IResourcesIncludeList
    {
        public virtual Control GetControl() { return this; }
    }
}

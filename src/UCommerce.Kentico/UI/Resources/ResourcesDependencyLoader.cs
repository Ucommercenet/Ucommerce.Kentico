using ClientDependency.Core.Controls;
using UCommerce.Presentation.Web;

namespace UCommerce.Kentico.UI.Resources
{
    /// <summary>
    /// Kentico implementation of <see cref="ClientDependencyLoader"/>.
    /// </summary>
    public class ResourcesDependencyLoader : ClientDependencyLoader
    {
        public ResourcesDependencyLoader(IUrlResolver urlResolver)
        {
            AddPath("UCommerce", urlResolver.ResolveUrl(""));
            AddPath("shell", urlResolver.ResolveUrl(""));
        }
    }
}

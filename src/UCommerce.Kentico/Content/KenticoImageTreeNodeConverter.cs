using UCommerce.Infrastructure.Components.Windsor;
using UCommerce.Infrastructure.Globalization;
using UCommerce.Tree;
using UCommerce.Web;
using UCommerce.Web.Models;

namespace UCommerce.Kentico.Content
{
	public class KenticoImageTreeNodeConverter : ITreeNodeContentToTreeNodeConverter
	{
		[Mandatory] public IResourceManager ResourceManager { get; set; }

		public NodeItem Convert(ITreeNodeContent node)
		{
			return CreateNodeFromItem(node);
		}

		
		private NodeItem CreateNodeFromItem(ITreeNodeContent node)
		{
            return new NodeItem()
			{
				Id = node.ItemId,
				Name = node.Name,
				NodeType = node.NodeType,
				Icon = ResourceManager.GetLocalizedIcon(node.Icon),
                HasChildren = node.HasChildren,
				AutoLoad = node.AutoLoad,
				DimNode = false,
			};
		}
	}
}

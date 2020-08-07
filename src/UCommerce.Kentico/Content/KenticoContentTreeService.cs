using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCommerce.Tree;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.SiteProvider;
using UCommerce.Tree.Impl;

namespace UCommerce.Kentico.Content
{
	public class KenticoContentTreeService : ITreeContentService
	{
		public ITreeNodeContent GetRoot()
		{
			TreeProvider contentTree = new TreeProvider(MembershipContext.AuthenticatedUser);

			TreeNode root = contentTree.SelectNodes().Path("/").OnCurrentSite().FirstObject;

			if (root != null)
			{
				var rootTreeNode = new TreeNodeContent("root", root.NodeGUID.ToString())
				{
					HasChildren = root.NodeHasChildren,
					Name = string.IsNullOrEmpty(root.NodeName) ? SiteContext.CurrentSiteName : root.NodeName,
					AutoLoad = true
				};
				return rootTreeNode;
			}

			return new TreeNodeContent("root", -1)
			{
				Name = SiteContext.CurrentSiteName
			};
		}

		public IList<ITreeNodeContent> GetChildren(string nodeType, string id)
		{
			var children = new List<ITreeNodeContent>();
            
			if (!Guid.TryParse(id, out var nodeGuid))
				throw new InvalidOperationException("contentId is not a Guid. Kentico only supports Guids for content Guids.");

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			
			TreeNode kenticoTreeNode = tree.SelectNodes().SingleOrDefault(x=>x.NodeGUID == nodeGuid);

			if (kenticoTreeNode != null)
			{
				foreach (var child in kenticoTreeNode.Children)
				{
					var treeNode = new TreeNodeContent("content", child.NodeGUID.ToString())
					{
						Name = child.NodeName,
						HasChildren = child.NodeHasChildren,
					    Icon = PrefixIconPath("/CMSModules/UCommerce/Css/Kentico/Kentico10/images/content-icon.png")
                    };

					children.Add(treeNode);
				}
			}

			return children;
		}

	    private string PrefixIconPath(string iconPath)
	    {
	        var applicationPath = HttpContext.Current.Request.ApplicationPath;

	        if (!iconPath.StartsWith("/"))
	        {
	            iconPath = "/" + iconPath;
	        }

	        return applicationPath + iconPath;
	    }
    }
}

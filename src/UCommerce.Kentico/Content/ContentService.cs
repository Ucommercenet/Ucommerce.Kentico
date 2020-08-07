using System;
using System.Linq;
using System.Web;
using UCommerce.Content;
using CMS.DocumentEngine;
using CMS.Membership;
using UCommerce.Infrastructure;


namespace UCommerce.Kentico.Content
{
    /// <summary>
    /// Kentico implementation of <see cref="IContentService"/>.
    /// </summary>
    public class ContentService : IContentService
    {
        public UCommerce.Content.Content GetContent(string contentId)
        {

			if (!Guid.TryParse(contentId, out var nodeGuid))
				throw new InvalidOperationException("contentId is not an Guid. Kentico only supports Guids for content Guids.");

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            TreeNode kenticoTreeNode = tree.SelectNodes().SingleOrDefault(x => x.NodeGUID == nodeGuid);


			if (kenticoTreeNode != null)
			{
                var content = new UCommerce.Content.Content()
				{
					Id = kenticoTreeNode.NodeGUID.ToString(),
					Name = kenticoTreeNode.NodeName,
					Url = ConvertPathToUrl(kenticoTreeNode.NodeAliasPath),
					Icon = ""
				};

                return content;
			}

			var contentNotFound = new UCommerce.Content.Content()
			{
				Id = contentId,
				Name = "Not found",
				Url = "/",
				Icon = ""
			};

			return contentNotFound;
            
        }

        protected virtual string ConvertPathToUrl(string nodeAliasPath)
        {
            Guard.Against.MissingStartingSlash(nodeAliasPath);

            string relativeUrl = "~" + nodeAliasPath;
            string url = VirtualPathUtility.ToAbsolute(relativeUrl);

            return url;
        }
    }
}

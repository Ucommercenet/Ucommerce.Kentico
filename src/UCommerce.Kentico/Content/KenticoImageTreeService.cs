using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CMS.DataEngine;
using CMS.MediaLibrary;
using CMS.SiteProvider;
using UCommerce.Tree;
using UCommerce.Tree.Impl;
using CMS.WebDAV;
using System.Web.Hosting;

namespace UCommerce.Kentico.Content
{
	public class KenticoImageTreeService : ITreeContentService
	{
		
		public ITreeNodeContent GetRoot()
		{
			var treeNodeContent = new TreeNodeContent("root", -1)
			{
				AutoLoad = true,
				Name = SiteContext.CurrentSiteName
			};

			return treeNodeContent;
		}

		public IList<ITreeNodeContent> GetChildren(string nodeType, string id)
		{

			switch (nodeType)
			{

				case "root":
					{
						var currentSiteId = new SiteInfoIdentifier(SiteContext.CurrentSiteID);
						var mediaLibraries = MediaLibraryInfoProvider.GetMediaLibraries().OnSite(currentSiteId);
						var treeNodes = new List<ITreeNodeContent>();

						var userPermissions = new [] { "read", "modify" };

						foreach (var mediaLibraryInfo in mediaLibraries)
						{

							if (WebDAVHelper.IsCurrentUserAuthorizedPerMediaLibrary(mediaLibraryInfo, userPermissions))
							{
								var treeNode = new TreeNodeContent("mediaLibrary", mediaLibraryInfo.LibraryID)
								{
									Name = mediaLibraryInfo.LibraryDisplayName,
									HasChildren = true,
									Icon = PrefixIconPath("/CMSModules/UCommerce/Css/Kentico/Kentico10/images/gallery-icon.png")
								};

								treeNodes.Add(treeNode);

							}
						}
						return treeNodes;
					}

				case "mediaLibrary":
					{
						var selectedMediaLibrary = MediaLibraryInfoProvider.GetMediaLibraryInfo(int.Parse(id));
						var mediaLibraryFolder = MediaLibraryInfoProvider.GetMediaLibraryFolderPath(selectedMediaLibrary,
							SiteContext.CurrentSiteName);
						CMS.IO.DirectoryInfo directoryInfo = CMS.IO.DirectoryInfo.New(mediaLibraryFolder);

						var treeNodes = new List<ITreeNodeContent>();

						var folderTreeNodes = GetFolderTreeNodesFromDirectory(directoryInfo).ToList();
						treeNodes.AddRange(folderTreeNodes);

						var mediaFilesTreeNodes = GetMediaFilesFromDirectory(directoryInfo).ToList();
						treeNodes.AddRange(mediaFilesTreeNodes);

						return treeNodes;

					}

				case "mediaLibraryFolder":
					{
						var directoryPath = HostingEnvironment.ApplicationPhysicalPath + id.DecodePath();
						CMS.IO.DirectoryInfo directoryInfo = CMS.IO.DirectoryInfo.New(directoryPath);

						var treeNodes = new List<ITreeNodeContent>();

						var folderTreeNodes = GetFolderTreeNodesFromDirectory(directoryInfo).ToList();
						treeNodes.AddRange(folderTreeNodes);

						var mediaFilesTreeNodes = GetMediaFilesFromDirectory(directoryInfo).ToList();
						treeNodes.AddRange(mediaFilesTreeNodes);

						return treeNodes;
					}

				default: return new List<ITreeNodeContent>();
			}
		}

		private IList<ITreeNodeContent> GetFolderTreeNodesFromDirectory(CMS.IO.DirectoryInfo directoryInfo)
		{
			var treeNodes = new List<ITreeNodeContent>();

			string fileHiddenFolder = MediaLibraryHelper.GetMediaFileHiddenFolder(SiteContext.CurrentSiteName);
			string filePreviewSuffix = MediaLibraryHelper.GetMediaFilePreviewSuffix(SiteContext.CurrentSiteName);

			foreach (CMS.IO.DirectoryInfo directory in directoryInfo.GetDirectories())
			{
                                                                                                                                                   
				if (directory.Name != fileHiddenFolder && directory.Name != filePreviewSuffix)
				{

					var directoryTreeNode = new TreeNodeContent("mediaLibraryFolder", directory.FullName.Remove(0, HostingEnvironment.ApplicationPhysicalPath.Length).EncodePath())
					{
						Name = directory.Name,
						HasChildren = (directory.GetDirectories().Length > 0) || (directory.GetFiles().Length > 0),
						Icon = PrefixIconPath("CMSModules/UCommerce/Css/Kentico/Kentico10/images/folder-icon.png")

					};

					treeNodes.Add(directoryTreeNode);

				}
			}
			return treeNodes;
		}

		private IList<ITreeNodeContent> GetMediaFilesFromDirectory(CMS.IO.DirectoryInfo directory)
		{
			var fileTreeNodes = new List<ITreeNodeContent>();

			foreach (var fileInfo in directory.GetFiles())
			{
				var fileTreeNode = new TreeNodeContent("Image", fileInfo.FullName.Remove(0, HostingEnvironment.ApplicationPhysicalPath.Length).EncodePath())
				{
					Name = fileInfo.Name,
					HasChildren = false,
					Icon = PrefixIconPath("CMSModules/UCommerce/Css/Kentico/Kentico10/images/image-icon.png")
				};

				fileTreeNodes.Add(fileTreeNode);
			}

			return fileTreeNodes;
		}

		private string PrefixIconPath(string iconPath)
		{
			var applicationPath = HttpContext.Current.Request.ApplicationPath;

			if (!iconPath.StartsWith("/"))
			{
				iconPath = "/" + iconPath;
			}

		    return Path.Combine(applicationPath, iconPath);
		}
	}
}

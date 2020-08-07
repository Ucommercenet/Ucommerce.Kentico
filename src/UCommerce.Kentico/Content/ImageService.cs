using System;
using System.Web;
using CMS.MediaLibrary;
using UCommerce.Content;
using System.Web.Hosting;
using CMS.IO;
using Path = System.IO.Path;

namespace UCommerce.Kentico.Content
{
	/// <summary>
	/// Kentico implementation of <see cref="IImageService"/>.
	/// </summary>
	public class ImageService : IImageService
	{
		public virtual UCommerce.Content.Content GetImage(string id)
		{
			int mediaLibraryId;

			if (IsMediaLibraryID(id, out mediaLibraryId))
			{
				var mediaLibraryInfo = MediaLibraryInfoProvider.GetMediaLibraryInfo(mediaLibraryId);

				if (mediaLibraryInfo != null)
				{
					var content = CreateMediaLibraryContent(mediaLibraryInfo);
					return content;
				}

				//It could also be a content node with no image
				return CreateImageNotFoundContent();

			}
             
			if (!string.IsNullOrEmpty(id))
			{
				//Deployment to different environment is enabled.
				//var path = HostingEnvironment.ApplicationPhysicalPath + id.DecodePath();
				return CreateFileContent(id);

			}

			return CreateImageNotFoundContent();
		}

		protected virtual string PrepareTeaserPath(string teaserPath)
		{
			var path = teaserPath.Replace("~", string.Empty);
			var applicationPath = HttpContext.Current.Request.ApplicationPath;

			return applicationPath + path;
		}

	    protected virtual string GetAbsolutePath(string path)
	    {
	        if (!path.StartsWith("~/"))
	        {
	            path = "~/" + path;
	        }

            return VirtualPathUtility.ToAbsolute(path);
	    }

	    protected virtual UCommerce.Content.Content CreateMediaLibraryContent(MediaLibraryInfo mediaLibrary)
		{
			var content = new UCommerce.Content.Content()
			{
				Id = Convert.ToString(mediaLibrary.LibraryGUID),
				Name = mediaLibrary.LibraryDisplayName
			};

			if (!string.IsNullOrEmpty(mediaLibrary.LibraryTeaserPath))
			{
				content.Url = PrepareTeaserPath(mediaLibrary.LibraryTeaserPath);
			}
			else
			{
				content.Url = GetAbsolutePath("CMSModules/UCommerce/Css/Kentico/Kentico10/images/image-not-found.png");
			}

			return content;
		}

	    protected virtual UCommerce.Content.Content CreateFileContent(string id)
		{
            var filePath = id.DecodePath();
            //create full path of image in file system

            var path = HostingEnvironment.ApplicationPhysicalPath + filePath;
            
            AbstractStorageProvider provider = StorageHelper.GetStorageProvider(path);
		    if (provider != null)
		    {
		        if (provider.IsExternalStorage)
		        {
		            var imageStorageUrl = provider.FileProviderObject.GetFileUrl(path, null);
		            return new UCommerce.Content.Content()
		            {
		                Id = id,
		                Name = provider.GetFileInfo(path).Name,
		                Url = imageStorageUrl,
		                Icon = imageStorageUrl
		            };
		        }
            }
            
		    if (File.Exists(path))
		    {
		        var image = new System.IO.FileInfo(filePath);

                var content = new UCommerce.Content.Content()
		        {
		            Id = id,
		            Name = image.Name,
		            Url = GetAbsolutePath(filePath),
		            Icon = GetAbsolutePath(filePath)
		        };
		        return content;
            }
            
		    return CreateImageNotFoundContent();
		}

	    protected virtual UCommerce.Content.Content CreateImageNotFoundContent()
		{
			return new UCommerce.Content.Content()
			{
				Id = "ImageNotFound",
				Name = "ImageNotFound",
				Url = GetAbsolutePath("CMSModules/UCommerce/Css/Kentico/Kentico10/images/image-not-found.png"),
			};
		}

	    protected virtual bool IsMediaLibraryID(string id, out int mediaLibraryId)
		{
			return int.TryParse(id, out mediaLibraryId);
		}
	}
}
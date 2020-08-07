using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using UCommerce.Installer;

namespace UCommerce.Kentico.Installer.InstallationSteps
{
    public enum ReaderPosition
    {
        BEFORE_UCOMMERCE_CONTENT,
        INSIDE_UCOMMERCE_CONTENT,
        AFTER_UCOMMERCE_CONTENT
    }

    /// <summary>
    /// A task that adds our custom Ucommerce activities to the JS file in kentico where they are registered.
    /// The file can be found under ~/CMSScripts/CMSModules/CMS.WebAnalytics/Campaign/build.js.
    /// </summary>
    public class ModifyKenticoCampaignBuildJsFile : IInstallationStep
    {
        // Identifier comment. This is used to verify if it is neccessary to append our activities again, or if they are already present
        // It also marks the beggining of the string we are appending.
        private const string BeginningIdentifierComment =
            @"// BEGIN Ucommerce activities - automatically inserted by Ucommerce, do not delete.";

        private const string EndingIdentifierComment = @"// END Ucommerce activities";

        // Javascript code appended as a string. It contains configuration of our custom activities.
        // If new custom activities are added, they should be added here.
        private const string StringToAppend = @"
            // BEGIN Ucommerce activities - automatically inserted by Ucommerce, do not delete.
            UCommerceProductAddedToBasket: {
                selectorLabel: resolveFilter('campaign.conversion.productselector'),
                areParametersRequired: true,
                errorMessage: resolveFilter('campaign.conversion.productisrequired'),
                configuration: {
                    restUrl: application.getData('applicationUrl') + 'cmsapi/UcommerceProducts',
                    restUrlParams: { objType: '' },
                    isRequired: true,
                    resultTemplate: resultTemplate,
                    allowAny: true
                }
            },

            UCommerceProductRemovedFromBasket: {
                selectorLabel: resolveFilter('campaign.conversion.productselector'),
                areParametersRequired: true,
                errorMessage: resolveFilter('campaign.conversion.productisrequired'),
                configuration: {
                    restUrl: application.getData('applicationUrl') + 'cmsapi/UcommerceProducts',
                    restUrlParams: { objType: '' },
                    isRequired: true,
                    resultTemplate: resultTemplate,
                    allowAny: true
                }
            },

            UCommerceProductPurchased: {
                selectorLabel: resolveFilter('campaign.conversion.productselector'),
                areParametersRequired: true,
                errorMessage: resolveFilter('campaign.conversion.productisrequired'),
                configuration: {
                    restUrl: application.getData('applicationUrl') + 'cmsapi/UcommerceProducts',
                    restUrlParams: { objType: '' },
                    isRequired: true,
                    resultTemplate: resultTemplate,
                    allowAny: true
                }
            },

            UCommercePurchaseMade: {
                selectorLabel: '',
                areParametersRequired: false,
                configuration: {}               
            },

            UCommerceBasketAbandoned: {
                selectorLabel: '',
                areParametersRequired: false,
                configuration: {}
            },

            // END Ucommerce activities";

        public void Execute()
        {
            var buildJsFilePath =
                HostingEnvironment.MapPath("~/CMSScripts/CMSModules/CMS.WebAnalytics/Campaign/build.js");
            var modifiedBuildJsFilePath =
                HostingEnvironment.MapPath("~/CMSScripts/CMSModules/CMS.WebAnalytics/Campaign/build.temp.js");
            var backupFileName =
                HostingEnvironment.MapPath("~/CMSScripts/CMSModules/CMS.WebAnalytics/Campaign/build.js.backup");

            if (!File.Exists(buildJsFilePath))
            {
                throw new FileNotFoundException($"Ucommerce installer could not locate the 'build.js' file at {buildJsFilePath}");
            }

            var file = new FileInfo(buildJsFilePath);
            var position = ReaderPosition.BEFORE_UCOMMERCE_CONTENT;

            using (var reader = new StreamReader(file.OpenRead()))
            using (var writer = File.AppendText(modifiedBuildJsFilePath))
            {
                String line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("var activityTypesConfiguration"))
                    {
                        writer.WriteLine(line);
                        writer.WriteLine(StringToAppend);
                        var nextLine = reader.ReadLine();
                        if (nextLine != string.Empty)
                        {
                            writer.WriteLine(nextLine);
                        }
                    }
                    else if (line.Contains(BeginningIdentifierComment))
                    {
                        position = ReaderPosition.INSIDE_UCOMMERCE_CONTENT;
                    }else if (line.Contains(EndingIdentifierComment))
                    {
                        position = ReaderPosition.AFTER_UCOMMERCE_CONTENT;
                    }
                    else
                    {
                        switch (position)
                        {
                            case ReaderPosition.BEFORE_UCOMMERCE_CONTENT:
                            case ReaderPosition.AFTER_UCOMMERCE_CONTENT:
                                writer.WriteLine(line);
                                break;
                            case ReaderPosition.INSIDE_UCOMMERCE_CONTENT:
                                break;
                        }                        
                    }
                }

                reader.Close();
                writer.Close();
            }

            ReplaceOriginalBuildFileWithModifiedBuildFile(modifiedBuildJsFilePath, buildJsFilePath, backupFileName);
        }

        /// <summary>
        /// This method replaces the contents of the original file with the modified one, 
        /// backs up the original file with a .backup extension
        /// and deletes the modified file.
        /// </summary>
        /// <param name="modifiedFile">The file we copy the contents from.</param>
        /// <param name="originalFile">The file we copy the contents to.</param>
        /// <param name="backupFileName">The name of the backup file we create.</param>
        private static void ReplaceOriginalBuildFileWithModifiedBuildFile(string modifiedFile, string originalFile, string backupFileName)
        {
            File.Replace(modifiedFile, originalFile, backupFileName);
        }
    }
}

using CMS.DataEngine;
using UCommerce.Installer;
using UCommerce.Installer.Extensions;

namespace UCommerce.Kentico.Installer.InstallationSteps
{
    /// <summary>
    /// Route hijack "/ucommerceapi" so Kentico ignores that URL.
    /// </summary>
    public class ExcludeUCommerceUrlFromKentico : IInstallationStep
    {
        public void Execute()
        {
            string ucommerceapi = "/ucommerceapi";
            var currentExcludedURLSettingValue = SettingsKeyInfoProvider.GetValue("CMSExcludedURLs");

            if (!currentExcludedURLSettingValue.ToLower().Contains(ucommerceapi.ToLower()))
            {
                if (currentExcludedURLSettingValue.IsSomething() &&
                    !currentExcludedURLSettingValue.ToLower().Contains(ucommerceapi.ToLower()))
                {
                    ucommerceapi = string.Format("{0};{1}", currentExcludedURLSettingValue, ucommerceapi);
                }

                SettingsKeyInfoProvider.SetGlobalValue("CMSExcludedURLs", ucommerceapi);
            }
        }
    }
}

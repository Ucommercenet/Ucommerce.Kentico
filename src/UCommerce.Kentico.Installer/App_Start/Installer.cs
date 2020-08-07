using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Hosting;
using UCommerce.Installer;
using UCommerce.Installer.InstallerSteps;
using UCommerce.Kentico.Installer.InstallationSteps;

namespace UCommerce.Kentico.Installer.App_Start
{
    public class Installer : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(this.PreStart);
        }

        private static object _padLock = new object(); 
        private static bool _installationWasRun = false;
        public void PreStart(object sender, EventArgs e)
        {
            if (!_installationWasRun)
            {
                lock (_padLock)
                {
                    if (!_installationWasRun)
                    {
                        _installationWasRun = InstallInternal();
                    }
                }
            }
        }

        private bool InstallInternal()
        {
            var migrationLoader = new MigrationLoader();
            var databaseScriptDirectory = new DirectoryInfo(HostingEnvironment.MapPath("~/CMSModules/uCommerce/install"));
            var kenticoInstallerLoggingService = new KenticoInstallerLoggingService();
            var databaseAvailabilityService = new KenticoDatabaseAvailabilityService();
            var connectionStringLocator = new KenticoInstallationConnectionStringLocator();
            var dbInstallerCore = new DbInstallerCore(connectionStringLocator, migrationLoader.GetDatabaseMigrations(databaseScriptDirectory), kenticoInstallerLoggingService);
            var runtimeVersionChecker = new RuntimeVersionChecker(connectionStringLocator, kenticoInstallerLoggingService);
            var updateService = new UpdateService(connectionStringLocator, runtimeVersionChecker, databaseAvailabilityService);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            // Installation steps that are run before the update service check will be ran at EVERY application startup.
            // These steps are meant to run every time in order to prevent Ucommerce from breaking in case Kentico is updated, but Ucommerce won't detect a neccessary update.
            new DatabaseInstallerKentico("~/CMSModules/Ucommerce/install").Execute();
            new ModifyKenticoCampaignBuildJsFile().Execute();

            if (!updateService.UpdateIsNeeded())
            {
                kenticoInstallerLoggingService.Log<Installer>(string.Format("Updating uCommerce was not needed. Current version: {0}. Determining took: {1} ms.", runtimeVersionChecker.GetUCommerceRuntimeAssemblyVersion(), stopwatch.ElapsedMilliseconds));
                return true;
            }

            stopwatch.Stop();

            var installationSteps = new List<IInstallationStep>();
            installationSteps.Add(new DatabaseInstallerStep(dbInstallerCore));
            installationSteps.Add(new UpdateUCommerceAssemblyVersionInDatabase(updateService, runtimeVersionChecker, kenticoInstallerLoggingService));
            installationSteps.Add(new BackupFile("~/web.config", "~/web.config.{DateTime.Now.Ticks}.backup", kenticoInstallerLoggingService));
            installationSteps.Add(new UpdateUCommerceApps("~/CMSModules/uCommerce/Apps", kenticoInstallerLoggingService));
            installationSteps.Add(new RenameCustomConfig("~/CMSModules/uCommerce", kenticoInstallerLoggingService));

            //Clean up unused configuration since payment integration has move to apps 
            installationSteps.Add(new DeleteFile("~/CMSModules/uCommerce/Configuration/Payments.config", kenticoInstallerLoggingService));

            installationSteps.Add(new ExcludeUCommerceUrlFromKentico());
            installationSteps.Add(new WebConfigTransformer("~/web.config", GetTransformationsForWebConfig(), kenticoInstallerLoggingService));

            var installer = new UCommerce.Installer.Installer(installationSteps, kenticoInstallerLoggingService);
            installer.Execute();

            return true;
        }

        private IList<Transformation> GetTransformationsForWebConfig()
        {
            IList<Transformation> transformations = new List<Transformation>();

            transformations.Add(new Transformation("~/CMSModules/uCommerce/install/CleanConfig.config"));
            transformations.Add(new Transformation("~/CMSModules/uCommerce/install/uCommerce.config"));
            transformations.Add(new Transformation("~/CMSModules/uCommerce/install/uCommerce.IIS7.config", isIntegrated: true));
            transformations.Add(new Transformation("~/CMSModules/uCommerce/install/NhibernateLogging.config"));
            transformations.Add(new Transformation("~/CMSModules/uCommerce/install/ClientDependency.config"));
            transformations.Add(new Transformation("~/CMSModules/uCommerce/install/ClientDependencyKentico.config"));
            transformations.Add(new Transformation("~/CMSModules/uCommerce/install/ExtensionlessUrlHandler.config"));

            return transformations;
        }

        public void Dispose()
        {
        }
    }
}

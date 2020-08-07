using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using UCommerce.Installer;

namespace UCommerce.Kentico.Installer.InstallationSteps
{
    public class DatabaseInstallerKentico: IInstallationStep
    {
        private readonly DbInstaller _command;

        public DatabaseInstallerKentico(string migrationsPath)
        {
            var migrationsDirectory = new DirectoryInfo(HostingEnvironment.MapPath(migrationsPath));

            IList<Migration> migrations = new MigrationLoader().GetDatabaseMigrations(migrationsDirectory);

            IInstallerLoggingService logging = new KenticoInstallerLoggingService();
            InstallationConnectionStringLocator locator = new KenticoInstallationConnectionStringLocator();

            _command = new DbInstallerKentico(locator, migrations, logging);
        }
        public void Execute()
        {
            _command.InstallDatabase();
        }
    }
}

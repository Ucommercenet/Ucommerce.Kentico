using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCommerce.Installer;

namespace UCommerce.Kentico.Installer.InstallationSteps
{
    /// <summary>
    /// An implementation of DbInstaller that runs migration scripts with the naming convention: "Kentico.00#.sql" against the KENTICO tables.
    /// </summary>
    public class DbInstallerKentico: DbInstaller
    {
        public DbInstallerKentico(InstallationConnectionStringLocator locator, IList<Migration> migrations, IInstallerLoggingService loggingService) : base(locator, migrations, loggingService)
        {
            MigrationName = "Kentico";
        }

        protected override int GetSchemaVersion()
        {
            // We return -1 because the core DbInstaller will then automatically run all the scripts. We do this so we don't have to maintain a SystemVersionKentico table because
            // Ucommerce and Kentico can be in separate databases - which might lead to complications.
            // Instead we will always run the scripts, but we'll make sure they are idempotent.
            return -1;
        }

        protected override void UpdateSchemaVersion(Migration migration)
        {
            // Do nothing as we are not using a table to maintain the schema version for Kentico specific migration scripts, instead all scripts are idempotent and we run them every time.
            return;
        }
    }
}

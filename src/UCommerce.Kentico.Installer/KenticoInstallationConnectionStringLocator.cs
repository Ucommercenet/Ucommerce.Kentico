using System.Configuration;
using UCommerce.Installer;

namespace UCommerce.Kentico.Installer
{
    public class KenticoInstallationConnectionStringLocator : InstallationConnectionStringLocator
    {
        public override string LocateConnectionString()
        {
            var connectionString = LocateConnectionStringInternal("CMSConnectionString");
            if (string.IsNullOrEmpty(connectionString))
                throw new ConfigurationErrorsException("Unable to locate a connection string in connection strings element called 'uCommerce' or 'CMSConnectionString' and connection string configured in CommerceConfiguration does not seem to be valid");

            return connectionString;
        }
    }
}

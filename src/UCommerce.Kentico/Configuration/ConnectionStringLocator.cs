using System.Configuration;

namespace UCommerce.Kentico.Configuration
{
    public class ConnectionStringLocator : UCommerce.Infrastructure.Configuration.ConnectionStringLocator
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

using UCommerce.Installer;

namespace UCommerce.Kentico.Installer
{
    public class KenticoDatabaseAvailabilityService: IDatabaseAvailabilityService
    {
        public bool IsAvailable()
        {
            return true;
        }
    }
}

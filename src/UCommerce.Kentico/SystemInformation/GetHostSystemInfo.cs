using System;
using System.Linq;
using UCommerce.SystemInformation;

namespace UCommerce.Kentico.SystemInformation
{
    public class GetHostSystemInfo : IGetHostSystemInfo
    {
        public HostSystemInfo Get()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var kenticoAssembly = assemblies.FirstOrDefault(x => x.FullName.ToLower().Contains("cms.core"));
            if (kenticoAssembly == null)
            {
                return null;
            }

            return new HostSystemInfo
            {
                Name = "Kentico",
                Version = kenticoAssembly.GetName().Version
            };
        }
    }
}
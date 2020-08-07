using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCommerce.Installer;

namespace UCommerce.Kentico.Installer
{
    public class Kentico10Installer: UCommerce.Installer.Installer
    {
        public Kentico10Installer(IList<IInstallationStep> kentico10InstallationSteps,
            IInstallerLoggingService loggingService)
            : base(kentico10InstallationSteps, loggingService)
        {
            
        }
    }
}

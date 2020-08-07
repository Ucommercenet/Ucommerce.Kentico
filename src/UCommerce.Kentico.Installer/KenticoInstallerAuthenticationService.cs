using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCommerce.Installer;

namespace UCommerce.Kentico.Installer
{
    public class KenticoInstallerAuthenticationService: IInstallerAuthenticationService
    {
        public string LoggedInUserId()
        {
            return "1";
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using UCommerce.Installer;

namespace UCommerce.Kentico.Installer.InstallationSteps
{
    public class MoveDirectory : IInstallationStep
    {
        private readonly string _source;
        private readonly string _target;
        private readonly bool _overwriteExisting;
        private readonly IInstallerLoggingService _loggingService;

        public MoveDirectory(string source, string target, bool overwriteExisting, IInstallerLoggingService loggingService)
        {
            _source = source;
            _target = target;
            _overwriteExisting = overwriteExisting;
            _loggingService = loggingService;
        }

        public void Execute()
        {
            var directoryMover = new DirectoryMover(
                               new DirectoryInfo(HostingEnvironment.MapPath(_source)),
                               new DirectoryInfo(HostingEnvironment.MapPath(_target)),
                               _overwriteExisting);
            directoryMover.Move(ex => _loggingService.Log<Exception>(ex));
        }
    }
}

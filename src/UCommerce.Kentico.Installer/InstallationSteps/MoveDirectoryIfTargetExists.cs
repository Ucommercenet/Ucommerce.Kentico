using System;
using System.IO;
using System.Web.Hosting;
using UCommerce.Installer;

namespace UCommerce.Kentico.Installer.InstallationSteps
{
    public class MoveDirectoryIfTargetExists: IInstallationStep
    {
        private readonly string _source;
        private readonly string _target;
        private readonly IInstallerLoggingService _loggingService;

        public MoveDirectoryIfTargetExists(string source, string target, IInstallerLoggingService loggingService)
        {
            _source = source;
            _target = target;
            _loggingService = loggingService;
        }

        public void Execute()
        {
            DirectoryMoverIfTargetExist simpleInventoryDirectoryMover = new DirectoryMoverIfTargetExist(
                    new DirectoryInfo(HostingEnvironment.MapPath(_source)),
                    new DirectoryInfo(HostingEnvironment.MapPath(_target))
                );
            simpleInventoryDirectoryMover.Move(ex => _loggingService.Log<Exception>(ex));
        }
    }
}

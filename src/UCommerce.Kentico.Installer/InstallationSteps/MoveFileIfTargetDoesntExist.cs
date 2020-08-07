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
    public class MoveFileIfTargetDoesntExist : IInstallationStep
    {
        private readonly string _source;
        private readonly string _target;
        private readonly IInstallerLoggingService _loggingService;

        public MoveFileIfTargetDoesntExist(string source, string target, IInstallerLoggingService loggingService)
        {
            _source = source;
            _target = target;
            _loggingService = loggingService;
        }

        public void Execute()
        {
            var fileMover = new FileMover(new FileInfo(HostingEnvironment.MapPath(_source)),
                new FileInfo(HostingEnvironment.MapPath(_target))
                );
            fileMover.MoveIfDoesntExist(ex => _loggingService.Log<Exception>(ex));
        }
    }
}

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
    public class MoveFile : IInstallationStep
    {
        private readonly string _source;
        private readonly string _target;
        private readonly bool _backup;
        private readonly IInstallerLoggingService _loggingService;

        public MoveFile(string source, string target, bool backup, IInstallerLoggingService loggingService)
        {
            _source = source;
            _target = target;
            _backup = backup;
            _loggingService = loggingService;
        }

        public void Execute()
        {
            var fileMover = new FileMover(new FileInfo(HostingEnvironment.MapPath(_source)),
                new FileInfo(HostingEnvironment.MapPath(_target)));

            fileMover.Move(_backup, ex => _loggingService.Log<Exception>(ex));
        }
    }
}

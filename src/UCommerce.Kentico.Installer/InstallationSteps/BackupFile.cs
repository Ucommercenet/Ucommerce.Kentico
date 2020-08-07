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
    public class BackupFile : IInstallationStep
    {
        private readonly string _source;
        private readonly string _target;
        private readonly IInstallerLoggingService _loggingService;

        public BackupFile(string source, string target, IInstallerLoggingService loggingService)
        {
            _source = source;
            _target = target;
            _loggingService = loggingService;
        }

        public void Execute()
        {
            var fileCopier = new FileCopier(
                   new FileInfo(HostingEnvironment.MapPath(_source)),
                   new FileInfo(HostingEnvironment.MapPath(_target)));
            fileCopier.Copy(ex => _loggingService.Log<Exception>(ex));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using UCommerce.Installer;
using UCommerce.Installer.InstallerSteps;

namespace UCommerce.Kentico.Installer.InstallationSteps
{
    public class WebConfigTransformer : IInstallationStep
    {
        private readonly string _target;
        private readonly IList<Transformation> _transformations;
        private readonly IInstallerLoggingService _loggingService;

        public WebConfigTransformer(string target, IList<Transformation> transformations, IInstallerLoggingService loggingService)
        {
            _target = target;
            _transformations = transformations;
            _loggingService = loggingService;
        }

        public void Execute()
        {
            var mergeConfig = new MergeConfig();
            mergeConfig.InitializeTargetDocumentPath(_target);

            mergeConfig.ReadConnectionStringAttribute();

            using (var configTransformer = new ConfigurationTransformer(new FileInfo(HostingEnvironment.MapPath(_target))))
            {
                foreach (var transformation in _transformations)
                {
                    configTransformer.Transform(new FileInfo(HostingEnvironment.MapPath(transformation.VirtualPath)),
                                            transformation.OnlyIfIisIntegrated,
                                            ex => _loggingService.Log<Exception>(ex));

                }
            }

            mergeConfig.SetConnectionStringAttribute();
        }
    }
}

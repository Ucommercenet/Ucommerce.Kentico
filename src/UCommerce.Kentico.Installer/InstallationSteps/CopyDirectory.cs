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
    class CopyDirectory : IInstallationStep
    {
        private readonly string _source;
        private readonly string _target;
        private readonly bool _overwriteExisting;
        private readonly IInstallerLoggingService _loggingService;

        public CopyDirectory(string source, string target, bool overwriteExisting, IInstallerLoggingService loggingService)
        {
            _source = source;
            _target = target;
            _overwriteExisting = overwriteExisting;
            _loggingService = loggingService;
        }

        public void Execute()
        {
            //var directoryMover = new DirectoryMover(
            //    new DirectoryInfo(HostingEnvironment.MapPath(_source)),
            //    new DirectoryInfo(HostingEnvironment.MapPath(_target)),
            //    _overwriteExisting);
            //directoryMover.Move(ex => _loggingService.Log<Exception>(ex));
            DirectoryCopy(HostingEnvironment.MapPath(_source), HostingEnvironment.MapPath(_target), true);
        }

        private static void DirectoryCopy(
            string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, true);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}

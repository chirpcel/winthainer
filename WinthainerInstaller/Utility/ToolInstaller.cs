using System;
using System.IO;

namespace WinthainerInstaller.Utility
{
    public class ToolInstaller
    {
        public void InstallToolset()
        {
            Console.WriteLine("Getting installation path");
            var appdataDir = Environment.GetEnvironmentVariable("APPDATA");
            var installDir = appdataDir += "/Winthainer";
            Console.WriteLine("Moving files");
            Directory.CreateDirectory(installDir);
            CopyFilesFromDirectory("./WinthainerCommand", installDir);
            CopyFilesFromDirectory("./WinthainerService", installDir);
            Console.WriteLine("Setting path variable");
            SettingPathVariableIfNecessary(installDir);
        }

        private void CopyFilesFromDirectory(string sourceDir, string destDir)
        {
            string[] files = Directory.GetFiles(sourceDir);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var destFile =  Path.Combine(destDir, fileName);
                File.Copy(file, destFile, true);
            }
        }

        private void SettingPathVariableIfNecessary(String addToPath)
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            if (!path.Contains(addToPath))
            {
                var editedPath = path += ";" + addToPath;
                Environment.SetEnvironmentVariable("PATH", editedPath, EnvironmentVariableTarget.User);
            }
        }
    }
}
using System;
using System.Diagnostics;

namespace WinthainerInstaller.Utility
{
    public class DistributionInstaller
    {
        public void InstallDistribution()
        {
            var appdataDir = Environment.GetEnvironmentVariable("APPDATA");
            var installDir = appdataDir += "/Winthainer/dist";
            Console.WriteLine("Importing distribution");
            ImportDistributionToWSL(installDir);
        }
        
        private void ImportDistributionToWSL(string installDir)
        {
            var winthainerProcess = new Process();
            winthainerProcess.StartInfo.FileName = "wsl";
            winthainerProcess.StartInfo.Arguments =
                "--import Winthainer " + installDir + " ./WinthainerDistribution.tar.gz";
            winthainerProcess.Start();
            winthainerProcess.WaitForExit();
        }
    }
}
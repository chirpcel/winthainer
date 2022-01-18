using System;
using System.Diagnostics;

namespace WinthainerInstaller.Utility
{
    public class DistributionInstaller
    {
        public void InstallDistributions()
        {
            var appdataDir = Environment.GetEnvironmentVariable("APPDATA");
            var installDir = appdataDir += "/Winthainer/dist";
            Console.WriteLine("Importing distribution");
            ImportEngineDistributionToWSL(installDir + "/engine");
            ImportDataDistributionToWSL(installDir + "/data");
        }
        
        private void ImportEngineDistributionToWSL(string installDir)
        {
            var winthainerProcess = new Process();
            winthainerProcess.StartInfo.FileName = "wsl";
            winthainerProcess.StartInfo.Arguments =
                "--import winthainer-engine " + installDir + " ./WinthainerEngine.tar.gz --version 2";
            winthainerProcess.Start();
            winthainerProcess.WaitForExit();
        }
        
        private void ImportDataDistributionToWSL(string installDir)
        {
            var winthainerProcess = new Process();
            winthainerProcess.StartInfo.FileName = "wsl";
            winthainerProcess.StartInfo.Arguments =
                "--import winthainer-data " + installDir + " ./WinthainerData.tar.gz --version 2";
            winthainerProcess.Start();
            winthainerProcess.WaitForExit();
        }
    }
}
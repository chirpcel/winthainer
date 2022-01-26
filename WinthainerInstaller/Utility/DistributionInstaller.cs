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
            ImportEngineDistributionToWsl(installDir + "/engine");
            ImportDataDistributionToWsl(installDir + "/data");
        }
        
        private void ImportEngineDistributionToWsl(string installDir)
        {
            DeleteExistingEngineDistribution();
            var winthainerProcess = new Process();
            winthainerProcess.StartInfo.FileName = "wsl";
            winthainerProcess.StartInfo.Arguments =
                "--import winthainer-engine " + installDir + " ./WinthainerEngine.tar.gz --version 2";
            winthainerProcess.Start();
            winthainerProcess.WaitForExit();
            winthainerProcess.Close();
        }

        private void DeleteExistingEngineDistribution()
        {
            var winthainerProcess = new Process();
            winthainerProcess.StartInfo.FileName = "wsl";
            winthainerProcess.StartInfo.Arguments =
                "--unregister winthainer-engine";
            winthainerProcess.Start();
            winthainerProcess.WaitForExit();
            winthainerProcess.Close();
        }
        
        private void ImportDataDistributionToWsl(string installDir)
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
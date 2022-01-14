using System.Diagnostics;
using System.Threading;

namespace WinthainerService.Utility
{
    public class ProcessUtility
    {
        public void StartWinthainerServiceProcess()
        {
            BootWinthainerDistributions();
            var winthainerServiceProcess = new Process();
            winthainerServiceProcess.StartInfo.FileName = "wsl";
            winthainerServiceProcess.StartInfo.Arguments = "-d winthainer-engine -u root dockerd";
            winthainerServiceProcess.StartInfo.CreateNoWindow = true;
            var threadStart = new ThreadStart(
                () =>
                {
                    winthainerServiceProcess.Start();
                }
            );
            var thread = new Thread(threadStart);
            thread.Start();
        }

        private void BootWinthainerDistributions()
        {
            BootWinthainerDataDistribution();
            BootWinthainerEngineDistribution();
            
            // wait 30 seconds to be sure, all systems like iptables are initialized
            Thread.Sleep(30000);
        }

        private void BootWinthainerDataDistribution()
        {
            var winthainerDataDistributionBootProcess = new Process();
            winthainerDataDistributionBootProcess.StartInfo.FileName = "wsl";
            winthainerDataDistributionBootProcess.StartInfo.Arguments = "-d winthainer-data -u root mount --bind ~/winthainer-data /mnt/wsl/winthainer-data";
            winthainerDataDistributionBootProcess.StartInfo.CreateNoWindow = true;
            winthainerDataDistributionBootProcess.WaitForExit();
            winthainerDataDistributionBootProcess.Close();
        }

        private void BootWinthainerEngineDistribution()
        {
            var winthainerEngineDistributionBootProcess = new Process();
            winthainerEngineDistributionBootProcess.StartInfo.FileName = "wsl";
            winthainerEngineDistributionBootProcess.StartInfo.Arguments = "-d winthainer-engine -u root iptables -V";
            winthainerEngineDistributionBootProcess.StartInfo.CreateNoWindow = true;
            winthainerEngineDistributionBootProcess.WaitForExit();
            winthainerEngineDistributionBootProcess.Close();
        }
        
        public void EndWinthainerServiceProcess()
        {
            var winthainerServiceProcessPidDetector = new Process();
            winthainerServiceProcessPidDetector.StartInfo.RedirectStandardOutput = true;
            winthainerServiceProcessPidDetector.StartInfo.FileName = "wsl";
            winthainerServiceProcessPidDetector.StartInfo.Arguments = "-d winthainer-engine -u root pgrep dockerd";
            winthainerServiceProcessPidDetector.StartInfo.CreateNoWindow = true;
            winthainerServiceProcessPidDetector.Start();
            var winthainerServiceProcessPid = "";
            while (!winthainerServiceProcessPidDetector.StandardOutput.EndOfStream)
            {
                winthainerServiceProcessPid += winthainerServiceProcessPidDetector.StandardOutput.ReadLine();
            }
            winthainerServiceProcessPidDetector.WaitForExit();
            winthainerServiceProcessPidDetector.Close();

            var winthainerServiceProcessKiller = new Process();
            winthainerServiceProcessKiller.StartInfo.RedirectStandardOutput = true;
            winthainerServiceProcessKiller.StartInfo.FileName = "wsl";
            winthainerServiceProcessKiller.StartInfo.Arguments = "-d winthainer-engine -u root kill " + winthainerServiceProcessPid;
            winthainerServiceProcessKiller.StartInfo.CreateNoWindow = true;
            winthainerServiceProcessKiller.Start();
            winthainerServiceProcessKiller.WaitForExit();
            winthainerServiceProcessKiller.Close();
        }
    }
}
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace WinthainerService.Utility
{
    public class ProcessUtility
    {
        public void StartWinthainerServiceProcess()
        {
            var logFile = new LogUtility().GetDaemonLogPath();
            var wslLogFile = MapPathToWsl(logFile);
            SetWinthainerDistributionVersion();
            BootWinthainerDistributions();
            var winthainerServiceProcess = new Process();
            winthainerServiceProcess.StartInfo.FileName = "wsl";
            winthainerServiceProcess.StartInfo.Arguments = "-d winthainer-engine -u root dockerd 2> " + wslLogFile;
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

        private void SetWinthainerDistributionVersion()
        {
            var setEngineDistributionVersionProcess = new Process();
            setEngineDistributionVersionProcess.StartInfo.FileName = "wsl";
            setEngineDistributionVersionProcess.StartInfo.Arguments = "--set-version winthainer-engine 2";
            setEngineDistributionVersionProcess.StartInfo.CreateNoWindow = true;
            setEngineDistributionVersionProcess.Start();
            setEngineDistributionVersionProcess.WaitForExit();
            setEngineDistributionVersionProcess.Close();
            
            var setDataDistributionVersionProcess = new Process();
            setDataDistributionVersionProcess.StartInfo.FileName = "wsl";
            setDataDistributionVersionProcess.StartInfo.Arguments = "--set-version winthainer-data 2";
            setDataDistributionVersionProcess.StartInfo.CreateNoWindow = true;
            setDataDistributionVersionProcess.Start();
            setDataDistributionVersionProcess.WaitForExit();
            setDataDistributionVersionProcess.Close();
        }

        private void BootWinthainerDistributions()
        {
            BootWinthainerDataDistribution();
            BootWinthainerEngineDistribution();
            
            // wait 10 seconds to be sure, all systems like iptables are initialized
            Console.WriteLine("Waiting 10 seconds to be sure, all distributions are initialized...");
            Thread.Sleep(10000);
        }

        private void BootWinthainerDataDistribution()
        {
            var winthainerDataDistributionPrepareBootProcess = new Process();
            winthainerDataDistributionPrepareBootProcess.StartInfo.FileName = "wsl";
            winthainerDataDistributionPrepareBootProcess.StartInfo.Arguments = "-d winthainer-data -u root mkdir /mnt/wsl/winthainer-data /root/winthainer-data";
            winthainerDataDistributionPrepareBootProcess.StartInfo.CreateNoWindow = true;
            winthainerDataDistributionPrepareBootProcess.Start();
            winthainerDataDistributionPrepareBootProcess.WaitForExit();
            winthainerDataDistributionPrepareBootProcess.Close();

            var winthainerDataDistributionBootProcess = new Process();
            winthainerDataDistributionBootProcess.StartInfo.FileName = "wsl";
            winthainerDataDistributionBootProcess.StartInfo.Arguments = "-d winthainer-data -u root mount --bind /root/winthainer-data /mnt/wsl/winthainer-data";
            winthainerDataDistributionBootProcess.StartInfo.CreateNoWindow = true;
            winthainerDataDistributionBootProcess.Start();
            winthainerDataDistributionBootProcess.WaitForExit();
            winthainerDataDistributionBootProcess.Close();
        }

        private void BootWinthainerEngineDistribution()
        {
            var winthainerEngineDistributionBootProcess = new Process();
            winthainerEngineDistributionBootProcess.StartInfo.FileName = "wsl";
            winthainerEngineDistributionBootProcess.StartInfo.Arguments = "-d winthainer-engine -u root iptables -V";
            winthainerEngineDistributionBootProcess.StartInfo.CreateNoWindow = true;
            winthainerEngineDistributionBootProcess.Start();
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
            ShutdownWinthainerDistributions();
        }

        private void ShutdownWinthainerDistributions()
        {
            var winthainerEngineShutdownProcess = new Process();
            winthainerEngineShutdownProcess.StartInfo.FileName = "wsl";
            winthainerEngineShutdownProcess.StartInfo.Arguments = "-t winthainer-engine";
            winthainerEngineShutdownProcess.StartInfo.CreateNoWindow = true;
            winthainerEngineShutdownProcess.Start();
            winthainerEngineShutdownProcess.WaitForExit();
            winthainerEngineShutdownProcess.Close();
            
            var winthainerDataShutdownProcess = new Process();
            winthainerDataShutdownProcess.StartInfo.FileName = "wsl";
            winthainerDataShutdownProcess.StartInfo.Arguments = "-t winthainer-data";
            winthainerDataShutdownProcess.StartInfo.CreateNoWindow = true;
            winthainerDataShutdownProcess.Start();
            winthainerDataShutdownProcess.WaitForExit();
            winthainerDataShutdownProcess.Close();
        }
        
        private string MapPathToWsl(string pathToMap)
        {
            if (pathToMap.Length > 2)
            {
                var regex = @"[A-Z][:]";
                var winLetter = pathToMap.Substring(0, 2);
                var match = Regex.Match(winLetter, regex, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    var wslLetter = "/mnt/" + winLetter.Substring(0, 1).ToLower();
                    var mappedPath = pathToMap.Replace(winLetter, wslLetter);
                    mappedPath = mappedPath.Replace("\\", "/");
                    return mappedPath;
                }
            }
            return pathToMap;
        }
    }
}
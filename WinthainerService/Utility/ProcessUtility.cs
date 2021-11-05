using System.Diagnostics;
using System.Threading;

namespace WinthainerService.Utility
{
    public class ProcessUtility
    {
        public void StartWinthainerServiceProcess()
        {
            var winthainerServiceProcess = new Process();
            winthainerServiceProcess.StartInfo.FileName = "wsl";
            winthainerServiceProcess.StartInfo.Arguments = "-d winthainer -u root dockerd";
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

        public void EndWinthainerServiceProcess()
        {
            var winthainerServiceProcessPidDetector = new Process();
            winthainerServiceProcessPidDetector.StartInfo.RedirectStandardOutput = true;
            winthainerServiceProcessPidDetector.StartInfo.FileName = "wsl";
            winthainerServiceProcessPidDetector.StartInfo.Arguments = "-d winthainer -u root pgrep dockerd";
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
            winthainerServiceProcessKiller.StartInfo.Arguments = "-d winthainer -u root kill " + winthainerServiceProcessPid;
            winthainerServiceProcessKiller.StartInfo.CreateNoWindow = true;
            winthainerServiceProcessKiller.Start();
            winthainerServiceProcessKiller.WaitForExit();
            winthainerServiceProcessKiller.Close();
        }
    }
}
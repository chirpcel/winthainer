using System;
using System.Diagnostics;

namespace WinthainerCommand.Utility
{
    public class ProcessUtility
    {
        public void StartWinthainerProcess(string arguments)
        {
            var winthainerProcess = new Process();
            winthainerProcess.StartInfo.FileName = "wsl";
            winthainerProcess.StartInfo.Arguments = "-d winthainer-engine -u root docker " + arguments;
            winthainerProcess.Start();
            winthainerProcess.WaitForExit();
        }
    }
}
using System;
using System.IO;

namespace WinthainerService.Utility
{
    public class LogUtility
    {
        public string GetDaemonLogPath()
        {
            var appdataDir = Environment.GetEnvironmentVariable("APPDATA");
            var installDir = appdataDir += "/Winthainer";
            Directory.CreateDirectory(installDir);
            Directory.CreateDirectory(installDir + "/log");
            return installDir + "/log/daemon.log";
        }
    }
}
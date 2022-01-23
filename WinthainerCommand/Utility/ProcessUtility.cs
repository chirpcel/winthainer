using System;
using System.Diagnostics;
using System.Reflection;

namespace WinthainerCommand.Utility
{
    public class ProcessUtility
    {
        public void StartWinthainerProcess(string arguments)
        {
            if (arguments.Equals("version "))
            {
                string version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;
                Console.WriteLine("Winthainer:");
                Console.WriteLine("  Version:          " + version);
                Console.WriteLine();
            }
            WslEnvUtility.UpdateWslEnv();
            var winthainerProcess = new Process();
            winthainerProcess.StartInfo.FileName = "wsl";
            winthainerProcess.StartInfo.Arguments = "-d winthainer-engine -u root docker " + arguments;
            winthainerProcess.Start();
            winthainerProcess.WaitForExit();
        }
    }
}
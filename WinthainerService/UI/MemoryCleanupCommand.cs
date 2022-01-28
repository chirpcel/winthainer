using System;
using System.Diagnostics;
using System.Windows.Input;

namespace WinthainerService.UI
{
    public class MemoryCleanupCommand : ICommand
    {
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var memoryCleanupProcess = new Process();
            memoryCleanupProcess.StartInfo.FileName = "wsl";
            memoryCleanupProcess.StartInfo.Arguments = "-d winthainer-engine -u root -e /bin/ash -c \"echo 1 > /proc/sys/vm/drop_caches\"";
            memoryCleanupProcess.StartInfo.CreateNoWindow = true;
            memoryCleanupProcess.Start();
            memoryCleanupProcess.Close();
        }

        public event EventHandler? CanExecuteChanged;
    }
}
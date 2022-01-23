using System;
using System.Diagnostics;
using System.Windows.Input;
using WinthainerService.Utility;

namespace WinthainerService.UI
{
    public class OpenLogCommand : ICommand
    {
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var logFile = new LogUtility().GetDaemonLogPath();
            var openLogProcess = new Process();
            openLogProcess.StartInfo.FileName = "notepad.exe";
            openLogProcess.StartInfo.Arguments = logFile;
            openLogProcess.StartInfo.CreateNoWindow = true;
            openLogProcess.Start();
            openLogProcess.Close();
        }

        public event EventHandler? CanExecuteChanged;
    }
}
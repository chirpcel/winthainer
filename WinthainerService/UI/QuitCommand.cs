using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;
using WinthainerService.Utility;

namespace WinthainerService.UI
{
    public class QuitCommand : ICommand
    {
        [DllImport("kernel32.dll")]
        internal static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);
        
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            new ProcessUtility().EndWinthainerServiceProcess();
            Application.Exit();
        }

        public event EventHandler? CanExecuteChanged;
    }
}
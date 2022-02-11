using System;
using System.Windows.Forms;
using WinthainerService.UI;
using WinthainerService.Utility;

namespace WinthainerService
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1 &&  args[0] == "quit")
            {
                new ProcessUtility().EndWinthainerServiceProcess();
                Application.Exit();
            }
            else
            {
                new ProcessUtility().StartWinthainerServiceProcess();
                new TrayIcon().ShowTrayIcon();
                Application.Run();
            }
        }
    }
}
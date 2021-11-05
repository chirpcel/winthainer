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
            new ProcessUtility().StartWinthainerServiceProcess();
            new TrayIcon().ShowTrayIcon();
            Application.Run();
        }
    }
}
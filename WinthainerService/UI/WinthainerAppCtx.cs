using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using WinthainerService.Utility;

namespace WinthainerService.UI
{
    public class WinthainerAppCtx : ApplicationContext
    {

        private NotifyIcon notifyIcon;

        public WinthainerAppCtx()
        {
            notifyIcon = new NotifyIcon()
            {
                Icon = GetTrayIcon(),
                ContextMenuStrip = GetContextMenuStrip(),
                Visible = true,
                Text = "Winthainer"
            };
        }

        private ContextMenuStrip GetContextMenuStrip()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem openLogItem = new ToolStripMenuItem("Open log");
            openLogItem.Click += new EventHandler(OpenLog);
            openLogItem.Name = "Open log";
            contextMenuStrip.Items.Add(openLogItem);
        
            ToolStripMenuItem memoryCleanupItem = new ToolStripMenuItem("Cleanup memory");
            memoryCleanupItem.Click += new EventHandler(MemoryCleanup);
            memoryCleanupItem.Name = "Cleanup memory";
            contextMenuStrip.Items.Add(memoryCleanupItem);
        
            ToolStripMenuItem quitItem = new ToolStripMenuItem("Quit");
            quitItem.Click += new EventHandler(Quit);
            quitItem.Name = "Quit";
            contextMenuStrip.Items.Add(quitItem);

            return contextMenuStrip;
        }

        private void OpenLog(object sender, EventArgs eventArgs)
        {
            var logFile = new LogUtility().GetDaemonLogPath();
            var openLogProcess = new Process();
            openLogProcess.StartInfo.FileName = "notepad.exe";
            openLogProcess.StartInfo.Arguments = logFile;
            openLogProcess.StartInfo.CreateNoWindow = true;
            openLogProcess.Start();
            openLogProcess.Close();
        }
    
        private void MemoryCleanup(object sender, EventArgs eventArgs)
        {
            var memoryCleanupProcess = new Process();
            memoryCleanupProcess.StartInfo.FileName = "wsl";
            memoryCleanupProcess.StartInfo.Arguments = "-d winthainer-engine -u root -e /bin/ash -c \"echo 1 > /proc/sys/vm/drop_caches\"";
            memoryCleanupProcess.StartInfo.CreateNoWindow = true;
            memoryCleanupProcess.Start();
            memoryCleanupProcess.Close();
        }

        private void Quit(object sender, EventArgs eventArgs)
        {
            new ProcessUtility().EndWinthainerServiceProcess();
            Application.Exit();
        }

        private Icon GetTrayIcon()
        {
            var assembly = typeof(WinthainerService.Program).GetTypeInfo().Assembly;
            var resourceName = "WinthainerService.Resources.container-truck.ico";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                return new Icon(stream);
        }

    }
}
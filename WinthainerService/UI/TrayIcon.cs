using System.Windows.Controls;
using Hardcodet.Wpf.TaskbarNotification;
using WinthainerService.Resources;

namespace WinthainerService.UI
{
    public class TrayIcon
    {
        public void ShowTrayIcon()
        {
            TaskbarIcon trayIcon = new TaskbarIcon();
            trayIcon.Icon = EmbeddedResourceLoader.GetTrayIcon();
            trayIcon.ToolTipText = "Winthainer";
            trayIcon.ContextMenu = GetTrayIconContextMenu();
            trayIcon.MenuActivation = PopupActivationMode.RightClick;
        }

        private ContextMenu GetTrayIconContextMenu()
        {
            var openLogItem = new MenuItem()
            {
                Header = "Open Log",
                ToolTip = "Opens the daemon log",
                Command = new OpenLogCommand()
            };
            var quitItem = new MenuItem()
            {
                Header = "Quit",
                ToolTip = "Quit WinthainerService",
                Command = new QuitCommand()
            };
            return new ContextMenu()
            {
                Items = {openLogItem, quitItem}
            };
        }
    }
}
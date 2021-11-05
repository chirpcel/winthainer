using System.Drawing;
using System.IO;
using System.Reflection;

namespace WinthainerService.Resources
{
    public class EmbeddedResourceLoader
    {
        public static Icon GetTrayIcon()
        {
            var assembly = typeof(WinthainerService.Program).GetTypeInfo().Assembly;
            var resourceName = "WinthainerService.Resources.container-truck.ico";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                return new Icon(stream);
        }
    }
}
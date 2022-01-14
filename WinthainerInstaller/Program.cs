using System;
using WinthainerInstaller.Utility;

namespace WinthainerInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Installing Winthainer!");
            Console.WriteLine("Installing toolset...");
            new ToolInstaller().InstallToolset();
            Console.WriteLine("Toolset installed!");
            Console.WriteLine("Installing distribution...");
            new DistributionInstaller().InstallDistribution();
            Console.WriteLine("Distribution installed!");
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
}
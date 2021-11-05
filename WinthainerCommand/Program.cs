using WinthainerCommand.Utility;

namespace WinthainerCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            var parsedArguments = new ArgumentUtility().ParseArguments(args);
            new ProcessUtility().StartWinthainerProcess(parsedArguments);
        }
    }
}
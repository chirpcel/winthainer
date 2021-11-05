namespace WinthainerCommand.Utility
{
    public class ArgumentUtility
    {
        public string ParseArguments(string[] arguments)
        {
            var pathUtility = new PathUtility();
            var parsedArguments = "";
            var nextIsPath = false;
            foreach (var arg in arguments)
            {
                if (arg.Equals("-v"))
                {
                    nextIsPath = true;
                    parsedArguments += arg + " ";
                } else if (nextIsPath)
                {
                    parsedArguments += pathUtility.MapPathToWsl(arg) + " ";
                    nextIsPath = false;
                }
                else
                {
                    parsedArguments += arg + " ";
                }
            }
            return parsedArguments;
        }
    }
}
using System.Text.RegularExpressions;

namespace WinthainerCommand.Utility
{
    public class PathUtility
    {
        public string MapPathToWsl(string pathToMap)
        {
            if (pathToMap.Length > 2)
            {
                var regex = @"[A-Z][:]";
                var winLetter = pathToMap.Substring(0, 2);
                var match = Regex.Match(winLetter, regex, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    var wslLetter = "/mnt/" + winLetter.Substring(0, 1).ToLower();
                    var mappedPath = pathToMap.Replace(winLetter, wslLetter);
                    mappedPath = mappedPath.Replace("\\", "/");
                    return mappedPath;
                }
            }
            return pathToMap;
        }
    }
}
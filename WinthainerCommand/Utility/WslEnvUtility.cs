using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WinthainerCommand.Utility
{
    /// <summary>
    /// Often docker commands depend on the configured environment while execution.
    /// An example is a docker-compose.yml with ports defined through environment variables.
    ///
    /// Therefore the environment needs to be put in the WSL-Context. The WSL provides a solution for this with the host-environment variable "WSLENV".
    /// Description of this feature in the microsoft blog: https://devblogs.microsoft.com/commandline/share-environment-vars-between-wsl-and-windows/
    ///
    /// Goal of this Utility is to fill nearly all environment-variable names in "WSLENV".
    /// Exceptions are the variables "WSLENV" and "PATH"
    /// </summary>
    public static class WslEnvUtility
    {
        /// <summary>
        /// Some environment-variables shouldn't be inserted into WSLENV.
        /// These are stored in this variable.
        ///
        /// All Values are lower case, because the check later expecting it.
        /// </summary>
        private static readonly string[] NotRequiredEnvKeys;

        static WslEnvUtility()
        {
            NotRequiredEnvKeys = new[] { "path", "wslenv" };
        }
        
        /// <summary>
        /// Initializes "WSLENV". More details are provided in class summary.
        /// </summary>
        public static void UpdateWslEnv()
        {
            var wslEnvContent = GenerateWslEnvStringFromCurrentEnvironment();
            SetWslEnvContent(wslEnvContent);
        }

        private static void SetWslEnvContent(string wslEnvContent)
        {
            Environment.SetEnvironmentVariable("WSLENV", wslEnvContent, EnvironmentVariableTarget.Process);
        }

        private static string GenerateWslEnvStringFromCurrentEnvironment()
        {
            var result = string.Empty;
            foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process))
            {
                if (ShouldEnvironmentVariableBePutInWslenv(environmentVariable))
                {
                    var wslEnvRepresentation = GetWslEnvRepresentation(environmentVariable);
                    result = $"{result}{wslEnvRepresentation}";
                }
            }
            if (result.Length > 0)
            {
                result = result.Remove(result.Length - 1, 1);
            }

            return result;
        }

        /// <summary>
        /// Formatting the environment-entry for the "WSLENV"-variable
        /// </summary>
        /// <param name="environmentVariable">checked environment variable</param>
        /// <returns>string in the format for WSLENV, with : at the end</returns>
        private static string GetWslEnvRepresentation(DictionaryEntry environmentVariable)
        {
            if (environmentVariable.Value != null)
            {
                var flags = GetFlagsForValue(environmentVariable.Value.ToString());
                return $"{environmentVariable.Key}{flags}:";
            }
            return "";
        }

        private static object GetFlagsForValue(string environmentVariableValue)
        {
            var flags = ""; //empty for now //slash needs to be added too
            return flags;
        }

        private static bool ShouldEnvironmentVariableBePutInWslenv(DictionaryEntry environmentVariable)
        {
            var environmentVariableKeyLower = environmentVariable.Key.ToString()?.ToLower();
            return NotRequiredEnvKeys.Contains(environmentVariableKeyLower) == false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using winsudo.Utilities;

namespace winsudo
{
    /// <summary>
    /// Provides several runtime checks.
    /// </summary>
    public class RuntimeChecks
    {
        /// <summary>
        /// Checks if help information should be printed.
        /// </summary>
        /// <param name="commandLineArgs"></param>
        /// <returns></returns>
        public static bool CheckHelp(string[] commandLineArgs)
        {
            if (commandLineArgs.Length >= 2)
            {
                if (commandLineArgs[1] == "-h" || commandLineArgs[1] == "--help" || commandLineArgs[1] == "-?")
                {
                    ConsoleUtilities.HighlightConsole(() => Console.WriteLine($"{ApplicationInfo.Name} version {ApplicationInfo.Version}"));
                    Console.WriteLine();
                    Console.WriteLine("[su only]");
                    Console.WriteLine("Use the '-c' option to run the specified commands in a console window and close the console window.");
                    Console.WriteLine("Use the '-C' option to run the specified commands in a console window.");
                    Console.WriteLine();
                    Console.WriteLine($"The following options can be used in {ApplicationInfo.Name}:");
                    Console.WriteLine();
                    Settings.PrintSettings();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if an alias should be launched.
        /// </summary>
        /// <param name="commandLineArgs">The command line arguments.</param>
        public static bool CheckAliases(string[] commandLineArgs)
        {
            if (commandLineArgs.Length == 2)
            {
                Dictionary<string, string> aliases = Settings.GetAliases();
                if (aliases.ContainsKey(commandLineArgs[1]))
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(aliases[commandLineArgs[1]])
                    {
                        Verb = "runas"
                    };
                    ProcessUtilities.RunSafe(processStartInfo, false);

                    return true;
                }
            }
            return false;
        }
    }
}

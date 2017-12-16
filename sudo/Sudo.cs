using System;
using System.Collections.Generic;
using System.Diagnostics;
using winsudo.sudo.Utilities;

namespace winsudo.sudo
{
    /// <summary>
    /// Main class of the program.
    /// </summary>
    public class Sudo
    {
        /// <summary>
        /// Executes the progam.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            Debugging.PrintDebugBuild();

            /* In this array, the first item (at 0) will be the path to the current
             * executable. The second item (at 1) will be the executable to be
             * invoked. Further arguments are input to the other process.
             */
            string[] commandLineArgs = Environment.GetCommandLineArgs();

            if (CheckHelp(commandLineArgs)) Exit();
            if (CheckAliases(commandLineArgs)) return;

            ProcessStartInfo processStartInfo = ProcessUtilities.CreateProcessStartInfo(commandLineArgs);

            if (processStartInfo == null)
            {
                ConsoleUtilities.HighlightConsole(() => Console.WriteLine("Please specify the command to execute."));
                Console.WriteLine();
                Console.WriteLine("Usage: sudo <executable> <executable arguments>");
                Exit();
            }

            if (ProcessUtilities.HasOwnWindow())
            {
                Console.Title = $"{ApplicationInfo.Name} - {processStartInfo.FileName} {processStartInfo.Arguments}";
            }
            int? exitCode = ProcessUtilities.RunSafe(processStartInfo, true);
            Exit(exitCode);
        }

        /// <summary>
        /// Checks if help information should be printed.
        /// </summary>
        /// <param name="commandLineArgs"></param>
        /// <returns></returns>
        private static bool CheckHelp(string[] commandLineArgs)
        {
            if (commandLineArgs.Length >= 2)
            {
                if (commandLineArgs[1] == "-h" || commandLineArgs[1] == "--help")
                {
                    ConsoleUtilities.HighlightConsole(() => Console.WriteLine($"{ApplicationInfo.Name} version {ApplicationInfo.Version}"));
                    Console.WriteLine();
                    Console.WriteLine("The following options can be used in winsudo:");
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
        private static bool CheckAliases(string[] commandLineArgs)
        {
            if (commandLineArgs.Length == 2)
            {
                Dictionary<string, string> aliases = Settings.GetAliases();
                if (aliases.ContainsKey(commandLineArgs[1]))
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(aliases[commandLineArgs[1]]);
                    Process.Start(processStartInfo);

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Prints an exit message and exits.
        /// </summary>
        /// <param name="exitCode">The exit code of the administrator process. Leave null if none.</param>
        private static void Exit(int? exitCode = null)
        {
            if (ProcessUtilities.HasOwnWindow())
            {
                ConsoleUtilities.HighlightConsole(() =>
                {
                    Console.WriteLine();
                    if (exitCode.HasValue)
                    {
                        Console.WriteLine($"The administrator process has exited with exit code {exitCode}.");
                    }
                    Console.WriteLine("Press any key to continue.");
                });
                Console.ReadKey();
            }
            Environment.Exit(0);
        }
    }
}

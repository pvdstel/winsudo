using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using winsudo.Utilities;

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

            if (RuntimeChecks.CheckHelp(commandLineArgs)) Exit();
            if (RuntimeChecks.CheckAliases(commandLineArgs)) return;

            ProcessStartInfo processStartInfo = CreateProcessStartInfo(commandLineArgs);

            if (processStartInfo == null)
            {
                ConsoleUtilities.HighlightConsole(() => Console.WriteLine("Please specify the command to execute."));
                Console.WriteLine();
                Console.WriteLine("Usage: sudo <executable> [<executable arguments>]");
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
        /// Creates process start information.
        /// </summary>
        /// <param name="commandLineArgs">The command line arguments.</param>
        /// <returns>An instance of <see cref="ProcessStartInfo"/>.</returns>
        public static ProcessStartInfo CreateProcessStartInfo(string[] commandLineArgs)
        {
            if (commandLineArgs.Length < 2)
            {
                return null;
            }

            string executable = commandLineArgs[1];

            CommandEscaper commandEscaper = new CommandEscaper();
            IEnumerable<string> escapedArguments = commandLineArgs.Skip(2).Select(a => commandEscaper.Escape(a));
            string arguments = string.Join(" ", escapedArguments);

            ProcessStartInfo processStartInfo = new ProcessStartInfo(executable, arguments)
            {
                UseShellExecute = false
            };
            return processStartInfo;
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

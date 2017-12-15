using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sudo
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
            string[] command = Environment.GetCommandLineArgs();

            CheckAdminSession(command);

            ProcessStartInfo processStartInfo = CreateProcessStartInfo(command);

            if (processStartInfo == null)
            {
                Utilities.HighlightConsole(() => Console.WriteLine("Please specify the command to execute."));
                Exit();
            }

            Process run = Process.Start(processStartInfo);
            run.WaitForExit();
            Exit(run.ExitCode);
        }

        /// <summary>
        /// Creates process start information.
        /// </summary>
        /// <param name="commandLineArgs">The command line arguments.</param>
        /// <returns>An instance of <see cref="ProcessStartInfo"/>.</returns>
        private static ProcessStartInfo CreateProcessStartInfo(string[] commandLineArgs)
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
        /// Checks if an interactive terminal should be created.
        /// </summary>
        /// <param name="commandLineArgs">The command line arguments.</param>
        private static void CheckAdminSession(string[] commandLineArgs)
        {
            if (commandLineArgs.Length != 2)
            {
                return;
            }

            if (commandLineArgs[1].ToLower() == "su")
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd");
                processStartInfo.Verb = "runas";
                Process.Start(processStartInfo);

                Environment.Exit(0);
            }

            if (commandLineArgs[1].ToLower() == "ps")
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo("powershell");
                processStartInfo.Verb = "runas";
                Process.Start(processStartInfo);

                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Prints an exit message and exits.
        /// </summary>
        private static void Exit(int? exitCode = null)
        {
            if (Utilities.HasOwnWindow())
            {
                Utilities.HighlightConsole(() =>
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

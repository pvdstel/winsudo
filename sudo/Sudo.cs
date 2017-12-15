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

            ProcessStartInfo processStartInfo = CreateProcessStartInfo(command);

            if (processStartInfo == null)
            {
                Console.WriteLine("Please specify the command to execute.");
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
        /// Prints an exit message and exits.
        /// </summary>
        private static void Exit(int? exitCode = null)
        {
            if (Utilities.HasOwnWindow())
            {
                Utilities.HighlightConsole(() =>
                {
                    Console.WriteLine();
                    Console.Write("The administrator process has ended");
                    if (exitCode.HasValue)
                    {
                        Console.Write($" with exit code {exitCode}");
                    }
                    Console.WriteLine(".");
                    Console.WriteLine("Press any key to continue.");
                });
                Console.ReadKey();
            }
            Environment.Exit(0);
        }
    }
}

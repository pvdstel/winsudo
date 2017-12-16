using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static sudo.Utilities.ConsoleUtilities;

namespace sudo.Utilities
{
    /// <summary>
    /// Provides process utility functions.
    /// </summary>
    internal class ProcessUtilities
    {
        /// <summary>
        /// Determines whether the current process has its own window.
        /// </summary>
        /// <returns>A <see cref="bool"/> value indicating whether the current process ahs its own window.</returns>
        internal static bool HasOwnWindow()
        {
            return Process.GetCurrentProcess().MainWindowHandle != IntPtr.Zero;
        }

        /// <summary>
        /// Runs a process safely, catching all possible errors that might occur.
        /// </summary>
        /// <param name="processStartInfo">The instance of <see cref="ProcessStartInfo"/> to use for launching the process.</param>
        /// <param name="wait">Whether the execution should block.</param>
        /// <returns>An exit code if tehre was any, or null otherwise.</returns>
        internal static int? RunSafe(ProcessStartInfo processStartInfo, bool wait)
        {
            try
            {
                Process run = Process.Start(processStartInfo);
                if (wait)
                {
                    run.WaitForExit();
                    return run.ExitCode;
                }
                return null;
            }
            catch (FileNotFoundException)
            {
                ErrorConsole(() => Console.WriteLine("The specified executable could not be found."));
            }
            catch (Win32Exception ex)
            {
                ErrorConsole(() => Console.WriteLine("There was an error executing the specified command:"));
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentNullException)
            {
                ErrorConsole(() => Console.WriteLine("An internal error occured. This exception should never be thrown."));
            }
            catch (InvalidOperationException)
            {
                ErrorConsole(() => Console.WriteLine("An internal error occured."));
            }
            return null;
        }

        /// <summary>
        /// Creates process start information.
        /// </summary>
        /// <param name="commandLineArgs">The command line arguments.</param>
        /// <returns>An instance of <see cref="ProcessStartInfo"/>.</returns>
        internal static ProcessStartInfo CreateProcessStartInfo(string[] commandLineArgs)
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
    }
}

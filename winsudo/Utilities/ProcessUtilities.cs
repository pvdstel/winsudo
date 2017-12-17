using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static winsudo.Utilities.ConsoleUtilities;

namespace winsudo.Utilities
{
    /// <summary>
    /// Provides process utility functions.
    /// </summary>
    public class ProcessUtilities
    {
        /// <summary>
        /// Determines whether the current process has its own window.
        /// </summary>
        /// <returns>A <see cref="bool"/> value indicating whether the current process ahs its own window.</returns>
        public static bool HasOwnWindow()
        {
            return Process.GetCurrentProcess().MainWindowHandle != IntPtr.Zero;
        }

        /// <summary>
        /// Runs a process safely, catching all possible errors that might occur.
        /// </summary>
        /// <param name="processStartInfo">The instance of <see cref="ProcessStartInfo"/> to use for launching the process.</param>
        /// <param name="wait">Whether the execution should block.</param>
        /// <returns>An exit code if tehre was any, or null otherwise.</returns>
        public static int? RunSafe(ProcessStartInfo processStartInfo, bool wait)
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
    }
}

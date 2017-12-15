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
            return -1;
        }
    }
}

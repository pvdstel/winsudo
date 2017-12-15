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
            /* In this array, the first item (at 0) will be the path to the current
             * executable. The second item (at 1) will be the executable to be
             * invoked. Further arguments are input to the other process.
             */
            string[] command = Environment.GetCommandLineArgs();

            if (command.Length < 2)
            {
                Console.WriteLine("Please specify the command to execute.");
                return;
            }

            string executable = command[1];

            CommandEscaper commandEscaper = new CommandEscaper();
            IEnumerable<string> escapedArguments = command.Skip(2).Select(a => commandEscaper.Escape(a));
            string arguments = string.Join(" ", escapedArguments);

            ProcessStartInfo processStartInfo = new ProcessStartInfo(executable, arguments)
            {
                UseShellExecute = false
            };
            Process.Start(processStartInfo);
        }
    }
}

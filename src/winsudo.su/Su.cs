using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using winsudo;
using winsudo.Utilities;

namespace winsudo.su
{
    /// <summary>
    /// Main class of the program.
    /// </summary>
    public class Su
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

            if (RuntimeChecks.CheckHelp(commandLineArgs)) return;
            if (RuntimeChecks.CheckAliases(commandLineArgs)) return;

            ProcessStartInfo processStartInfo;
            if (commandLineArgs.Length < 2)
            {
                processStartInfo = new ProcessStartInfo(Settings.GetDefaultExecutable())
                {
                    UseShellExecute = true,
                    Verb = "runas"
                };
            }
            else if (commandLineArgs[1] == "-c" || commandLineArgs[1] == "-C")
            {
                bool leaveOpen = commandLineArgs[1] == "-C";

                CommandEscaper commandEscaper = new CommandEscaper();
                IEnumerable<string> escapedArguments = commandLineArgs.Skip(2).Select(a => commandEscaper.Escape(a));
                string passedArguments = string.Join(" ", escapedArguments);

                processStartInfo = new ProcessStartInfo("cmd", $"/{(leaveOpen ? "K" : "C")} {passedArguments}")
                {
                    UseShellExecute = true,
                    Verb = "runas"
                };
            }
            else
            {
                string executable = commandLineArgs[1];

                CommandEscaper commandEscaper = new CommandEscaper();
                IEnumerable<string> escapedArguments = commandLineArgs.Skip(2).Select(a => commandEscaper.Escape(a));
                string arguments = string.Join(" ", escapedArguments);

                processStartInfo = new ProcessStartInfo(executable, arguments)
                {
                    UseShellExecute = true,
                    Verb = "runas"
                };
            }

            ProcessUtilities.RunSafe(processStartInfo, false);
        }
    }
}

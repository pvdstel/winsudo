using System;
using System.Diagnostics;
using winsudo.Utilities;

namespace winsudo
{
    /// <summary>
    /// Provides debugging utilities.
    /// </summary>
    public class Debugging
    {
        /// <summary>
        /// Informs the user that the current build is a release build.
        /// </summary>
        [Conditional("DEBUG")]
        public static void PrintDebugBuild()
        {
            ConsoleUtilities.HighlightConsole(() =>
            {
                Console.WriteLine("This is a debug build of winsudo.");
                Console.WriteLine(new String('-', 33));
                Console.WriteLine();
            });
        }
    }
}

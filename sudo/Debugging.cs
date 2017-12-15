using System;
using System.Diagnostics;

namespace sudo
{
    /// <summary>
    /// Provides debugging utilities.
    /// </summary>
    class Debugging
    {
        /// <summary>
        /// Informs the user that the current build is a release build.
        /// </summary>
        [Conditional("DEBUG")]
        internal static void PrintDebugBuild()
        {
            Utilities.HighlightConsole(() =>
            {
                Console.WriteLine("This is a debug build of winsudo.");
                Console.WriteLine(new String('-', 33));
                Console.WriteLine();
            });
        }
    }
}

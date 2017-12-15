using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudo
{
    /// <summary>
    /// Provides utility functions.
    /// </summary>
    internal static class Utilities
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
        /// Highlights console output colors.
        /// </summary>
        internal static void HighlightConsoleColors()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Restores console output colors.
        /// </summary>
        internal static void RestoreConsoleColors()
        {
            Console.ResetColor();
        }

        /// <summary>
        /// Creates a context in which console colors are highlighted.
        /// </summary>
        /// <param name="contextCallback">The callback to be executed in the highlighted context.</param>
        internal static void HighlightConsole(Action contextCallback)
        {
            HighlightConsoleColors();
            contextCallback?.Invoke();
            RestoreConsoleColors();
        }
    }
}

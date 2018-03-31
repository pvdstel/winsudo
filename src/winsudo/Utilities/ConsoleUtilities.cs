using System;

namespace winsudo.Utilities
{
    /// <summary>
    /// Provides console utilities.
    /// </summary>
    public class ConsoleUtilities
    {
        /// <summary>
        /// Highlights console output colors.
        /// </summary>
        private static void HighlightConsoleColors()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Highlights console output colors for errors.
        /// </summary>
        private static void ErrorConsoleColors()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Restores console output colors.
        /// </summary>
        public static void RestoreConsoleColors()
        {
            Console.ResetColor();
        }

        /// <summary>
        /// Creates a context in which console colors are highlighted.
        /// </summary>
        /// <param name="contextCallback">The callback to be executed in the highlighted context.</param>
        public static void HighlightConsole(Action contextCallback)
        {
            HighlightConsoleColors();
            contextCallback?.Invoke();
            RestoreConsoleColors();
        }

        /// <summary>
        /// Creates a context in which console colors are highlighted for errors.
        /// </summary>
        /// <param name="contextCallback">The callback to be executed in the highlighted context.</param>
        public static void ErrorConsole(Action contextCallback)
        {
            ErrorConsoleColors();
            contextCallback?.Invoke();
            RestoreConsoleColors();
        }
    }
}

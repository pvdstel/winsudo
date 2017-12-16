using System;
using System.Text;
using System.Text.RegularExpressions;

namespace winsudo.sudo
{
    /// <summary>
    /// Escapes commands and arguments.
    /// 
    /// Modified from http://csharptest.net/529/how-to-correctly-escape-command-line-arguments-in-c/index.html.
    /// </summary>
    public class CommandEscaper
    {
        /// <summary>
        /// Contains characters that cannot be escaped.
        /// </summary>
        private static readonly Regex invalid = new Regex("[\x00\x0a\x0d]");
        /// <summary>
        /// Contains whitespace or two quote characters.
        /// </summary>
        private static readonly Regex needsQuotes = new Regex(@"\s|""");
        /// <summary>
        /// Contains one or more '\' followed with a quote or end of string.
        /// </summary>
        private static readonly Regex escapeQuotes = new Regex(@"(\\*)(""|$)");

        /// <summary>
        /// Quotes arguments that contain whitespace, or begin with a quote and returns a single
        /// argument string for use with Process.Start().
        /// </summary>
        /// <param name="argument">The argument to escape. May not contain null, '\0', '\r', or '\n'.</param>
        /// <returns>A <see cref="string"/> containing the escaped argument.</returns>
        /// <exception cref="ArgumentNullException">Raised the argument is null.</exception>
        /// <exception cref="InvalidOperationException">Raised if the argument contains '\0', '\r', or '\n'.</exception>
        public string Escape(string argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(nameof(argument));
            }

            if (IsInvalid(argument))
            {
                throw new InvalidOperationException("This argument contains invalid characters.");
            }

            StringBuilder escaped = new StringBuilder();
            if (argument == string.Empty)
            {
                escaped.Append("\"\"");
            }
            else if (!NeedsQuotes(argument))
            {
                escaped.Append(argument);
            }
            else
            {
                escaped.Append('"');
                escaped.Append(EscapeQuotes(argument));
                escaped.Append('"');
            }
            return escaped.ToString();
        }

        /// <summary>
        /// Checks if the given argument is invalid.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <returns>A <see cref="bool"/> indicating whether the argument is valid.</returns>
        private bool IsInvalid(string argument) => invalid.IsMatch(argument);

        /// <summary>
        /// Checks if the given argument needs quotes.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <returns>A <see cref="bool"/> indicating whether the argument needs quotes.</returns>
        private bool NeedsQuotes(string argument) => needsQuotes.IsMatch(argument);

        /// <summary>
        /// Escapes an argument.
        /// </summary>
        /// <param name="argument">The argument to escape.</param>
        /// <returns>A <see cref="string"/> containing the escaped argument.</returns>
        private string EscapeQuotes(string argument)
        {
            return escapeQuotes.Replace(argument, eval =>
            {
                bool hasQuotes = eval.Groups[2].Value == "\"";

                string backslashes = eval.Groups[1].Value;
                string quotes = hasQuotes ? "\\\"" : "";

                return backslashes + backslashes + quotes;
            });
        }
    }
}

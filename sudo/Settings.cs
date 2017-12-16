using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sudo
{
    /// <summary>
    /// Represents settings.
    /// </summary>
    public static class Settings
    {
        private static Regex iniRegex = new Regex(@"^\s*(\w+)\s*=\s*(\w+)\s*$");

        /// <summary>
        /// The path used for the alias config file.
        /// </summary>
        private static readonly string AliasPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationInfo.Name, "aliases");

        /// <summary>
        /// Default aliases.
        /// </summary>
        public static ReadOnlyDictionary<string, string> DefaultAliases;

        /// <summary>
        /// Initializes static members of <see cref="Settings"/> whose initialization code is
        /// too long to comfortably fit on one line.
        /// </summary>
        static Settings()
        {
            Dictionary<string, string> defaultAliases = new Dictionary<string, string>()
            {
                { "su", "cmd" },
                { "-", "powershell" }
            };
            DefaultAliases = new ReadOnlyDictionary<string, string>(defaultAliases);
        }

        /// <summary>
        /// Prints the available settings to the console.
        /// </summary>
        public static void PrintSettings()
        {
            Console.WriteLine($"File used for aliases:\n{AliasPath}");
        }

        /// <summary>
        /// Gets aliases.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> of aliases.</returns>
        public static Dictionary<string, string> GetAliases()
        {
            Dictionary<string, string> aliasFile = new Dictionary<string, string>(DefaultAliases);
            if (File.Exists(AliasPath))
            {
                using (StreamReader read = new StreamReader(AliasPath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        Match lineMatch = iniRegex.Match(line);
                        if (lineMatch.Success)
                        {
                            aliasFile[lineMatch.Groups[1].Value] = lineMatch.Groups[2].Value;
                        }
                    }
                }
            }
            return aliasFile;
        }
    }
}

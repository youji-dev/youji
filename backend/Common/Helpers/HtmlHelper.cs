using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Helpers
{
    /// <summary>
    /// Provides helper methods for html strings
    /// </summary>
    public static partial class HtmlHelper
    {
        /// <summary>
        /// Convert an html string to plain text
        /// </summary>
        /// <param name="html">The input html string</param>
        /// <returns>The plain text result</returns>
        public static string HtmlToPlainText(string html)
        {
            string result = (string)html.Clone();
            result = StyleTagRegex().Replace(html, string.Empty);
            result = IndentTagsRegex().Replace(result, "\t");

            result = GenericTagRegex().Replace(result, string.Empty);
            result = result.Trim();

            return result;
        }

        /// <summary>
        /// Breaks all lines longer than the given line length
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="lineLenght">The maximum line length</param>
        /// <returns>The string limited to the max line length</returns>
        public static string LimitLineLength(string input, int lineLenght)
        {
            string[] lines = input.Split(Environment.NewLine);

            StringBuilder limitedString = new();

            foreach (string line in lines)
            {
                string lineCopy = (string)line.Clone();
                string indentation = IndentationRegex().Match(lineCopy).ToString();
                do
                {
                    int charactersInLine = Math.Min(lineCopy.Length, lineLenght);
                    string shortenedLine = string.Join(string.Empty, lineCopy.Take(charactersInLine)).Trim();

                    limitedString.AppendLine($"{indentation}{shortenedLine}");
                    lineCopy = lineCopy.Remove(0, charactersInLine);
                }
                while (lineCopy.Length > 0);
            }

            return limitedString.ToString();
        }
    }

    /// <summary>
    /// Partial class for Regex
    /// </summary>
    public static partial class HtmlHelper
    {
        [GeneratedRegex(@"<style>(.[^/]|\s)*</style>")]
        private static partial Regex StyleTagRegex();

        [GeneratedRegex(@"</{0,1}[^<>]*>")]
        private static partial Regex GenericTagRegex();

        [GeneratedRegex(@"<(p|li)>")]
        private static partial Regex IndentTagsRegex();

        [GeneratedRegex(@"^\s")]
        private static partial Regex IndentationRegex();
    }
}

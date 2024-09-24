using System.Reflection;
using System.Text.RegularExpressions;
using Common.Extensions;

namespace DomainLayer.BusinessLogic.Mailing
{
    /// <summary>
    /// Helper class for razor templates
    /// </summary>
    internal static partial class TemplateHelper
    {
        /// <summary>
        /// Get the template with the given name
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        /// <returns>The template as a plain string or null if it could not be found</returns>
        public static string? GetTemplate(string templateName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly()
                ?? throw new InvalidOperationException("Could not get assembly while loading email template");

            templateName = templateName.EndsWith(".cshtml") ? templateName : $"{templateName}.cshtml";

            using Stream? stream = assembly.GetResource(templateName);
            if (stream is null)
                return null;

            using StreamReader sr = new(stream);
            string html = sr.ReadToEnd();

            html = RazorDeclaresRegex().Replace(html, string.Empty);
            return html;
        }
    }

    /// <summary>
    /// Partial class for Regex
    /// </summary>
    internal static partial class TemplateHelper
    {
        [GeneratedRegex(@"^[^<]*")]
        private static partial Regex RazorDeclaresRegex();
    }
}

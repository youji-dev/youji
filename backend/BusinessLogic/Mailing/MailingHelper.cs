using System.Reflection;
using System.Text.RegularExpressions;
using Common.Extensions;
using DomainLayer.BusinessLogic.Mailing.Models;
using MimeKit.Utils;
using MimeKit;

namespace DomainLayer.BusinessLogic.Mailing
{
    /// <summary>
    /// Helper class for razor templates
    /// </summary>
    internal static partial class MailingHelper
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

        /// <summary>
        /// Adds common resources (icon's, ...) to the <paramref name="bodyBuilder"/> and makes them available for templates
        /// </summary>
        /// <param name="bodyBuilder">The mail body builder to add resources to</param>
        /// <param name="mailModel">The mail model to provide resources in</param>
        public static void AddResourcesToModel(BodyBuilder bodyBuilder, MailModel mailModel)
        {
            Assembly assembly = Assembly.GetExecutingAssembly()
                ?? throw new InvalidOperationException("Could not get assembly while loading logo file");

            var logo = bodyBuilder.LinkedResources.Add(
                "Logo.svg",
                assembly.GetResource("Logo.svg"));
            logo.ContentId = MimeUtils.GenerateMessageId();

            var arrowRight = bodyBuilder.LinkedResources.Add(
                "arrow_right.svg",
                assembly.GetResource("arrow_right.svg"));
            arrowRight.ContentId = MimeUtils.GenerateMessageId();

            var arrowDown = bodyBuilder.LinkedResources.Add(
                "arrow_down.svg",
                assembly.GetResource("arrow_down.svg"));
            arrowDown.ContentId = MimeUtils.GenerateMessageId();

            mailModel.LogoSrc = $"cid:{logo.ContentId}";
            mailModel.ArrowRightIconSrc = $"cid:{arrowRight.ContentId}";
            mailModel.ArrowDownIconSrc = $"cid:{arrowDown.ContentId}";
        }
    }

    /// <summary>
    /// Partial class for Regex
    /// </summary>
    internal static partial class MailingHelper
    {
        [GeneratedRegex(@"^.*<!-- Begin document -->\s*", RegexOptions.Singleline)]
        private static partial Regex RazorDeclaresRegex();
    }
}

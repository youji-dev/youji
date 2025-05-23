﻿using System.Reflection;
using System.Text.RegularExpressions;
using Common.Extensions;
using DomainLayer.BusinessLogic.Mailing.Models;
using MimeKit.Utils;
using MimeKit;
using I18N.DotNet;
using PersistenceLayer.DataAccess.Entities;
using Common.Helpers;
using RazorEngine;
using RazorEngine.Templating;

namespace DomainLayer.BusinessLogic.Mailing
{
    /// <summary>
    /// Helper class for mail body generation
    /// </summary>
    public static partial class MailGenerator
    {
        /// <summary>
        /// Generate a mail body for a changed ticket
        /// </summary>
        /// <param name="newTicket">New version of the ticket</param>
        /// <param name="oldTicket">Old version of the ticket</param>
        /// <param name="localizer">Localizer for mail generation</param>
        /// <param name="mailGenConfiguration">Configuration values used during mail generation</param>
        /// <returns>The generated mail body</returns>
        public static MimeEntity GenerateTicketChangedMail(Ticket newTicket, Ticket oldTicket, Localizer localizer, MailGenConfigurationDto mailGenConfiguration)
        {
            TicketDataChangedModel mailModel = TicketDataChangedModel.FromTickets(newTicket, oldTicket, localizer);

            return GenerateMail(mailModel, mailGenConfiguration);
        }

        /// <summary>
        /// Generate a mail body for a new attachment
        /// </summary>
        /// <param name="newAttachment">The new attachment</param>
        /// <param name="localizer">Localizer for mail generation</param>
        /// <param name="mailGenConfiguration">Configuration values used during mail generation</param>
        /// <returns>The generated mail body</returns>
        public static MimeEntity GenerateNewTicketAttachmentMail(TicketAttachment newAttachment, Localizer localizer, MailGenConfigurationDto mailGenConfiguration)
        {
            NewTicketAttachmentModel mailModel = NewTicketAttachmentModel.FromAttachment(newAttachment, localizer);

            return GenerateMail(mailModel, mailGenConfiguration);
        }

        /// <summary>
        /// Generate a mail body for a new comment
        /// </summary>
        /// <param name="newComment">The new comment</param>
        /// <param name="localizer">Localizer for mail generation</param>
        /// <param name="mailGenConfiguration">Configuration values used during mail generation</param>
        /// <returns>The generated mail body</returns>
        public static MimeEntity GenerateNewTicketCommentMail(TicketComment newComment, Localizer localizer, MailGenConfigurationDto mailGenConfiguration)
        {
            NewTicketCommentModel mailModel = NewTicketCommentModel.FromComment(newComment, localizer);

            return GenerateMail(mailModel, mailGenConfiguration);
        }

        /// <summary>
        /// Generate a mail body for a new ticket
        /// </summary>
        /// <param name="newTicket">The new ticket</param>
        /// <param name="localizer">Localizer for mail generation</param>
        /// <param name="mailGenConfiguration">Configuration values used during mail generation</param>
        /// <returns>The generated mail body</returns>
        public static MimeEntity GenerateNewTicketMail(Ticket newTicket, Localizer localizer, MailGenConfigurationDto mailGenConfiguration)
        {
            NewTicketModel mailModel = NewTicketModel.FromTicket(newTicket, localizer);
            return GenerateMail(mailModel, mailGenConfiguration);
        }

        /// <summary>
        /// Generate the mail body for a model
        /// </summary>
        /// <param name="mailModel">The model to use as a data provider</param>
        /// <returns>The generated mail body</returns>
        private static MimeEntity GenerateMail(MailModel mailModel, MailGenConfigurationDto mailGenConfiguration)
        {
            BodyBuilder bodyBuilder = new();

            AddResourcesToModel(bodyBuilder, mailModel);
            AddMiscellaniousValuesToModel(mailModel, mailGenConfiguration);

            string layout = GetTemplate("MailBase")
                ?? throw new InvalidOperationException("Could not find resource file for mail layout");

            string template = GetTemplate(mailModel.TemplateName)
                ?? throw new InvalidOperationException($"Could not find resource file for mail template with name '{mailModel.TemplateName}'");

            Engine.Razor.AddTemplate("mailLayout", layout);
            var html = Engine.Razor.RunCompile(template, mailModel.TemplateName, mailModel.GetType(), mailModel);

            bodyBuilder.HtmlBody = html;
            bodyBuilder.TextBody = HtmlHelper.LimitLineLength(HtmlHelper.HtmlToPlainText(html), 80);

            return bodyBuilder.ToMessageBody();
        }

        /// <summary>
        /// Get the template with the given name
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        /// <returns>The template as a plain string or null if it could not be found</returns>
        private static string? GetTemplate(string templateName)
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
        private static void AddResourcesToModel(BodyBuilder bodyBuilder, MailModel mailModel)
        {
            Assembly assembly = Assembly.GetExecutingAssembly()
                ?? throw new InvalidOperationException("Could not get assembly while loading logo file");

            var logo = bodyBuilder.LinkedResources.Add(
                "Logo.svg",
                assembly.GetResource("Logo.svg"));
            logo.ContentId = MimeUtils.GenerateMessageId();

            mailModel.AutoValues.LogoSrc = $"cid:{logo.ContentId}";

            if (mailModel.AutoValues.ArrowRightIconSrc is null)
            {
                var arrowRight = bodyBuilder.LinkedResources.Add(
                    "arrow_right.svg",
                    assembly.GetResource("arrow_right.svg"));
                arrowRight.ContentId = MimeUtils.GenerateMessageId();

                mailModel.AutoValues.ArrowRightIconSrc = $"cid:{arrowRight.ContentId}";
            }

            if (mailModel.AutoValues.ArrowDownIconSrc is null)
            {
                var arrowDown = bodyBuilder.LinkedResources.Add(
                    "arrow_down.svg",
                    assembly.GetResource("arrow_down.svg"));
                arrowDown.ContentId = MimeUtils.GenerateMessageId();

                mailModel.AutoValues.ArrowDownIconSrc = $"cid:{arrowDown.ContentId}";
            }
        }

        private static void AddMiscellaniousValuesToModel(MailModel mailModel, MailGenConfigurationDto mailGenConfiguration)
        {
            if (mailModel.RelatedTicketId is Guid relatedTicketId)
            {
                mailModel.AutoValues.GoToHyperref = Path.Combine(mailGenConfiguration.FrontendTicketBaseUrl, relatedTicketId.ToString());
            }
        }
    }

    /// <summary>
    /// Partial class for Regex
    /// </summary>
    public static partial class MailGenerator
    {
        [GeneratedRegex(@"^.*<!-- Begin document -->\s*", RegexOptions.Singleline)]
        private static partial Regex RazorDeclaresRegex();
    }
}

using System.Globalization;
using System.Reflection;
using System.Text;
using Common.Extensions;
using Common.Helpers;
using DomainLayer.BusinessLogic.Mailing.Models;
using I18N.DotNet;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using PersistenceLayer.DataAccess.Entities;
using RazorEngine;
using RazorEngine.Templating;

namespace DomainLayer.BusinessLogic.Mailing
{
    /// <summary>
    /// Provides methods for mailing
    /// </summary>
    public class MailingService(IConfiguration configuration)
    {
        private readonly string mailSenderName = configuration.GetValueOrThrow("SenderName", ["Mail"]);
        private readonly string mailSenderAddress = configuration.GetValueOrThrow("SenderAddress", ["Mail"]);
        private readonly string mailServerAddress = configuration.GetValueOrThrow("SmtpAddress", ["Mail"]);
        private readonly int mailServerPort = int.Parse(configuration.GetValueOrThrow("SmtpPort", ["Mail"]));
        private readonly bool useSsl = bool.Parse(configuration.GetValueOrThrow("UseSsl", ["Mail"]));
        private readonly CompositeFormat mailSubjectFormat = CompositeFormat.Parse(
            configuration.GetValueOrThrow("SubjectFormat", ["Mail"]));

        /// <summary>
        /// Generate a mail body for a changed ticket
        /// </summary>
        /// <param name="newTicket">New version of the ticket</param>
        /// <param name="oldTicket">Old version of the ticket</param>
        /// <param name="localizer">Localizer for mail generation</param>
        /// <returns>The generated mail body</returns>
        public MimeEntity GenerateTicketChangedMail(Ticket newTicket, Ticket oldTicket, Localizer localizer)
        {
            TicketDataChangedModel mailModel = TicketDataChangedModel.FromTickets(newTicket, oldTicket, localizer);

            return this.GenerateMail(mailModel);
        }

        /// <summary>
        /// Generate a mail body for a new attachement
        /// </summary>
        /// <param name="newAttachment">The new attachment</param>
        /// <param name="localizer">Localizer for mail generation</param>
        /// <returns>The generated mail body</returns>
        public MimeEntity GenerateNewTicketAttachmentMail(TicketAttachment newAttachment, Localizer localizer)
        {
            NewTicketAttachmentModel mailModel = NewTicketAttachmentModel.FromAttachment(newAttachment, localizer);

            return this.GenerateMail(mailModel);
        }

        /// <summary>
        /// Generate a mail body for a new comment
        /// </summary>
        /// <param name="newComment">The new comment</param>
        /// <param name="localizer">Localizer for mail generation</param>
        /// <returns>The generated mail body</returns>
        public MimeEntity GenerateNewTicketCommentMail(TicketComment newComment, Localizer localizer)
        {
            NewTicketCommentModel mailModel = NewTicketCommentModel.FromComment(newComment, localizer);

            return this.GenerateMail(mailModel);
        }

        /// <summary>
        /// Send same mail to many recipients with recipient-specific localization
        /// </summary>
        /// <param name="recipients">The recpients of the mail</param>
        /// <param name="mailGenerator">A generator function that produces the mail body</param>
        /// <param name="subjectGenerator">A generator function that produces the mail subject</param>
        /// <returns>A Task representing the asynchronous operation</returns>
        public async Task SendManyLocalized(
            IEnumerable<MailRecipient> recipients,
            Func<Localizer, MimeEntity> mailGenerator,
            Func<Localizer, string> subjectGenerator)
        {
            using SmtpClient client = new();
            await client.ConnectAsync(this.mailServerAddress, this.mailServerPort, this.useSsl);

            Localizer localizer = new();
            using var localizerResourceStream = Assembly.GetExecutingAssembly().GetResource("Mailing.I18N.xml");

            var senderMail = new MailboxAddress(this.mailSenderName, this.mailSenderAddress);
            foreach (var recipient in recipients)
            {
                string language = recipient.PreferredLcid ?? "en-EN";

                if (localizerResourceStream is not null)
                    localizer.LoadXML(localizerResourceStream, CultureInfo.GetCultureInfo(language));

                MimeMessage message = new();
                message.From.Add(senderMail);
                message.To.Add(recipient.Address);
                message.Subject = string.Format(CultureInfo.InvariantCulture, this.mailSubjectFormat, subjectGenerator(localizer));

                var body = mailGenerator(localizer);
                message.Body = body;

                await client.SendAsync(message);
            }

            await client.DisconnectAsync(true);
        }

        /// <summary>
        /// Generate a mail body for the given <paramref name="mailTemplate"/> using the <paramref name="mailModel"/>
        /// </summary>
        /// <param name="mailModel">The model to use as a data provider</param>
        /// <returns>The generated mail body</returns>
        private MimeEntity GenerateMail(MailModel mailModel)
        {
            BodyBuilder bodyBuilder = new();

            MailingHelper.AddResourcesToModel(bodyBuilder, mailModel);

            string layout = MailingHelper.GetTemplate("MailBase")
                ?? throw new InvalidOperationException("Could not find resource file for mail layout");

            string template = MailingHelper.GetTemplate(mailModel.TemplateName)
                ?? throw new InvalidOperationException($"Could not find resource file for mail template with name '{mailModel.TemplateName}'");

            Engine.Razor.AddTemplate("mailLayout", layout);
            var html = Engine.Razor.RunCompile(template, mailModel.TemplateName, mailModel.GetType(), mailModel);

            bodyBuilder.HtmlBody = html;
            bodyBuilder.TextBody = HtmlHelper.LimitLineLength(HtmlHelper.HtmlToPlainText(html), 80);

            return bodyBuilder.ToMessageBody();
        }
    }
}

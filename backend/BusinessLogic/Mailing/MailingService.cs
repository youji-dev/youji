using System.Globalization;
using System.Text;
using Common.Extensions;
using Common.Helpers;
using DomainLayer.BusinessLogic.Mailing.Models;
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
        /// <returns>The generated mail body</returns>
        public MimeEntity GenerateTicketChangedMail(Ticket newTicket, Ticket oldTicket)
        {
            TicketDataChangedModel mailModel = TicketDataChangedModel.FromTickets(newTicket, oldTicket);

            return this.GenerateMail(mailModel);
        }

        /// <summary>
        /// Generate a mail body for a new attachement
        /// </summary>
        /// <param name="newAttachment">The new attachment</param>
        /// <returns>The generated mail body</returns>
        public MimeEntity GenerateNewTicketAttachmentMail(TicketAttachment newAttachment)
        {
            NewTicketAttachmentModel mailModel = NewTicketAttachmentModel.FromAttachment(newAttachment);

            return this.GenerateMail(mailModel);
        }

        /// <summary>
        /// Generate a mail body for a new comment
        /// </summary>
        /// <param name="newComment">The new comment</param>
        /// <returns>The generated mail body</returns>
        public MimeEntity GenerateNewTicketCommentMail(TicketComment newComment)
        {
            NewTicketCommentModel mailModel = NewTicketCommentModel.FromComment(newComment);

            return this.GenerateMail(mailModel);
        }

        /// <summary>
        /// Send a mail
        /// </summary>
        /// <param name="recipient">The mail recipient</param>
        /// <param name="subject">The mail subject</param>
        /// <param name="body">The mail body</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task Send(MailboxAddress recipient, string subject, MimeEntity body)
        {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress(this.mailSenderName, this.mailSenderAddress));
            message.To.Add(recipient);
            message.Subject = subject;

            message.Body = body;

            using SmtpClient client = new();

            await client.ConnectAsync(this.mailServerAddress, this.mailServerPort, this.useSsl);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        /// <summary>
        /// Send same mail to many recipients
        /// </summary>
        /// <param name="recipients">List of all recipients</param>
        /// <param name="subject">The mail subject</param>
        /// <param name="body">The mail body</param>
        /// <returns>A Task representing the asynchronous operation</returns>
        public async Task SendMany(IEnumerable<MailboxAddress> recipients, string subject, MimeEntity body)
        {
            using SmtpClient client = new();
            await client.ConnectAsync(this.mailServerAddress, this.mailServerPort, this.useSsl);

            foreach (var recipient in recipients)
            {
                MimeMessage message = new();
                message.From.Add(new MailboxAddress(this.mailSenderName, this.mailSenderAddress));
                message.To.Add(recipient);
                message.Subject = subject;

                message.Body = body;

                await client.SendAsync(message);
            }

            await client.DisconnectAsync(true);
        }

        /// <summary>
        /// Format a string to conform to shared subject format
        /// </summary>
        /// <param name="subjectText">The text the subject should include</param>
        /// <returns>The formatted subject</returns>
        public string FormatMailSubject(string subjectText)
            => string.Format(CultureInfo.InvariantCulture, this.mailSubjectFormat, subjectText);

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

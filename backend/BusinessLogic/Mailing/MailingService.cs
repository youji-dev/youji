using System.Reflection;
using Common.Extensions;
using Common.Helpers;
using DomainLayer.BusinessLogic.Mailing.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Utils;
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

        /// <summary>
        /// Generate a mail body for a changed ticket
        /// </summary>
        /// <param name="newTicket">New version of the ticket</param>
        /// <param name="oldTicket">Old version of the ticket</param>
        /// <returns>The generated mail body</returns>
        public MimeEntity GenerateTicketChangedMail(Ticket newTicket, Ticket oldTicket)
        {
            BodyBuilder bodyBuilder = new();

            TicketDataChangedModel mailModel = TicketDataChangedModel.FromTickets(newTicket, oldTicket);
            this.AddResourcesToModel(bodyBuilder, mailModel);

            string layout = TemplateHelper.GetTemplate("MailBase")
                ?? throw new InvalidOperationException("Could not find mail layout");

            string template = TemplateHelper.GetTemplate("TicketDataChanged")
                ?? throw new InvalidOperationException("Could not find template");

            Engine.Razor.AddTemplate("mailLayout", layout);
            var html = Engine.Razor.RunCompile(template, "template", typeof(TicketDataChangedModel), mailModel);

            bodyBuilder.HtmlBody = html;
            bodyBuilder.TextBody = HtmlHelper.LimitLineLength(HtmlHelper.HtmlToPlainText(html), 80);

            return bodyBuilder.ToMessageBody();
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

        private void AddResourcesToModel(BodyBuilder bodyBuilder, MailModel mailModel)
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
}

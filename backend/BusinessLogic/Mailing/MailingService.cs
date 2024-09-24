using Common.Extensions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using PersistenceLayer.DataAccess.Entities;

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
        public BodyBuilder GenerateTicketChangedMail(Ticket newTicket, Ticket oldTicket)
        {
            MailBodyBuilder builder = new($"Ticket '{newTicket.Title}' wurde geändert");

            if (newTicket.Title != oldTicket.Title)
            {
                builder.AddHeading("Titel geändert", 3);
                builder.AddParagraph($"{oldTicket.Title} -> {newTicket.Title}");
            }

            if (newTicket.Description != oldTicket.Description)
            {
                builder.AddHeading("Beschreibung geändert", 3);
                builder.AddCard([oldTicket.Description ?? "-", "->", newTicket.Description ?? "-"]);
            }

            if (newTicket.Priority != oldTicket.Priority)
            {
                builder.AddHeading("Priorität geändert", 3);
                builder.AddParagraph($"{oldTicket.Priority?.Name ?? "-"} -> {newTicket.Priority?.Name ?? "-"}");
            }

            if (newTicket.State != oldTicket.State)
            {
                builder.AddHeading("Status geändert", 3);
                builder.AddParagraph($"{oldTicket.State.Name} -> {newTicket.State.Name}");
            }

            if (newTicket.Building != oldTicket.Building)
            {
                builder.AddHeading("Gebäude geändert", 3);
                builder.AddParagraph($"{oldTicket.Building?.Name ?? "-"} -> {newTicket.Building?.Name ?? "-"}");
            }

            if (newTicket.Room != oldTicket.Room)
            {
                builder.AddHeading("Raum geändert", 3);
                builder.AddParagraph($"{oldTicket.Room ?? "-"} -> {newTicket.Room ?? "-"}");
            }

            if (newTicket.Object != oldTicket.Object)
            {
                builder.AddHeading("Betroffenes Objekt geändert");
                builder.AddParagraph($"{oldTicket.Object} -> {newTicket.Object}");
            }

            if (newTicket.Comments.Count > oldTicket.Comments.Count)
            {
                var newComments = newTicket.Comments.Skip(oldTicket.Comments.Count);

                foreach (var comment in newComments)
                {
                    builder.AddHeading($"Neuer Kommentar von '{comment.Author}'", 3);
                    builder.AddCard(comment.Content);
                }
            }

            if (newTicket.Attachments.Count != oldTicket.Attachments.Count)
            {
                var newAttachments = newTicket.Attachments.Skip(oldTicket.Attachments.Count);
                builder.AddHeading($"Neue Anhänge", 3);

                builder.AddUnorderedList(newAttachments.Select(attachment => $"{attachment.Name}"));
            }

            return builder.Complete();
        }

        /// <summary>
        /// Send a mail
        /// </summary>
        /// <param name="recipient">The mail recipient</param>
        /// <param name="subject">The mail subject</param>
        /// <param name="body">The mail body</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task Send(MailboxAddress recipient, string subject, BodyBuilder body)
        {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress(this.mailSenderName, this.mailSenderAddress));
            message.To.Add(recipient);
            message.Subject = subject;

            message.Body = body.ToMessageBody();

            using SmtpClient client = new();

            await client.ConnectAsync(this.mailServerAddress, this.mailServerPort, this.useSsl);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}

using System.Globalization;
using System.Reflection;
using System.Text;
using Common.Extensions;
using I18N.DotNet;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

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
    }
}

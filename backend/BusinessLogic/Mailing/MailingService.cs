using System.Globalization;
using System.Reflection;
using System.Text;
using Common.Extensions;
using I18N.DotNet;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace DomainLayer.BusinessLogic.Mailing
{
    /// <summary>
    /// Provides methods for mailing
    /// </summary>
    public class MailingService(IConfiguration configuration, ILogger<MailingService> logger)
    {
        private readonly string mailSenderName = configuration.GetValueOrThrow("SenderName", ["Mail"]);
        private readonly string mailSenderAddress = configuration.GetValueOrThrow("SenderAddress", ["Mail"]);
        private readonly string mailServerAddress = configuration.GetValueOrThrow("SmtpAddress", ["Mail"]);
        private readonly int mailServerPort = int.Parse(configuration.GetValueOrThrow("SmtpPort", ["Mail"]));
        private readonly string mailServerAuthenticationUsername = configuration.GetValueOrThrow("SmtpUser", ["Mail"]);
        private readonly string mailServerAuthenticationPassword = configuration.GetValueOrThrow("SmtpPassword", ["Mail"]);
        private readonly bool useSsl = bool.Parse(configuration.GetValueOrThrow("UseSsl", ["Mail"]));
        private readonly CompositeFormat mailSubjectFormat = CompositeFormat.Parse(
            configuration.GetValueOrThrow("SubjectFormat", ["Mail"]));

        private readonly MailGenConfigurationDto mailGenConfigurationDto = new(
            configuration.GetValueOrThrow("FrontendTicketBaseUri", ["Mail"]));

        /// <summary>
        /// Send same mail to many recipients with recipient-specific localization
        /// </summary>
        /// <param name="recipients">The recipients of the mail</param>
        /// <param name="mailGenerator">A generator function that produces the mail body</param>
        /// <param name="subjectGenerator">A generator function that produces the mail subject</param>
        /// <returns>A Task representing the asynchronous operation</returns>
        public async Task SendManyLocalized(
            IEnumerable<MailRecipient> recipients,
            Func<Localizer, MailGenConfigurationDto, MimeEntity> mailGenerator,
            Func<Localizer, string> subjectGenerator)
        {
            using SmtpClient client = new();

            try
            {
                await client.ConnectAsync(this.mailServerAddress, this.mailServerPort, this.useSsl);

                if (this.mailServerAuthenticationUsername != string.Empty || this.mailServerAuthenticationPassword != string.Empty)
                {
                    await client.AuthenticateAsync(
                        this.mailServerAuthenticationUsername,
                        this.mailServerAuthenticationPassword);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Could not connect to SMTP server while trying to send e-mails");
                return;
            }

            var senderMail = new MailboxAddress(this.mailSenderName, this.mailSenderAddress);

            Localizer localizer = new();
            using var localizerResourceStream = Assembly.GetExecutingAssembly().GetResource("Mailing.I18N.xml");

            var localeGroups = recipients.GroupBy(r => r.PreferredLcid);

            foreach (var localeGroup in localeGroups)
            {
                string language = localeGroup.Key ?? "en-EN";

                if (localizerResourceStream is not null)
                {
                    localizerResourceStream.Position = 0;
                    localizer.LoadXML(localizerResourceStream, CultureInfo.GetCultureInfo(language));
                }

                string subject = string.Format(CultureInfo.InvariantCulture, this.mailSubjectFormat, subjectGenerator(localizer));
                MimeEntity body = mailGenerator(localizer, this.mailGenConfigurationDto);

                foreach (var recipient in localeGroup)
                {
                    MimeMessage message = new();
                    message.From.Add(senderMail);
                    message.To.Add(recipient.Address);
                    message.Subject = subject;
                    message.Body = body;

                    try
                    {
                        await client.SendAsync(message);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Failed to send e-mail");
                    }
                }
            }

            await client.DisconnectAsync(true);
        }
    }
}

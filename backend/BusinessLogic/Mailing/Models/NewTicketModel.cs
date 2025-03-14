using I18N.DotNet;
using PersistenceLayer.DataAccess.Entities;

namespace DomainLayer.BusinessLogic.Mailing.Models
{
    /// <summary>
    /// Model for new ticket email
    /// </summary>
    public class NewTicketModel : MailModel
    {
        /// <inheritdoc/>
        public override string TemplateName => "NewTicket";

        /// <summary>
        /// The new ticket
        /// </summary>
        public required Ticket NewTicket { get; init; }

        /// <summary>
        /// Creation date of the <see cref="NewTicket"/> formatted to locale
        /// </summary>
        public required string FormattedCreationDate { get; init; }

        /// <summary>
        /// Create an instance of <see cref="NewTicketModel"/> from a <see cref="Ticket"/>
        /// </summary>
        /// <param name="newTicket">The new ticket</param>
        /// <param name="localizer">Localizer to use for mail generation</param>
        /// <returns>The generated model</returns>
        public static NewTicketModel FromTicket(Ticket newTicket, Localizer localizer)
        {
            return new()
            {
                MailTitle = localizer.Localize($"New ticket: '{newTicket.Title}'"),
                Localizer = localizer,
                NewTicket = newTicket,
                FormattedCreationDate = newTicket.CreationDate.ToString(
                    localizer.TargetCulture.DateTimeFormat.ShortDatePattern,
                    localizer.TargetCulture),
                RelatedTicketId = newTicket.Id,
            };
        }
    }
}

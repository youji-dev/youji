using I18N.DotNet;
using PersistenceLayer.DataAccess.Entities;

namespace DomainLayer.BusinessLogic.Mailing.Models
{
    public record NewTicketModel : MailModel
    {
        public override string TemplateName => "NewTicket";

        public required Ticket Ticket { get; init; }

        public required string FormattedCreationDate { get; init; }

        public static NewTicketModel FromTicket(Ticket ticket, Localizer localizer)
        {
            return new()
            {
                MailTitle = localizer.Localize($"New ticket: '{ticket.Title}'"),
                Localizer = localizer,
                Ticket = ticket,
                FormattedCreationDate = ticket.CreationDate.ToString(
                    localizer.TargetCulture.DateTimeFormat.ShortDatePattern,
                    localizer.TargetCulture),
            };
        }
    }
}

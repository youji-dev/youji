using PersistenceLayer.DataAccess.Entities;

namespace DomainLayer.BusinessLogic.Mailing.Models
{
    /// <summary>
    /// Model for new attachment email
    /// </summary>
    public record NewTicketAttachmentModel : MailModel
    {
        /// <summary>
        /// Name of the attachment
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="NewTicketAttachmentModel"/> from a <see cref="TicketAttachment"/>
        /// </summary>
        /// <param name="attachment">The attachment</param>
        /// <returns>The generated model</returns>
        public static NewTicketAttachmentModel FromAttachment(TicketAttachment attachment)
        {
            return new NewTicketAttachmentModel()
            {
#warning TODO: Platzhalter mit Ticket Titel ersetzten
                Title = $"Neue Anhang an '{"N/A"}'",
                Name = attachment.Name,
            };
        }
    }
}

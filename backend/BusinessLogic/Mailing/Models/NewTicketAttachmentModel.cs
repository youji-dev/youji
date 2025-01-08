using PersistenceLayer.DataAccess.Entities;

namespace DomainLayer.BusinessLogic.Mailing.Models
{
    /// <summary>
    /// Model for new attachment email
    /// </summary>
    public record NewTicketAttachmentModel : MailModel
    {
        /// <inheritdoc/>
        public override string TemplateName { get; } = "NewTicketAttachment";

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
                MailTitle = $"Neuer Anhang an '{attachment.Ticket?.Title ?? "unbekannt"}'",
                Name = attachment.Name,
            };
        }
    }
}

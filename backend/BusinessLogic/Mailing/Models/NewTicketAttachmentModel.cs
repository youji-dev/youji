using I18N.DotNet;
using PersistenceLayer.DataAccess.Entities;

namespace DomainLayer.BusinessLogic.Mailing.Models
{
    /// <summary>
    /// Model for new attachment email
    /// </summary>
    public class NewTicketAttachmentModel : MailModel
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
        /// <param name="localizer">Localizer to use for mail generation</param>
        /// <returns>The generated model</returns>
        public static NewTicketAttachmentModel FromAttachment(TicketAttachment attachment, Localizer localizer)
        {
            return new NewTicketAttachmentModel()
            {
                MailTitle = localizer.Localize($"New attachment on ticket '{attachment.Ticket?.Title ?? "unbekannt"}'"),
                Name = attachment.Name,
                Localizer = localizer,
                RelatedTicketId = attachment.TicketId,
            };
        }
    }
}

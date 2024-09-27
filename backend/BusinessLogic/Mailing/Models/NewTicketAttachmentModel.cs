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
    }
}

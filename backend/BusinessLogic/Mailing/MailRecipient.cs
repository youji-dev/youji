using MimeKit;

namespace DomainLayer.BusinessLogic.Mailing
{
    /// <summary>
    /// Represents a recipient of a mail with additonal meta information
    /// </summary>
    public struct MailRecipient
    {
        /// <summary>
        /// The mail address of the recipient
        /// </summary>
        public MailboxAddress Address { get; init;  }

        /// <summary>
        /// The preferred language of the recipient
        /// </summary>
        public string? PreferredLcid { get; init; }
    }
}

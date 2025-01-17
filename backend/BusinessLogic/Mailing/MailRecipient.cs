using MimeKit;
using PersistenceLayer.DataAccess.Entities;

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

        /// <summary>
        /// Filter and map a collection of users to a collection of mail recipients
        /// </summary>
        /// <param name="users">The collection of users</param>
        /// <returns>The collection of mail recipients</returns>
        public static IEnumerable<MailRecipient> GetCollectionFromUsers(IEnumerable<User> users)
        {
            return users
                .Where(u => !string.IsNullOrWhiteSpace(u.Email)
                    && u.AllowsEmailNotifications)
                .Select(u => new MailRecipient()
                {
                    Address = new MailboxAddress(u.UserId, u.Email),
                    PreferredLcid = u.PreferredEmailLcid,
                });
        }
    }
}

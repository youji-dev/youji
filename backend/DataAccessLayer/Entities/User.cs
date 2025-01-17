using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the assignment of the roles.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The id of the user.
        /// </summary>
        [Key]
        public required string UserId { get; set; }

        /// <summary>
        /// The type of the user.
        /// </summary>
        public required Roles Type { get; set; }

        /// <summary>
        /// E-Mail address of the user.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Preferred language code for e-mail notifications
        /// </summary>
        public string? PreferredEmailLcid { get; set; }

        /// <summary>
        /// Whether the user wants to recieve e-mail notifications
        /// </summary>
        public bool AreEmailNotificationsAllowed { get; set; }
    }
}

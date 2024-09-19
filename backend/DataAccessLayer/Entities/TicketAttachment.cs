using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Common.Helper;

namespace PersistenceLayer.DataAccess.Entities
{
    /// <summary>
    /// Represents the attachment at the ticket.
    /// </summary>
    public class TicketAttachment
    {
        /// <summary>
        /// The id of the attachment.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The binaries of the attachment.
        /// </summary>
        public required byte[] Binary { get; set; }

        /// <summary>
        /// The name of the attachment.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The file type of the attachment.
        /// </summary>
        public required string FileType { get; set; }
    }
}

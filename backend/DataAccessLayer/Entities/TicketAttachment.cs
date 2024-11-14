using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public required Guid Id { get; set; }

        /// <summary>
        /// The binaries of the attachment.
        /// null! is required because json serializer can't serialize properties
        /// that are required and null at the same time
        /// </summary>
        [JsonIgnore]
        public byte[] Binary { get; set; } = null!;

        /// <summary>
        /// The name of the attachment.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The file type of the attachment.
        /// </summary>
        public required string FileType { get; set; }

        /// <summary>
        /// The id of the related ticket.
        /// </summary>
        public required Guid TicketId { get; set; }

        /// <summary>
        /// The refernce type of the relatet tickets.
        /// </summary>
        [JsonIgnore]
        public Ticket? Ticket { get; set; }
    }
}

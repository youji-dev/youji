using PersistenceLayer.DataAccess.Entities;

namespace DomainLayer.BusinessLogic.PDF
{
    /// <summary>
    /// Model for PDF export document
    /// </summary>
    public record TicketExportModel
    {
        /// <inheritdoc cref="Ticket.Title"/>
        public required string Title { get; set; }

        /// <inheritdoc cref="Ticket.Description"/>
        public string? Description { get; set; }

        /// <inheritdoc cref="Ticket.Author"/>
        public required string Author { get; set; }

        /// <inheritdoc cref="Ticket.CreationDate"/>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// <see cref="Ticket.Attachments"/> filtered to only include images
        /// </summary>
        public ImageData[] Images { get; set; } = [];

        /// <inheritdoc cref="Ticket.Building"/>
        public string? Building { get; set; }

        /// <inheritdoc cref="Ticket.Room"/>
        public string? Room { get; set; }

        /// <inheritdoc cref="Ticket.Object"/>
        public string? Object { get; set; }

        /// <summary>
        /// Creates a new <see cref="TicketExportModel"/> from a <see cref="Ticket"/> entity
        /// </summary>
        /// <param name="ticket">The entity to create this from</param>
        /// <returns>A new <see cref="TicketExportModel"/></returns>
        public static TicketExportModel FromTicket(Ticket ticket)
        {
            var images = ticket.Attachments
                .Where(attachment => ((string[])["webp", "png", "jpeg", "jfif"]).Contains(attachment.FileType))
                .Select(attachment => new ImageData()
                {
                    FileName = attachment.Name,
                    Data = attachment.Binary,
                })
                .ToArray();

            return new()
            {
                Title = ticket.Title,
                Description = ticket.Description,
                Author = ticket.Author,
                CreationDate = ticket.CreationDate,
                Images = images,
                Building = ticket.Building?.Name,
                Room = ticket.Room,
                Object = ticket.Object,
            };
        }
    }
}

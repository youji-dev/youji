namespace Common.Contracts.Put
{
    /// <summary>
    /// Represents a ticket data transfer object for put operation.
    /// </summary>
    public class TicketPutDTO
    {
        /// <summary>
        /// The id of the ticket.
        /// </summary>
        public required Guid Id { get; set; }

        /// <summary>
        /// The title of the ticket.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// The description of the ticket.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The priority of the ticket
        /// </summary>
        public Guid PriorityId { get; set; }

        /// <summary>
        /// The state of the ticket.
        /// </summary>
        public Guid StateId { get; set; }

        /// <summary>
        /// The building where the issue of the ticket was located.
        /// </summary>
        public Guid BuildingId { get; set; }

        /// <summary>
        /// The room where the issue of the ticket was located.
        /// </summary>
        public string? Room { get; set; }

        /// <summary>
        /// The affected object.
        /// </summary>
        public string? Object { get; set; }
    }
}

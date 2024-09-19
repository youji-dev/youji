namespace Common.Contracts
{
    public interface ITicket
    {
        /// <summary>
        /// The title of the ticket:
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The description of the ticket.
        /// </summary>
        string? Description { get; set; }

        /// <summary>
        /// The priority of the ticket
        /// </summary>
        string PriorityName { get; set; }

        /// <summary>
        /// The author of the ticket.
        /// </summary>
        string Author { get; set; }

        /// <summary>
        /// The date when the ticket was created
        /// </summary>
        DateTime CreationDate { get; set; }

        /// <summary>
        /// The state of the ticket.
        /// </summary>
        Guid StateId { get; set; }

        /// <summary>
        /// The building where the issue of the ticket was located.
        /// </summary>
        Guid BuildingId { get; set; }

        /// <summary>
        /// The room where the issue of the ticket was located.
        /// </summary>
        string Room { get; set; }

        /// <summary>
        /// The affected object.
        /// </summary>
        string Object {  get; set; }
    }
}

namespace Common.Contracts.Get
{
    /// <summary>
    /// Represents a comment data transfer object for post operation.
    /// </summary>
    public class TicketSearchDTO
    {
        /// <summary>
        /// The total number of ticket results without pagination.
        /// </summary>
        public required int Total { get; set; }

        /// <summary>
        /// The results.
        /// </summary>
        public required Array Results { get; set; }
    }
}

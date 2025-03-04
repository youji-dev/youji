using PersistenceLayer.DataAccess.Entities;

namespace Application.WebApi.Contracts.Response
{
    /// <summary>
    /// Represents a comment data transfer object for post operation.
    /// </summary>
    public class TicketSearchResponseDTO
    {
        /// <summary>
        /// The total number of ticket results without pagination.
        /// </summary>
        public required int Total { get; set; }

        /// <summary>
        /// The results (for the current page).
        /// </summary>
        public required Ticket[] Results { get; set; }
    }
}

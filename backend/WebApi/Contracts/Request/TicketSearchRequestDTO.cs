namespace Application.WebApi.Contracts.Request
{
    /// <summary>
    /// Data transfer object for ticket search requests
    /// </summary>
    public class TicketSearchRequestDTO
    {
        /// <summary>
        /// List of filters to apply to the search
        /// </summary>
        public Dictionary<string, List<object>>? Filters { get; set; }

        /// <summary>
        /// The column to order the results by
        /// </summary>
        public string OrderByColumn { get; set; } = "CreationDate";

        /// <summary>
        /// The order direction of the results
        /// </summary>
        public bool OrderDesc { get; set; } = false;

        /// <summary>
        /// The number of results to skip. Used for pagination.
        /// </summary>
        public int Skip { get; set; } = 0;

        /// <summary>
        /// The number of results to take. Used for pagination.
        /// </summary>
        public int Take { get; set; } = 25;

        /// <summary>
        /// Indicates whether to use OR instead of AND for the filters
        /// </summary>
        public bool UseOr { get; set; } = false;
    }
}
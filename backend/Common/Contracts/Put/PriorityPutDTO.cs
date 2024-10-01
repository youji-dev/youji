namespace Common.Contracts.Put
{
    /// <summary>
    /// Represents a priority data transfer object for put operation.
    /// </summary>
    public class PriorityPutDTO
    {
        /// <summary>
        /// The id of the priority.
        /// </summary>
        public required Guid Id { get; set; }

        /// <summary>
        /// The value of the priority.
        /// </summary>
        public int? Value { get; set; }

        /// <summary>
        /// The name of the priority.
        /// </summary>
        public string? Name { get; set; }
    }
}

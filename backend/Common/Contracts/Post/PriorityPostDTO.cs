namespace Common.Contracts.Post
{
    /// <summary>
    /// Represents a priority data transfer object for post operation.
    /// </summary>
    public class PriorityPostDTO
    {
        /// <summary>
        /// The value of the priority.
        /// </summary>
        public required int Value { get; set; }

        /// <summary>
        /// The name of the priority.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The color of the priority.
        /// </summary>
        public required string Color { get; set; }
    }
}

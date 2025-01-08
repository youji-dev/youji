namespace Common.Contracts.Post
{
    /// <summary>
    /// Represents a state data transfer object for post operation.
    /// </summary>
    public class StatePostDTO
    {
        /// <summary>
        /// The name of the state.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The color of the state.
        /// </summary>
        public required string Color { get; set; }

        /// <summary>
        /// Indicates if tickets will be purged automatically.
        /// </summary>
        public required bool HasAutoPurge { get; set; }

        /// <summary>
        /// Days from when tickets will be purged.
        /// </summary>
        public int? AutoPurgeDays { get; set; }
    }
}

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
        /// Marks the status so that tickets with it can be purged automatically.
        /// </summary>
        public required bool HasAutoPurge { get; set; }

        /// <summary>
        /// Days from when tickets will purged.
        /// </summary>
        public int? AutoPurgeDays { get; set; }
    }
}

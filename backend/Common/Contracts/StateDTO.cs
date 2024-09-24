namespace Common.Contracts
{
    /// <summary>
    /// Represents a state data transfer object.
    /// </summary>
    public class StateDTO
    {
        /// <summary>
        /// The name of the state.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The color of the state.
        /// </summary>
        public required string Color { get; set; }
    }
}

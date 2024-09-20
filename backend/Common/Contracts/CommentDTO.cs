namespace Common.Contracts
{
    /// <summary>
    /// Represents a comment data transfer object.
    /// </summary>
    public class CommentDTO
    {
        /// <summary>
        /// The author of the comment.
        /// </summary>
        public required string Author { get; set; }

        /// <summary>
        /// The content of the comment.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// The date and time when the comment was created.
        /// </summary>
        public required DateTime CreationDate { get; set; }
    }
}

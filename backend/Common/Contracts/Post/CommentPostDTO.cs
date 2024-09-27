namespace Common.Contracts.Post
{
    /// <summary>
    /// Represents a comment data transfer object for post operation.
    /// </summary>
    public class CommentPostDTO
    {
        /// <summary>
        /// The author of the comment.
        /// </summary>
        public required string Author { get; set; }

        /// <summary>
        /// The content of the comment.
        /// </summary>
        public required string Content { get; set; }
    }
}

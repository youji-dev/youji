namespace Common.Contracts
{
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

        /// <summary>
        /// The ticket id where the comment will binded
        /// </summary>
        public required Guid TicketId { get; set; }
    }
}

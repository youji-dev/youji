/**
 * Represents a comment that was written in a ticket
 */
export default interface ticketComment {
  /**
   * The Id of the comment
   */
  id: string;

  /**
   * The author of the comment
   */
  author: string;

  /**
   * The content of the comment.
   */
  content: string;

  /**
   * The date and time when the comment was created.
   */
  creationDate: Date;

  /**
   * The id of the ticket that this comment belongs to
   */
  ticketId: string;
}

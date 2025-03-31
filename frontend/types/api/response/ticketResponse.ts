import type building from './buildingResponse';
import type priority from './priorityResponse';
import type state from './stateResponse';
import type ticketAttachment from './ticketAttachmentResponse';
import type ticketComment from './ticketCommentResponse';

/**
 * Represents a ticket
 */
export default interface ticket {
  /**
   * The Id of the ticket
   */
  id: string;

  /**
   * The title of the ticker
   */
  title: string;

  /**
   * The description of the ticket
   */
  description: string | undefined;

  /**
   * The author of the ticket
   */
  author: string;

  /**
   * The point in time where the Ticket was created
   */
  creationDate: Date;

  /**
   * The priority that was set for this ticket
   */
  priority: priority;

  /**
   * The state that was set for this ticket
   */
  state: state;

  /**
   * The building where the issue of this ticket was located
   */
  building: building | undefined;

  /**
   * The room where the issue of this ticket was located
   */
  room: string | undefined;

  /**
   * The object that the report is about
   */
  object: string | undefined;

  /**
   * The comments of the ticket
   */
  comments: ticketComment[];

  /**
   * The attachments of the ticket
   */
  attachments: ticketAttachment[];
}

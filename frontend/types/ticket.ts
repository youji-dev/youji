import type building from "./building";
import type priority from "./priority";
import type state from "./state";
import type ticketAttachment from "./ticketAttachment";
import type ticketComment from "./ticketComment";

/**
 * Represents a ticket
 */
export default interface ticket {
	/**
	 * The Id of the ticket
	 */
	id?: string;

	/**
	 * The title of the ticker
	 */
	title: string;

	/**
	 * The description of the ticket
	 */
	description: string | null;

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
	building: building | null;

	room: string | null;

	object: string | null;

	Comments: ticketComment[];

	Attachments: ticketAttachment[];
}

/**
 * Represents a file that was uploaded to a ticket
 */
export default interface ticketAttachment {
	/**
	 * The Id of the attachment
	 */
	id?: string;

	/**
	 * The attachment itself as binary
	 */
	binary: any;

	/**
	 * The file name of the attachment
	 */
	name: string;

	/**
	 * The file type of the attachment
	 */
	fileType: string;

	/**
	 * The id of the ticket that this attachment belongs to
	 */
	ticketId: string;
}

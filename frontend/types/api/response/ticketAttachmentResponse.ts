/**
 * Represents a file that was uploaded to a ticket
 */
export default interface ticketAttachment {
  /**
   * The Id of the attachment
   */
  id: string;

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

  /**
   * Blurhash of the attachment. Null if attachment is not an image.
   * This is used to generate the image preview before the image is downloadet
   * @see https://blurha.sh
  */
  blurHash?: string;
}

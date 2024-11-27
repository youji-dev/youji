/**
 * Represents the state of a ticket
 */
export default interface state {
  /**
   * The Id of the ticket
   */
  id: string;

  /**
   * The name of the state
   */
  name: string;

  /**
   * The color of the state
   */
  color: string;

  /**
   * Marks the status so that tickets with it can be purged automatically
   */
  hasAutoPurge: boolean;

  /**
   * Days from when tickets will purged
   */
  autoPurgeDays: number;
}

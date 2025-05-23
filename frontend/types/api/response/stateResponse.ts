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
   * Indicates if tickets will be purged automatically
   */
  hasAutoPurge: boolean;

  /**
   * Days from when tickets will be purged
   */
  autoPurgeDays: number;

  /**
   * Indicates if this is the default state for new tickets
   */
  isDefault: boolean;
}

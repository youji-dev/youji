/**
 * Represents the ticket priority.
 */
export default interface priority {
    /**
     * The id of the priority.
     */
    id: string;

    /**
     * The value of the priority. Used for sort order.
     */
    value: number;

    /**
     * The name of the priority.
     */
    name: string;
    /**
     * The color to be displayed for the priority.
     */
    color: string;
}


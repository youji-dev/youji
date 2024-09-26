/**
 * Represents the state of a ticket
 */
export default interface state {
    /**
     * The Id of the ticket
     */
    id?: string,

    /**
     * The name of the state
     */
    name: string,

    /**
     * The color of the state
     */
    color: string,
}
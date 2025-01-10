import type ticket from "./ticketResponse";

/**
 * Represents a ticket
 */
export default interface searchResponse {
    /**
     * T
     */
    results: ticket[];
  
    /**
     * The total number of ticket results without pagination
     */
    total: number;
  }
  
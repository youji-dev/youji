import type ticketResponse from '../response/ticketResponse';

export default interface CreateTicketRequest {
  title: string;
  description: string | null;
  priorityId: string;
  author: string;
  stateId: string;
  buildingId: string | null;
  room: string | null;
  object: string | null;
}

/**
 * Parses a ticket response to a create ticket type
 * @param ticket the ticket response to parse
 * @returns Parsed create ticket request
 */
export function fromTicketResponse(ticket: ticketResponse): CreateTicketRequest {
  return {
    title: ticket.title,
    description: ticket.description ?? null,
    priorityId: ticket.priority.id,
    author: ticket.author,
    stateId: ticket.state.id,
    buildingId: ticket.building?.id || null,
    room: ticket.room || null,
    object: ticket.object || null,
  };
}

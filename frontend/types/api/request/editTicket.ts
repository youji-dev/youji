import type ticketResponse from '../response/ticketResponse';

export default interface EditTicketRequest {
  id: string;
  title: string | undefined;
  description: string | undefined;
  priorityId: string;
  stateId: string;
  buildingId: string | undefined;
  room: string | undefined;
  object: string | undefined;
}

/**
 * Parses a ticket response to a edit ticket type
 * @param ticket The ticket response to parse
 * @returns Parsed edit ticket request
 */
export function fromTicketResponse(ticket: ticketResponse): EditTicketRequest {
  return {
    id: ticket.id,
    title: ticket.title,
    description: ticket.description ?? undefined,
    priorityId: ticket.priority.id,
    stateId: ticket.state.id,
    buildingId: ticket.building?.id || undefined,
    room: ticket.room || undefined,
    object: ticket.object || undefined,
  };
}

import type ticketResponse from "../response/ticketResponse";

export default interface EditTicketRequest {
  id: string;
  title: string | null;
  description: string | null;
  priorityValue: number;
  stateId: string;
  buildingId: string | null;
  room: string | null;
  object: string | null;
}

export function fromTicketResponse(ticket: ticketResponse): EditTicketRequest {
  return {
    id: ticket.id,
    title: ticket.title,
    description: ticket.description,
    priorityValue: ticket.priority.value,
    stateId: ticket.state.id,
    buildingId: ticket.building?.id || null,
    room: ticket.room || null,
    object: ticket.object || null,
  };
}

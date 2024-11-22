import type ticketResponse from "../response/ticketResponse";

export default interface EditTicketRequest {
  id: string;
  title: string | undefined;
  description: string | undefined;
  priorityValue: number;
  stateId: string;
  buildingId: string | undefined;
  room: string | undefined;
  object: string | undefined;
}

export function fromTicketResponse(ticket: ticketResponse): EditTicketRequest {
  return {
    id: ticket.id,
    title: ticket.title,
    description: ticket.description ?? undefined,
    priorityValue: ticket.priority.value,
    stateId: ticket.state.id,
    buildingId: ticket.building?.id || undefined,
    room: ticket.room || undefined,
    object: ticket.object || undefined,
  };
}

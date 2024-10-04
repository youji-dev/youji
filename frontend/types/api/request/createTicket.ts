import type ticket from "../response/ticketResponse";

export default interface CreateTicketRequest {
  title: string;
  description: string | null;
  priorityValue: number;
  author: string;
  stateId: string;
  buildingId: string | null;
  room: string | null;
  object: string | null;
}

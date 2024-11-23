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

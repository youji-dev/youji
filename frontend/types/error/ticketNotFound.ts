export default class TicketNotFoundError extends Error {
  constructor(message: string) {
    super(message);
  }
}

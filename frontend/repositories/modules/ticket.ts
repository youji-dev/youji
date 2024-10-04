import HttpFactory from "@/repositories/factory";
import type ticket from "~/types/api/response/ticketResponse";
import type CreateTicketRequest from "~/types/api/request/createTicket";
import type ticketAttachment from "~/types/api/response/ticketAttachmentResponse";
import type ticketComment from "~/types/api/response/ticketCommentResponse";
import type createCommentRequest from "~/types/api/request/createComment";

class TicketRepository extends HttpFactory {
  private path = "/api/Ticket";

  async get(id: string): Promise<ReturnType<typeof useFetchAuthenticated<ticket>>> {
    return this.call<ticket>(`${this.path}/${id}`, { method: "GET" });
  }

  async delete(id: string): Promise<ReturnType<typeof useFetchAuthenticated<void>>> {
    return this.call<void>(`${this.path}/${id}`, { method: "DELETE" });
  }

  async search(searchTerm: string, orderByColumn: string, orderDesc: boolean, skip: number, take: number): Promise<ReturnType<typeof useFetchAuthenticated<ticket[]>>> {
    return this.call<ticket[]>(`${this.path}/search`, {
      method: "GET",
      query: {
        searchTerm,
        orderByColumn,
        orderDesc,
        skip,
        take,
      },
    });
  }

  async create(ticket: CreateTicketRequest): Promise<ReturnType<typeof useFetchAuthenticated<ticket>>> {
    return this.call<ticket>(this.path, { method: "POST", body: ticket });
  }

  async edit(ticket: CreateTicketRequest): Promise<ReturnType<typeof useFetchAuthenticated<ticket>>> {
    return this.call<ticket>(this.path, { method: "PUT", body: ticket });
  }

  async getComments(id: string): Promise<ReturnType<typeof useFetchAuthenticated<ticketComment[]>>> {
    return this.call<ticketComment[]>(`${this.path}/${id}/comments`, { method: "GET" });
  }

  async getAttachments(id: string): Promise<ReturnType<typeof useFetchAuthenticated<ticketAttachment[]>>> {
    return this.call<ticketAttachment[]>(`${this.path}/${id}/attachments`, { method: "GET" });
  }

  async addComment(id: string, comment: createCommentRequest): Promise<ReturnType<typeof useFetchAuthenticated<ticketComment>>> {
    return this.call<ticketComment>(`${this.path}/${id}/comment`, { method: "POST", body: comment });
  }

  async uploadAttachment(id: string, file: File): Promise<ReturnType<typeof useFetchAuthenticated<ticketAttachment>>> {
    const formData = new FormData();
    formData.append("attachmentFile", file);
    return this.call<ticketAttachment>(`${this.path}/${id}/attachment`, { method: "POST", headers: { "Content-Type": "multipart/form-data" }, body: formData });
  }
}

export default TicketRepository;

import type ticket from "~/types/api/response/ticketResponse";
import type CreateTicketRequest from "~/types/api/request/createTicket";
import type ticketAttachment from "~/types/api/response/ticketAttachmentResponse";
import type ticketComment from "~/types/api/response/ticketCommentResponse";
import type createCommentRequest from "~/types/api/request/createComment";
import type EditTicketRequest from "~/types/api/request/editTicket";

class TicketRepository {
  private path = "/api/Ticket";

  async get(id: string): Promise<ReturnType<typeof useFetchAuthenticated<ticket>>> {
    return useFetchAuthenticated<ticket>(`${this.path}/${id}`, { method: "GET" });
  }

  async search(searchTerm: string, orderByColumn: string, orderDesc: boolean, skip: number, take: number): Promise<ReturnType<typeof useFetchAuthenticated<ticket[]>>> {
    return useFetchAuthenticated<ticket[]>(`${this.path}/search`, {
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
    return useFetchAuthenticated<ticket>(this.path, { method: "POST", body: ticket });
  }

  async edit(ticket: EditTicketRequest): Promise<ReturnType<typeof useFetchAuthenticated<ticket>>> {
    return useFetchAuthenticated<ticket>(this.path, { method: "PUT", body: ticket });
  }

  async delete(id: string): Promise<ReturnType<typeof useFetchAuthenticated<string>>> {
    return useFetchAuthenticated<string>(`${this.path}/${id}`, { method: "DELETE" });
  }

  async getComments(id: string): Promise<ReturnType<typeof useFetchAuthenticated<ticketComment[]>>> {
    return useFetchAuthenticated<ticketComment[]>(`${this.path}/${id}/comments`, { method: "GET" });
  }

  async getAttachments(id: string): Promise<ReturnType<typeof useFetchAuthenticated<ticketAttachment[]>>> {
    return useFetchAuthenticated<ticketAttachment[]>(`${this.path}/${id}/attachments`, { method: "GET" });
  }

  async addComment(id: string, comment: createCommentRequest): Promise<ReturnType<typeof useFetchAuthenticated<ticketComment>>> {
    return useFetchAuthenticated<ticketComment>(`${this.path}/${id}/comment`, { method: "POST", body: comment });
  }

  async uploadAttachment(id: string, file: File): Promise<ReturnType<typeof useFetchAuthenticated<ticketAttachment>>> {
    const formData = new FormData();
    formData.append("attachmentFile", file);
    return useFetchAuthenticated<ticketAttachment>(`${this.path}/${id}/attachment`, { method: "POST", headers: { "Content-Type": "multipart/form-data" }, body: formData });
  }

  async exportToPDF(id: string, language: string) {
    await useFetchAuthenticated<Blob>(`${this.path}/${id}/export`, {
      method: "GET",
      headers: {
        "Accept-Language": language,
      },
      onResponse({ response }) {
        const filename = response.headers.get("Content-Disposition")?.split("filename=")[1]?.trim();
        const url = window.URL.createObjectURL(new Blob([response._data]));
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", filename ?? "ticket.pdf");
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      },
    });
  }
}

export default TicketRepository;

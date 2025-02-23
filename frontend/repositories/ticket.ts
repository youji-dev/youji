import type ticket from "~/types/api/response/ticketResponse";
import type CreateTicketRequest from "~/types/api/request/createTicket";
import type ticketAttachment from "~/types/api/response/ticketAttachmentResponse";
import type ticketComment from "~/types/api/response/ticketCommentResponse";
import type EditTicketRequest from "~/types/api/request/editTicket";
import type searchResponse from "~/types/api/response/searchResponse";

class TicketRepository {
  private path = "/api/Ticket";

  async get(
    id: string
  ): Promise<ReturnType<typeof useFetchAuthenticated<ticket>>> {
    return useFetchAuthenticated<ticket>(`${this.path}/${id}`, {
      method: "GET",
    });
  }

  async search(
    searchTerm: string,
    orderByColumn: string,
    orderDesc: boolean,
    skip: number,
    take: number
  ): Promise<ReturnType<typeof useFetchAuthenticated<searchResponse>>> {
    return useFetchAuthenticated<searchResponse>(`${this.path}/search`, {
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

  async create(
    ticket: CreateTicketRequest
  ): Promise<ReturnType<typeof useFetchAuthenticated<ticket>>> {
    return useFetchAuthenticated<ticket>(this.path, {
      method: "POST",
      body: ticket,
    });
  }

  async edit(
    ticket: EditTicketRequest
  ): Promise<ReturnType<typeof useFetchAuthenticated<ticket>>> {
    return useFetchAuthenticated<ticket>(this.path, {
      method: "PUT",
      body: ticket,
    });
  }

  async delete(
    id: string
  ): Promise<ReturnType<typeof useFetchAuthenticated<string>>> {
    return useFetchAuthenticated<string>(`${this.path}/${id}`, {
      method: "DELETE",
    });
  }

  async getComments(
    id: string
  ): Promise<ReturnType<typeof useFetchAuthenticated<ticketComment[]>>> {
    return useFetchAuthenticated<ticketComment[]>(
      `${this.path}/${id}/comments`,
      { method: "GET" }
    );
  }

  async getAttachments(
    id: string
  ): Promise<ReturnType<typeof useFetchAuthenticated<ticketAttachment[]>>> {
    return useFetchAuthenticated<ticketAttachment[]>(
      `${this.path}/${id}/attachments`,
      { method: "GET" }
    );
  }

  async addComment(
    id: string,
    comment: string
  ): Promise<ReturnType<typeof useFetchAuthenticated<ticketComment>>> {
    return useFetchAuthenticated<ticketComment>(`${this.path}/${id}/comment`, {
      method: "POST",
      headers: { "Content-Type": "text/plain" },
      body: comment,
    });
  }

  async exportToPDF(id: string, language: string) {
    await useFetchAuthenticated<Blob>(`${this.path}/${id}/export`, {
      method: "GET",
      query: { lang: language },
      onResponse({ response }) {
        const filename = `ticket_${id}_${new Date(
          Date.now()
        ).toISOString()}.pdf`;

        const url = window.URL.createObjectURL(new Blob([response._data]));
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", filename);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      },
    });
  }
}

export default TicketRepository;

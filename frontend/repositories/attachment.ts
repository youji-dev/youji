class AttachmentRepository {
  private path = "/api/TicketAttachment";

  async getBlob(id: string): Promise<ReturnType<typeof useFetchAuthenticated<Blob>>> {
    return useFetchAuthenticated<Blob>(`${this.path}/${id}`, { method: "GET" });
  }

  async delete(id: string): Promise<ReturnType<typeof useFetchAuthenticated<string>>> {
    return useFetchAuthenticated<string>(`${this.path}/${id}`, { method: "DELETE" });
  }
}

export default AttachmentRepository;

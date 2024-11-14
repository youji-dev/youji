class AttachmentRepository {
  private path = "/api/TicketAttachment";

  async delete(id: string): Promise<ReturnType<typeof useFetchAuthenticated<string>>> {
    return useFetchAuthenticated<string>(`${this.path}/${id}`, { method: "DELETE" });
  }
}

export default AttachmentRepository;

class AttachmentRepository {
  private path = "/api/TicketAttachment";

  generateAttachmentURL(id: string): string {
    const {
      public: { BACKEND_URL },
    } = useRuntimeConfig();

    return `${BACKEND_URL}${this.path}/${id}`;
  }

  async delete(id: string): Promise<ReturnType<typeof useFetchAuthenticated<string>>> {
    return useFetchAuthenticated<string>(`${this.path}/${id}`, { method: "DELETE" });
  }
}

export default AttachmentRepository;

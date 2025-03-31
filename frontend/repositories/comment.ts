class CommentRepository {
  private path = '/api/TicketComment';

  async delete(id: string): Promise<ReturnType<typeof useFetchAuthenticated<string>>> {
    return useFetchAuthenticated<string>(`${this.path}/${id}`, { method: 'DELETE' });
  }
}

export default CommentRepository;

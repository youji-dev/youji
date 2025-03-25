import type priority from '~/types/api/response/priorityResponse';

class PriorityRepository {
  private path = '/api/Priority';

  async getAll(): Promise<ReturnType<typeof useFetchAuthenticated<priority[]>>> {
    return useFetchAuthenticated<priority[]>(this.path, { method: 'GET' });
  }

  async create(priority: priority): Promise<ReturnType<typeof useFetchAuthenticated<priority>>> {
    return useFetchAuthenticated<priority>(this.path, { method: 'POST', body: priority });
  }

  async edit(priority: priority): Promise<ReturnType<typeof useFetchAuthenticated<priority>>> {
    return useFetchAuthenticated<priority>(this.path, { method: 'PUT', body: priority });
  }

  async delete(priorityValue: string): Promise<ReturnType<typeof useFetchAuthenticated<string>>> {
    return useFetchAuthenticated<string>(`${this.path}/${priorityValue}`, { method: 'DELETE' });
  }
}

export default PriorityRepository;

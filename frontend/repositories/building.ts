import type building from '~/types/api/response/buildingResponse';

class BuildingRepository {
  private path = '/api/Building';

  async getAll(): Promise<ReturnType<typeof useFetchAuthenticated<building[]>>> {
    return useFetchAuthenticated<building[]>(this.path, { method: 'GET' });
  }

  async create(name: string): Promise<ReturnType<typeof useFetchAuthenticated<building>>> {
    return useFetchAuthenticated<building>(this.path, {
      method: 'POST',
      headers: { 'Content-Type': 'text/plain' },
      body: name,
    });
  }

  async edit(building: building): Promise<ReturnType<typeof useFetchAuthenticated<building>>> {
    return useFetchAuthenticated<building>(this.path, { method: 'PUT', body: building });
  }

  async delete(id: string): Promise<ReturnType<typeof useFetchAuthenticated<string>>> {
    return useFetchAuthenticated<string>(`${this.path}/${id}`, { method: 'DELETE' });
  }
}

export default BuildingRepository;

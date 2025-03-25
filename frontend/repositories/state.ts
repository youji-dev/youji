import type createStateRequest from '~/types/api/request/createPost';
import type state from '~/types/api/response/stateResponse';

class StateRepository {
  private path = '/api/State';

  async getAll(): Promise<ReturnType<typeof useFetchAuthenticated<state[]>>> {
    return useFetchAuthenticated<state[]>(this.path, { method: 'GET' });
  }

  async create(state: createStateRequest): Promise<ReturnType<typeof useFetchAuthenticated<state>>> {
    return useFetchAuthenticated<state>(this.path, { method: 'POST', body: state });
  }

  async edit(state: state): Promise<ReturnType<typeof useFetchAuthenticated<state>>> {
    return useFetchAuthenticated<state>(this.path, { method: 'PUT', body: state });
  }

  async delete(id: string): Promise<ReturnType<typeof useFetchAuthenticated<string>>> {
    return useFetchAuthenticated<string>(`${this.path}/${id}`, { method: 'DELETE' });
  }
}

export default StateRepository;

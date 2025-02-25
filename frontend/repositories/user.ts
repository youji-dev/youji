import type CreateUserRequest from "~/types/api/request/createUser";
import type user from "~/types/api/response/userResponse";

class UserRepository {
  private path = "/api/User";

  async getAll(): Promise<ReturnType<typeof useFetchAuthenticated<user[]>>> {
    return useFetchAuthenticated<user[]>(this.path, { method: "GET" });
  }

  async create(state: CreateUserRequest): Promise<ReturnType<typeof useFetchAuthenticated<user>>> {
    return useFetchAuthenticated<user>(this.path, { method: "POST", body: state });
  }

  async edit(state: user): Promise<ReturnType<typeof useFetchAuthenticated<user>>> {
    return useFetchAuthenticated<user>(this.path, { method: "PUT", body: state });
  }

  async delete(id: string): Promise<ReturnType<typeof useFetchAuthenticated<string>>> {
    return useFetchAuthenticated<string>(`${this.path}/${id}`, { method: "DELETE" });
  }
}

export default UserRepository;

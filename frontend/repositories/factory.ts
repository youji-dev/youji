import type { AsyncData, UseFetchOptions } from "#app";
import useFetchAuthenticated from "~/composables/useFetchAuthenticated";

class HttpFactory {
  async call<T>(url: string, options?: UseFetchOptions<T>): Promise<ReturnType<typeof useFetchAuthenticated<T>>> {
    return useFetchAuthenticated<T>(url, options);
  }
}

export default HttpFactory;

import { defineStore } from "pinia";
import useFetchAuthenticated from "~/composables/useFetchAuthenticated";

interface UserPayloadInterface {
  name: string;
  password: string;
}

export const useAuthStore = defineStore("auth", {
  state: () => ({
    authenticated: false,
    name: "" as any,
    role: 0 as number,
    loading: false,
    csrfToken: "" as any,
    authErrors: [] as string[],
  }),
  actions: {
    async authenticateUser({ name, password }: UserPayloadInterface) {
      const {
        public: { BACKEND_URL, ACCESS_TOKEN_NAME, REFRESH_TOKEN_NAME },
      } = useRuntimeConfig();

      this.authErrors = [];

      try {
        const { data, pending, error }: any = await useFetch(
          `${BACKEND_URL}/Auth/login`,
          {
            body: {
              username: name,
              password: password,
            },
            method: "post",
            headers: { "Content-Type": "application/json" },
          }
        );
        this.loading = pending;

        if (error?.value?.statusCode === 401) {
          this.authErrors.push("wrong credentials");
        } else if (error?.value) {
          console.error(error);
          this.authErrors.push(error.value);
        }

        if (!data.value) return;
        const accessToken = useCookie(ACCESS_TOKEN_NAME);
        const refreshToken = useCookie(REFRESH_TOKEN_NAME);
        accessToken.value = data.value.accessToken;
        refreshToken.value = data.value.refreshToken;
        console.log(accessToken);
        console.log(accessToken.value);
        this.authenticated = true;
      } catch (error) {
        console.error(error);
      }
    },
    logUserOut() {
      this.authenticated = false;
      useCookie(useRuntimeConfig().public.ACCESS_TOKEN_NAME).value = null;
      useCookie(useRuntimeConfig().public.REFRESH_TOKEN_NAME).value = null;
    },

    async checkIfTokenIsValid(): Promise<boolean> {
      try {
        await useFetchAuthenticated("/Auth/verify-token");
        return true;
      } catch {
        return false;
      }
    },
  },
});

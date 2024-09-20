import { defineStore } from "pinia";

// In this store we can define actions for authenticating the user at the backend and store variables like the state of the authentication request, errors, user information ...
// All of these actions and variables can be used and called in our vue files.

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
    authErrors: [] as Array<string>,
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
              password,
            },
            method: "post",
            headers: { "Content-Type": "application/json" },
          }
        );

        this.loading = pending;

        if (error.value) {
          console.error(error.value);
          this.authErrors.push(error.value);
          return;
        }

        if (!data.value) {
          console.error("missing data");
          this.authErrors.push("missing data");
          return;
        }

        const accessToken = useCookie(ACCESS_TOKEN_NAME, {
          httpOnly: true,
          secure: true,
          sameSite: "strict",
        });

        const refreshToken = useCookie(REFRESH_TOKEN_NAME, {
          httpOnly: true,
          secure: true,
          sameSite: "strict",
        });

        accessToken.value = data.value.accessToken;
        refreshToken.value = data.value.refreshToken;

        this.authenticated = true;
      } catch (error) {
        console.error(error);
      }
    },
    async refreshAccessToken() {
      const {
        public: { BACKEND_URL, ACCESS_TOKEN_NAME, REFRESH_TOKEN_NAME },
      } = useRuntimeConfig();

      try {
        const accessToken = useCookie(ACCESS_TOKEN_NAME, {
          httpOnly: true,
          secure: true,
          sameSite: "strict",
        });

        const refreshToken = useCookie(REFRESH_TOKEN_NAME, {
          httpOnly: true,
          secure: true,
          sameSite: "strict",
        });

        const { data, pending, error }: any = await useFetch(
          `${BACKEND_URL}/Auth/refresh`,
          {
            body: {
              refreshToken: refreshToken.value,
            },
            method: "post",
            headers: { "Content-Type": "application/json" },
          }
        );

        this.loading = pending;

        if (error.value) {
          console.error(error.value);
          this.authErrors.push(error.value);
          this.authenticated = false;
          return;
        }

        if (!data.value) {
          console.error("missing data");
          this.authErrors.push("missing data");
          this.authenticated = false;
          return;
        }


        accessToken.value = data.value.accessToken;
        refreshToken.value = data.value.refreshToken;

        this.authenticated = true;
      } catch (error) {
        console.error(error);
        this.authenticated = false;
      }

    },
    logUserOut() {
      this.authenticated = false;
      useCookie(useRuntimeConfig().public.ACCESS_TOKEN_NAME).value = null;
      useCookie(useRuntimeConfig().public.REFRESH_TOKEN_NAME).value = null;
    },

    checkIfTokenIsSet() {
      return !!useCookie(useRuntimeConfig().public.REFRESH_TOKEN_NAME, { httpOnly: true, secure: true, sameSite: 'strict' }).value;
    },

    async checkIfTokenIsValid(): Promise<boolean> {
      const { $api } = useNuxtApp();
      if (!this.checkIfTokenIsSet()) return false;

      try {
        await $api("Auth/verify-token");
        return true;
      } catch {
        return false;
      }
    }
  },
});

// https://dev.to/rafaelmagalhaes/authentication-in-nuxt-3-375o

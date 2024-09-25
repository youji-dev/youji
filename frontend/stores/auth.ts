import { defineStore } from "pinia";
import type { Response } from "~/types/response";
// In this store we can define actions for authenticating the user at the backend and store variables like the state of the authentication request, errors, user information ...
// All of these actions and variables can be used and called in our vue files.

interface UserPayloadInterface {
  name: string;
  password: string;
}

type CheckAuthData = {
  authenticated: boolean,
  userId: number
}

export const useAuthStore = defineStore("auth", {
  state: () => ({
    authenticated: false,
    name: "" as any,
    loading: false,
    csrfToken: "" as any,
    authErrors: [] as Array<string>,
  }),
  actions: {
    async authenticateUser({ name, password }: UserPayloadInterface) {
      const {
        public: { BACKEND_URL, AUTH_TOKEN_NAME },
      } = useRuntimeConfig();
      const { $locally } = useNuxtApp();
      this.authErrors = [];
      const { data, pending, error }: any = await useFetch(
        BACKEND_URL + "/api/login",
        {
          body: {
            name: name,
            password: password,
          },
          method: "post",
          headers: { "Content-Type": "application/json" },
        }
      );
      this.loading = pending;
      if (error.value) {
        console.log(error.value);
        this.authErrors.push(error.value);
        console.log(error.value);
      }
      if (data.value) {
        if (data.value.allowed) {
          const token = useCookie(AUTH_TOKEN_NAME, { httpOnly: true, secure: true, sameSite: 'strict' });
          token.value = data?.value?.token;
          this.authenticated = true;
          this.name = name;
          $locally.setItem("username", name);
        }
      }
    },
    async logUserOut() {
      const {
        public: { BACKEND_URL, AUTH_TOKEN_NAME },
      } = useRuntimeConfig();
      const token = useCookie(AUTH_TOKEN_NAME, { httpOnly: true, secure: true, sameSite: 'strict' });
      this.authenticated = false;
      const { data, error }: any = await useFetch<Response<CheckAuthData>>(BACKEND_URL + "/api/logout", {
        method: "get",
        headers: {
          Authorization: "Bearer " + token.value,
          "Content-Type": "application/json",
        },
      });
      if (error) {
        console.log(error);
      }
      token.value = null;
    },

    checkIfTokenIsSet() {
      const {
        public: { AUTH_TOKEN_NAME },
      } = useRuntimeConfig();
      const cookie = useCookie(AUTH_TOKEN_NAME, { httpOnly: true, secure: true, sameSite: 'strict' });
      return !!cookie.value
    },

    async checkIfTokenIsValid() {
      const {
        public: { BACKEND_URL, AUTH_TOKEN_NAME },
      } = useRuntimeConfig();
      const cookie = useCookie(AUTH_TOKEN_NAME, { httpOnly: true, secure: true, sameSite: 'strict' });
      if (this.checkIfTokenIsSet()) {
        const { data, error }: any = await useFetch(BACKEND_URL + "/api/check-token", {
          method: "post",
          headers: {
            Authorization: "Bearer " + cookie.value,
            "Content-Type": "application/json",
          },
        });
        if (error) {
          this.authErrors.push(error)
          console.log(error);
        }
        if (data.valid) {
          return true;
        } else {
          return false;
        }
      } else {
        return false;
      }
    }
  },
});

// https://dev.to/rafaelmagalhaes/authentication-in-nuxt-3-375o

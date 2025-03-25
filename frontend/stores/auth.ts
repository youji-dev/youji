import { jwtDecode } from 'jwt-decode';
import { defineStore } from 'pinia';
import useFetchAuthenticated from '~/composables/useFetchAuthenticated';
import { Roles } from '~/types/roles';

interface UserPayloadInterface {
  name: string;
  password: string;
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    authenticated: false,
    username: '' as string,
    userRole: 0 as Roles,
    loading: false,
    csrfToken: '' as any,
    authErrors: [] as string[],
  }),
  getters: {
    isUserAdmin: state => (state.userRole & Roles.Admin) > 0,
    isUserFacilityManager: state => (state.userRole & Roles.FacilityManager) > 0,
    isUserTeacher: state => (state.userRole & Roles.Teacher) > 0,
  },
  actions: {
    async authenticateUser({ name, password }: UserPayloadInterface) {
      const {
        public: { BACKEND_URL, ACCESS_TOKEN_NAME, REFRESH_TOKEN_NAME },
      } = useRuntimeConfig();

      this.authErrors = [];

      try {
        const { data, pending, error }: any = await useFetch(`${BACKEND_URL}/Auth/login`, {
          body: {
            username: name,
            password: password,
          },
          method: 'post',
          headers: { 'Content-Type': 'application/json' },
        });
        this.loading = pending;

        if (error?.value?.statusCode === 401) {
          this.authErrors.push('wrong credentials');
        } else if (error?.value) {
          console.error(error);
          this.authErrors.push(error.value);
        }

        if (!data.value.accessToken || !data.value.refreshToken) {
          this.authErrors.push('Did not receive expected response with access and refresh token');
          return;
        }

        const accessToken = useCookie(ACCESS_TOKEN_NAME);
        const refreshToken = useCookie(REFRESH_TOKEN_NAME);
        accessToken.value = data.value.accessToken;
        refreshToken.value = data.value.refreshToken;

        this.authenticated = true;

        this.getUserData();
      } catch (error) {
        console.error(error);
      }
    },
    getUserData() {
      const {
        public: { ACCESS_TOKEN_NAME },
      } = useRuntimeConfig();

      const token = useCookie(ACCESS_TOKEN_NAME, {
        httpOnly: true,
        secure: true,
        sameSite: 'strict',
      });

      if (!token.value) return;
      const { username, role } = jwtDecode<{ username: string; role: number }>(token.value);
      this.username = username;
      this.userRole = role;
    },
    logUserOut() {
      this.authenticated = false;
      useCookie(useRuntimeConfig().public.ACCESS_TOKEN_NAME).value = null;
      useCookie(useRuntimeConfig().public.REFRESH_TOKEN_NAME).value = null;
    },

    async checkIfTokenIsValid(): Promise<boolean> {
      try {
        await useFetchAuthenticated('/Auth/verify-token');
        return true;
      } catch {
        return false;
      }
    },
  },
});

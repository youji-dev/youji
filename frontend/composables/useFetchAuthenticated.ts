/* eslint-disable no-async-promise-executor */
import type { FetchOptions } from 'ofetch';

const useFetchAuthenticated = <T>(url: string | (() => string), providedOptions?: FetchOptions) => {
  const {
    public: { BACKEND_URL, ACCESS_TOKEN_NAME, REFRESH_TOKEN_NAME },
  } = useRuntimeConfig();
  const localePath = useLocaleRoute();
  const authStore = useAuthStore();

  const accessToken = useCookie(ACCESS_TOKEN_NAME, {
    secure: true,
    sameSite: 'strict',
  });

  const refreshToken = useCookie(REFRESH_TOKEN_NAME, {
    secure: true,
    sameSite: 'strict',
  });

  let refreshPromise: Promise<void> | null = null;

  const refreshAccessToken = async () => {
    if (!refreshPromise) {
      refreshPromise = new Promise<void>(async (resolve, reject) => {
        try {
          const data = await $fetch<{ accessToken: string; refreshToken: string }>(`${BACKEND_URL}/Auth/Refresh`, {
            body: { refreshToken: refreshToken.value },
            method: 'post',
            headers: { 'Content-Type': 'application/json' },
          });

          accessToken.value = data.accessToken;
          refreshToken.value = data.refreshToken;

          resolve();
        } catch (error) {
          reject(error);
        } finally {
          refreshPromise = null;
        }
      });
    }
    return refreshPromise;
  };

  const customFetch = $fetch.create({
    baseURL: BACKEND_URL,
    retry: 1,
    retryStatusCodes: [401],
    retryDelay: 500,
    cache: 'no-cache',

    onRequest({ options }) {
      options.headers = {
        ...providedOptions?.headers,
        Authorization: `Bearer ${accessToken.value}`,
      };
    },
    async onResponseError({ response }) {
      if (response?.status === 401) {
        try {
          await refreshAccessToken();
        } catch {
          authStore.logUserOut();
          navigateTo(localePath('/login')?.fullPath);
        }
      }
    },
  });

  const resolvedUrl = typeof url === 'function' ? url() : url;
  return customFetch<T>(resolvedUrl, providedOptions)
    .then(data => ({ data: { value: data as T }, error: { value: null } }))
    .catch(err => ({ data: { value: null as T | null }, error: { value: err } }));
};

export default useFetchAuthenticated;

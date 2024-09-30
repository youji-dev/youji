import type { UseFetchOptions } from "#app";

const useFetchAuthenticated = (url: string | (() => string), options?: UseFetchOptions<null>) => {
    const {
        public: { BACKEND_URL, ACCESS_TOKEN_NAME, REFRESH_TOKEN_NAME },
    } = useRuntimeConfig();
    const localePath = useLocaleRoute();
    const authStore = useAuthStore();

    const accessToken = useCookie(ACCESS_TOKEN_NAME, {
        secure: true,
        sameSite: "strict",
    });

    const refreshToken = useCookie(REFRESH_TOKEN_NAME, {
        secure: true,
        sameSite: "strict",
    });

    const customFetch = $fetch.create({
        baseURL: BACKEND_URL,
        retry: 1,
        retryStatusCodes: [401],
        retryDelay: 500,

        onRequest({ options }) {
            options.headers = {
                ...options.headers,
                Authorization: `Bearer ${accessToken.value}`,
            };
        },
        async onResponseError({ response }) {
            if (response?.status === 401) {
                try {
                    const { data }: any = await useFetch(
                        `${BACKEND_URL}/Auth/refresh`,
                        {
                            body: {
                                refreshToken: refreshToken.value,
                            },
                            method: "post",
                            headers: { "Content-Type": "application/json" },
                        }
                    );
                    accessToken.value = data.value.accessToken;
                    refreshToken.value = data.value.refreshToken;
                } catch (error) {
                    authStore.logUserOut();
                    navigateTo(localePath("/login")?.fullPath);
                }
            }

        },
    });

    return useFetch(url, { ...options, $fetch: customFetch });
}

export default useFetchAuthenticated;
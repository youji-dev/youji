export default defineNuxtPlugin((nuxtApp) => {
    const {
        public: { BACKEND_URL, ACCESS_TOKEN_NAME, REFRESH_TOKEN_NAME },
      } = useRuntimeConfig();

      const token = useCookie(ACCESS_TOKEN_NAME, { httpOnly: true, secure: true, sameSite: 'strict' });

    const api = $fetch.create({
        baseURL: BACKEND_URL,
        onRequest({request, options, error}) {
            if (token) {
                const headers = options.headers ||= {}
                if (Array.isArray(headers)) {
                    headers.push(['Authorization', `Bearer ${token}`])
                  } else if (headers instanceof Headers) {
                    headers.set('Authorization', `Bearer ${token}`)
                  } else {
                    headers.Authorization = `Bearer ${token}`
                  }
            }
        },
        async onRequestError({response}) {
            if (response?.status === 401) {
                // TODO REFRSH TOKEN, TRY AGAIN IF SUCCESS, REDIRECT TO LOGIN IF FAILURE
                // ONLY RERTY ONCE
            }
        }

    })
    return {
        provide: {
            api
        }
    }
})
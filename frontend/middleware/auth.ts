export default defineNuxtRouteMiddleware(async (to, from) => {
  const { authenticated, name, csrfToken } = storeToRefs(useAuthStore());
  const { $locally } = useNuxtApp();
  const localeRoute = useLocaleRoute();
  const { public: { AUTH_TOKEN_NAME } } = useRuntimeConfig()
  const token = useCookie(AUTH_TOKEN_NAME, {httpOnly: true, secure: true, sameSite: 'strict'});
  const { checkIfTokenIsValid } = useAuthStore()

  if (token.value) {
      if (await checkIfTokenIsValid()) {
        authenticated.value = true;
        name.value = $locally.getItem("username");
      } else {
        authenticated.value = false;
        token.value = null;
      }
  }

  if (token.value && to?.name === 'login') {
      abortNavigation();
      return navigateTo(localeRoute("/")?.fullPath);
  }

  if (!token.value && to?.name !== 'login') {
      abortNavigation();
      return navigateTo(localeRoute("/login")?.fullPath);
  }
})

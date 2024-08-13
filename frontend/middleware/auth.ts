export default defineNuxtRouteMiddleware((to, from) => {
  const { authenticated, name, csrfToken } = storeToRefs(useAuthStore());
  const { $locally } = useNuxtApp();
  const localeRoute = useLocaleRoute();
  const token = useCookie('token');

  if (token.value) {
    // TODO add method to check if token is valid
      authenticated.value = true;
      name.value = $locally.getItem("username");
      csrfToken.value = $locally.getItem("csrfToken");
  }

  if (token.value && to?.name === 'login') {
      return navigateTo(localeRoute("/")?.fullPath);
  }

  if (!token.value && to?.name !== 'login') {
      abortNavigation();
      return navigateTo(localeRoute("/login")?.fullPath);
  }
})

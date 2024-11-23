export default defineNuxtRouteMiddleware(async (to, from) => {
  const { authenticated, name } = storeToRefs(useAuthStore());
  const { $locally } = useNuxtApp();
  const { public: { ACCESS_TOKEN_NAME } } = useRuntimeConfig()
  const token = useCookie(ACCESS_TOKEN_NAME, { httpOnly: true, secure: true, sameSite: 'strict' });
  const localeRoute = useLocaleRoute();
  const { logUserOut } = useAuthStore()
  const { checkIfTokenIsValid } = useAuthStore()
  if (to.fullPath === localeRoute("/login")?.fullPath) {
    return;
  } 
  if (to.fullPath === "/logout") {
    logUserOut();
    return navigateTo(localeRoute("/login")?.fullPath);
  }
  if (to.fullPath === localeRoute("/")?.fullPath + "/" || to.fullPath === localeRoute("/")?.fullPath) {
    return navigateTo(localeRoute("/tickets")?.fullPath);
  }

  if (token.value) {
    if (await checkIfTokenIsValid()) {
      authenticated.value = true;
      name.value = $locally.getItem("username");
    } else {
      authenticated.value = false;
      token.value = null;
    }
  }

  if (!token.value && !to?.name?.toString().startsWith("login")) {
    abortNavigation();
    return navigateTo(localeRoute("/login")?.fullPath);
  }
})

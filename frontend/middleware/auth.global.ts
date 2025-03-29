export default defineNuxtRouteMiddleware(async (to, from) => {
  const { getUserData } = useAuthStore();
  const { authenticated, isUserTeacher } = storeToRefs(useAuthStore());
  const {
    public: { ACCESS_TOKEN_NAME },
  } = useRuntimeConfig();
  const token = useCookie(ACCESS_TOKEN_NAME, {
    httpOnly: true,
    secure: true,
    sameSite: 'strict',
  });
  const localeRoute = useLocaleRoute();
  const { logUserOut } = useAuthStore();
  const { checkIfTokenIsValid } = useAuthStore();
  const currentFullFromPath = from.fullPath.endsWith('/')
    ? from.fullPath.slice(from.fullPath.length, from.fullPath.length)
    : from.fullPath;
  const currentFullToPath = to.fullPath.endsWith('/')
    ? to.fullPath.slice(to.fullPath.length, to.fullPath.length)
    : to.fullPath;
  if (to.fullPath === localeRoute('/login')?.fullPath) {
    return;
  }
  if (to.fullPath === '/logout') {
    logUserOut();
    return navigateTo(localeRoute('/login')?.fullPath);
  }
  if (to.fullPath === localeRoute('/')?.fullPath + '/' || to.fullPath === localeRoute('/')?.fullPath) {
    return navigateTo(localeRoute('/tickets')?.fullPath);
  }

  if (token.value) {
    if (await checkIfTokenIsValid()) {
      authenticated.value = true;
      getUserData();
    } else {
      authenticated.value = false;
      token.value = null;
    }
  }

  if (!token.value && !to?.name?.toString().startsWith('login')) {
    abortNavigation();
    return navigateTo(localeRoute('/login')?.fullPath);
  }
  if (
    currentFullFromPath === localeRoute('/login')?.fullPath &&
    isUserTeacher.value &&
    currentFullToPath !== localeRoute('/tickets/new')?.fullPath
  ) {
    abortNavigation();
    return navigateTo(localeRoute('/tickets/new')?.fullPath);
  }
});

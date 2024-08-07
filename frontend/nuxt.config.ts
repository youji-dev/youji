// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: "2024-04-03",
  devtools: { enabled: true },
  routeRules: {
    '/login': { ssr: false }
  },
  runtimeConfig: {
    public: {
      BACKEND_URL: process.env.BACKEND_BASE_URL,
    },
  },
  modules: [
    "@nuxtjs/tailwindcss",
    "@pinia/nuxt",
    "@nuxtjs/color-mode",
    "@nuxt/icon",
    "@nuxtjs/i18n",
    "@element-plus/nuxt",
  ],
  i18n: {
    strategy: "prefix",
    locales: [
      {
        code: "en",
        name: "English",
      },
      {
        code: "de",
        name: "Deutsch",
      },
    ],
  },
  elementPlus: {
    icon: "ElIcon",
  },
});

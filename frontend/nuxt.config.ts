// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: "2024-04-03",
  devtools: { enabled: true },
  routeRules: {
    "**/login": { ssr: false },
  },
  runtimeConfig: {
    public: {
      BACKEND_URL: process.env.BACKEND_BASE_URL,
      ACCESS_TOKEN_NAME: "youji-access-token",
      REFRESH_TOKEN_NAME: "youji-refresh-token",
      COLORS: {
        ACCENT_COLOR: "#409EFF",
        TEXT_LIGHT: "#303133",
        TEXT_DARK: "#E5EAF3",
        BORDER_DARK: "#636466",
        BORDER_LIGHT: "#E6E8EB",
        FILL_DARK: "#424243",
        FILL_LIGHT: "#E6E8EB",
        BASE_BG_DARK: "#141414",
        BASE_BG_LIGHT: "#FFFFFF",
        PAGE_BG_DARK: "#0A0A0A",
        PAGE_BG_LIGHT: "#F2F3F5"
      }
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
    // Icons from https://www.npmjs.com/package/@iconify-json/flag
    locales: [
      {
        code: "en",
        name: "English",
        icon: "flag:gb-4x3",
      },
      {
        code: "de",
        name: "Deutsch",
        icon: "flag:de-4x3",
      },
    ],
    experimental: {
      localeDetector: "./localeDetector.ts",
    },
  },
  elementPlus: {
    icon: "ElIcon",
    importStyle: "scss",
    themes: ["dark"],
  },
  colorMode: {
    classSuffix: '',
    preference: 'system', // default value of $colorMode.preference
    fallback: 'light', // fallback value if not system preference found
  },
});

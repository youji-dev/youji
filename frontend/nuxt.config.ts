// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: "2024-04-03",
  devtools: { enabled: true },
  sourcemap: {
    server: true,
    client: true,
  },
  app: {
    head: {
      title:
        "Youji - Hausmeister Ticketing System der Industrieschule Chemnitz",
      link: [
        {
          rel: "apple-touch-icon",
          sizes: "180x180",
          href: "/apple-touch-icon.png",
        },
        {
          rel: "icon",
          type: "image/png",
          sizes: "32x32",
          href: "/favicon-32x32.png",
        },
        {
          rel: "icon",
          type: "image/png",
          sizes: "16x16",
          href: "/favicon-16x16.png",
        },
        { rel: "manifest", href: "/site.webmanifest" },
      ],
    },
  },
  routeRules: {
    "**/login": { ssr: false },
  },
  runtimeConfig: {
    public: {
      BACKEND_URL: "", // create a .env file with NUXT_PUBLIC_BACKEND_URL=<your backend url> to override this
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
        PAGE_BG_LIGHT: "#F2F3F5",
      },
    },
  },
  modules: [
    "@nuxtjs/tailwindcss",
    "@pinia/nuxt",
    "@nuxtjs/color-mode",
    "@nuxt/icon",
    "@nuxtjs/i18n",
    "@element-plus/nuxt",
    "@unlazy/nuxt",
  ],
  i18n: {
    strategy: "prefix",
    // Icons from https://www.npmjs.com/package/@iconify-json/flag
    locales: [
      {
        code: "en-EN",
        name: "English",
        icon: "flag:gb-4x3",
      },
      {
        code: "de-DE",
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
    classSuffix: "",
    preference: "system", // default value of $colorMode.preference
    fallback: "light", // fallback value if not system preference found
  },
});

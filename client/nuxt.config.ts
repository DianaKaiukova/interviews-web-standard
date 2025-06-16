export default defineNuxtConfig({
  modules: ['@nuxtjs/tailwindcss'],
  runtimeConfig: {
    public: {
      apiBase: process.env.API_BASE || 'http://localhost:5221'
    }
  },
  nitro: {
    compatibilityDate: '2025-06-17'
  }
})
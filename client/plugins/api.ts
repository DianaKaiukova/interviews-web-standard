// Nuxt 3 plugin to provide a simple API client using ofetch

import { $fetch } from 'ofetch'

// This plugin provides a simple API client that can be used throughout the Nuxt application.
export default defineNuxtPlugin(() => {
  // Access the runtime configuration to get the API base URL
  const config = useRuntimeConfig()

  const fetchOptions = {
    timeout: 5000,
    retry: 2,
    retryDelay: 1000
  }

  // Set global fetch options
  const apiBase = config.public.apiBase
  
  // Create an API client with methods for GET, POST, PUT, and DELETE requests
  const api = {
    async get(endpoint: string) {
      return $fetch(`${apiBase}/api${endpoint}`)
    },
    async post(endpoint: string, body: any) {
      return $fetch(`${apiBase}/api${endpoint}`, {
        method: 'POST',
        body
      })
    },
    async put(endpoint: string, body: any) {
      return $fetch(`${apiBase}/api${endpoint}`, {
        method: 'PUT',
        body
      })
    },
    async delete(endpoint: string) {
      return $fetch(`${apiBase}/api${endpoint}`, {
        method: 'DELETE'
      })
    }
  }

  // Provide the API client to the Nuxt contextS
  return {
    provide: { api }
  }
})
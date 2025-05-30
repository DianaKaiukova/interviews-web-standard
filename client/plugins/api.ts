import { $fetch } from 'ofetch'

export default defineNuxtPlugin(() => {
  const config = useRuntimeConfig()

  const fetchOptions = {
    timeout: 5000,
    retry: 2,
    retryDelay: 1000
  }

  const apiBase = config.public.apiBase
  
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
  
  return {
    provide: { api }
  }
})
import { defineStore } from 'pinia'
import { login, register } from '../utils/auth'
import type { LoginPayload } from '../types/auth'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || '',
    user: localStorage.getItem('user') || null, // Load from localStorage on refresh
    error: null as string | null,
  }),
  actions: {
    async login(payload: LoginPayload) {
      try {
        const data = await login(payload)
        if (data.success && data.token) {
          this.token = data.token
          this.user =  payload.username
          this.error = null
          localStorage.setItem('token', data.token)
          localStorage.setItem('user', this.user)
        } else {
          this.error = data.error || 'Login failed'
        }
      } catch (err: any) {
        this.error = err.response?.data?.error || 'An error occurred'
      }
    },

    async register(payload: LoginPayload) {
      try {
        const data = await register(payload)
        if (data.success && data.token) {
          this.token = data.token
          this.user =  payload.username
          this.error = null
          localStorage.setItem('token', data.token)
          localStorage.setItem('user', this.user)
        } else {
          this.error = data.error || 'Registration failed'
        }
      } catch (err: any) {
        this.error = err.response?.data?.error || 'An error occurred during registration'
      }
    },

    logout() {
      this.token = ''
      this.user = null
      this.error = null
      localStorage.removeItem('token')
      localStorage.removeItem('user')
    }
  },
})

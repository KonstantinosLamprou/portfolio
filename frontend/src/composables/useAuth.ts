import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { loginUser } from '../services/AuthService'
import type { LoginCredentials } from '@/types/auth'

export function useAuth() {
  const isLoading = ref(false)
  const error = ref<string | null>(null)
  const router = useRouter()

  async function login(credentials: LoginCredentials) {
    isLoading.value = true
    error.value = null

    try {
      const response = await loginUser(credentials)
      // Token speichern (z.B. localStorage oder Pinia)
      localStorage.setItem('authToken', response.token)
      // Weiterleitung
      router.push('/dashboard')
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Anmeldung fehlgeschlagen'
    } finally {
      isLoading.value = false
    }
  }

  return { login, isLoading, error }
}

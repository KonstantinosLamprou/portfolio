import apiClient from '@/services/api'
import { useQuery } from '@tanstack/vue-query'

// Definiere die Struktur deines Session/User-Objekts
export interface UserSession {
  userId: string
  name: string
  email: string
  provider?: string
}

// Mock-Funktion für den API-Call (Ersetze dies mit deinem echten API-Client/Axios/Fetch)
const fetchSession = async (): Promise<UserSession | null> => {
  try {
    const response = await apiClient.get('/auth/session')
    
    return response.data.user
  } catch (error: any) {
      // 401 bedeutet einfach nur "Nicht eingeloggt", kein Grund zur Panik
      if (error.response?.status === 401) {
          return null 
      }
      console.error('Session konnte nicht geladen werden', error)
      return null
    }
}

export function useSession() {
  return useQuery({
    queryKey: ['session'],
    queryFn: fetchSession,
    // Optional: Konfiguriere das Caching
    staleTime: 1000 * 60 * 5, // Daten bleiben 5 Minuten "frisch"
    retry: false, // Bei 401 nicht endlos neu versuchen
  })
}
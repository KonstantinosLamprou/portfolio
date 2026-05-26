// src/services/authService.ts
import apiClient from './api'
import type {  AuthResponse, UserProfile } from '@/types/auth'

// OAuth Code an das Backend schicken
export async function exchangeOAuthCode(provider: string, code: string): Promise<AuthResponse> {
  // code wird in sauberes Json gepackt und an den Backend-Endpunkt geschickt
  const response = await apiClient.post<AuthResponse>(`auth/${provider}/callback`, { code })
  return response.data
}

// Aktuelles Profil abrufen (z.B. nach einem Seiten-Reload)
export async function fetchProfile(): Promise<UserProfile | null> {
  try {
    const response = await apiClient.get<UserProfile>('auth/me')
    return response.data
  } catch (error) {

    return null;
  }
}

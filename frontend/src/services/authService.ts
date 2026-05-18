import apiClient from './api'
import type { LoginCredentials, AuthResponse } from '@/types/auth'

export async function loginUser(credentials: LoginCredentials): Promise<AuthResponse> {
  const response = await apiClient.post<AuthResponse>('admin/login', credentials)
  return response.data
}

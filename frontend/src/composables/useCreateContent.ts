import { useMutation } from '@tanstack/vue-query'
import { type CreateBlogRequest } from '@/types/blogTypes.ts'
import  apiClient from '@/services/api'
import { isAxiosError } from 'axios'

export function useCreateContentblogs() {
  return useMutation({
    mutationFn: async (newPost: CreateBlogRequest) => {
      try {
      const response = await apiClient.post('/blogs', newPost)
      
      return response.data

      } catch (error) {

        if (isAxiosError(error)) {

          const serverMessage = error.response?.data?.message 
          throw new Error(serverMessage || 'Fehler beim Speichern. Bist du richtig eingeloggt?')
        }
    
        throw new Error('Ein unerwarteter Fehler ist aufgetreten.')
      }
    }
  })
}

export function useCreateContentprojects() {
  return useMutation({
    mutationFn: async (newPost: CreateBlogRequest) => {
      try {
      const response = await apiClient.post('/projects', newPost)
      
      return response.data

      } catch (error) {

        if (isAxiosError(error)) {

          const serverMessage = error.response?.data?.message 
          throw new Error(serverMessage || 'Fehler beim Speichern. Bist du richtig eingeloggt?')
        }
    
        throw new Error('Ein unerwarteter Fehler ist aufgetreten.')
      }
    }
  })
}

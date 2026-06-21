import type { StatisticsResponse, UpdateStatisticsRequest } from '@/types/statistics';
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query';
import apiClient from '@/services/api.ts';
import { isAxiosError } from 'axios';

const fetchStatistics = async (): Promise<StatisticsResponse> => {
  try {
    const response = await apiClient.get('/statistics');
    return response.data;
  } catch (error) {
    if (isAxiosError(error)) {
      console.error('Fehler beim Abrufen der Statistiken:', error.response?.data || error.message);
    } else {
      console.error('Unerwarteter Fehler:', error);
    }
    throw error; 
  }
};

export function useStatistics() {
    return useQuery({
        queryKey: ['statistics'],
        queryFn: fetchStatistics,
        staleTime: 1000 * 60 * 5, // Daten bleiben 5 Minuten "frisch"
        retry: false, // Bei Fehlern nicht endlos neu versuchen
  });
}

export function useUpdateStatisticsMutation() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: async (updateData: UpdateStatisticsRequest) => {
      const response = await apiClient.patch('/statistics', updateData);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['statistics'] });
      console.log('Statistiken erfolgreich aktualisiert!');
    }, 
    onError: (error: any) => {
      if (isAxiosError(error)) {
        console.error('Fehler beim Aktualisieren der Statistiken:', error.response?.data || error.message);
      } else {
        console.error('Unerwarteter Fehler:', error);
      }
    }
  });
}

import { useMutation, useQueryClient } from '@tanstack/vue-query';
import apiClient from '@/services/api';

export function useUpdateCommentMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async ({ commentId, text, contentId }: { commentId: string, text: string, contentId: number }) => {
      const response = await apiClient.patch(`/comments/${commentId}?${contentId}`, { 
        commentId: commentId, 
        text: text 
      });
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['comments'] });

    },
    onError: (error: any) => {
      if (error.response?.status === 403) {
        console.error("Keine Berechtigung zum Bearbeiten!");
      } else {
        console.error("Fehler beim Aktualisieren", error);
      }
    }
  });
}

export function useDeleteCommentMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async ({commentId, contentId}: { commentId: string, contentId: number }) => {
      await apiClient.delete(`/comments/${commentId}?${contentId}`);

    },
    onSuccess: () => {
        queryClient.invalidateQueries({ queryKey: ['comments'] });
    },
    onError: (error: any) => {
      if (error.response?.status === 403) {
        console.error("Keine Berechtigung zum Löschen!");
      } else {
        console.error("Fehler beim Löschen", error);
      }
    }
  });
}
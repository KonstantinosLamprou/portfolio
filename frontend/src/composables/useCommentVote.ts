import { useMutation, useQueryClient } from '@tanstack/vue-query';
import apiClient from '@/services/api';

export function useUpdateCommentVoteMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async ({ commentId, isUpvote, contentId }: { commentId: string, isUpvote: boolean, contentId: number }) => {
        const response = await apiClient.patch(`/commentvote/${commentId}`, {
            commentId: commentId,
            isUpvote: isUpvote, 
            contentId: contentId 
        });
        return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['comments'] });
    },
    onError: (error: any) => {
      if (error.response?.status === 403) {
        console.error("Keine Berechtigung zum Abstimmen!");
      } else {
        console.error("Fehler beim Abstimmen", error);
      }
    }
  });
}

export function useCreateCommentVoteMutation() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: async ({ commentId, isUpvote, contentId }: { commentId: string, isUpvote: boolean, contentId: number }) => {
      const response = await apiClient.post(`/commentvote`, {
        commentId: commentId, 
        isUpvote: isUpvote,
        contentId: contentId
      });
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['comments'] });
    },
    onError: (error: any) => {
      if (error.response?.status === 403) {
        console.error("Keine Berechtigung zum Abstimmen!");
      } else {
        console.error("Fehler beim Abstimmen", error);
      }
    }
  });
}


export function useDeleteCommentVoteMutation() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: async ({commentId, contentId}: {commentId: string, contentId: number}) => {
      const response = await apiClient.delete(`/commentvote/${commentId}`, { 
         data: { contentId: contentId } 
      });
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['comments'] });
    },
    onError: (error: any) => {
      console.error("Fehler beim Entfernen der Abstimmung", error);
    }
  });
}
import { useMutation, useQueryClient } from '@tanstack/vue-query';
import axios, { AxiosError } from 'axios';
import apiClient from '@/services/api';



interface LogoutResponse {
    message: string;
}

export function useLogout() {
    const queryClient = useQueryClient();

    return useMutation<LogoutResponse, AxiosError, void>({
        
        mutationFn: async () => {
            const response = await apiClient.post<LogoutResponse>('/auth/logout');

            return response.data;
        },
        onSuccess: (data) => {
            console.log(data.message); 
            queryClient.setQueryData(['session'], null);

        },
        onError: (error) => {
            console.error('Fehler beim Logout:', error.message);
        }
    });
}
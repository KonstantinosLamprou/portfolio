import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query';
import apiClient from '@/services/api.ts';
import { isAxiosError } from 'axios';
import type { CreateGuestbookEntryRequest, UserGuestbookEntryResponse } from '@/types/GuestbookTypes';


export const useCreateGuestbookEntry = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (newEntry: CreateGuestbookEntryRequest) => {
                const { data } = await apiClient.post('/guestbook', newEntry);
                return data;
            },
        onSuccess: () => {
            console.log('Guestbook-Eintrag erfolgreich erstellt');


            queryClient.invalidateQueries({ queryKey: ['entries'] });    
        },
        onError: (error) => {
            if (isAxiosError(error)) {
                if (error.response?.status === 403) {
                    console.error('Nicht autorisiert, um einen Guestbook-Eintrag zu erstellen.');
                } else {
                    console.error('Fehler beim Erstellen des Eintrags:', error);
                }
            } else {
                console.error('Unerwarteter Fehler:', error);
            }
        },
    });
};

export const useGetGuestbookEntries = () => useQuery({
  queryKey: ['entries'],
  queryFn: async (): Promise<UserGuestbookEntryResponse[]> => {
    const { data } = await apiClient.get('/guestbook');

    return data.map((entry: any) => ({
        ...entry,
        createdAt: new Date(entry.createdAt)
       })
    );
   }
});



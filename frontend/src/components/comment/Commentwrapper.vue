<template>
  <div class="w-full mx-auto p-4 bg-main"> 
    <div>
      <div v-if="isPending" class="flex justify-center py-10">
        <p class="text-gray-400">Lade Kommentare...</p>
      </div>

      <div v-else-if="isError" class="text-center py-10 text-red-500">
        <p>Kommentare konnten nicht geladen werden.</p>
      </div>
      <div v-else>
        <CommentHeader 
          :totalComments="comments?.length || 0" 
          :totalReplies="totalReplies" 
          :sort-order="currentSort"
          @update-sort="currentSort = $event"
        />
  
        <div class="mt-6">
          <div v-if="!comments || comments.length === 0" class="text-center py-12 border border-dashed border-gray-800 rounded-xl">
            <p class="text-gray-500">Noch keine Kommentare vorhanden. Mach den Anfang!</p>
          </div>
          <CommentSection 
            v-else
            v-for="comment in sortedComments" 
            :key="comment.id" 
            :comment="comment"
            @reply-added="onReplyAdded"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';
import { useQuery } from '@tanstack/vue-query';

import apiClient from '@/services/api.ts';
import CommentHeader from './CommentHeaderSection.vue';
import CommentSection from './CommentSection.vue';
import type { CommentResponseDto } from '@/types/comment.ts';

const props = defineProps<{
  blogId: number
}>()

export interface CommentResponseDtoExtended extends Omit<CommentResponseDto, 'createdAt'> {
  createdAt: Date; 
}

const currentSort = ref<'newest' | 'oldest'>('newest');

const fetchAllTopLevelComments = async (): Promise<CommentResponseDtoExtended[]> => {

  const { data } = await apiClient.get<CommentResponseDto[]>(
    `/comments?contentId=${props.blogId}&contentType=blog`
  );
  
  return data.map((comment: any) => ({
    ...comment,
    createdAt: new Date(comment.createdAt)
  }));
};

const { 
  data: comments, 
  isPending, 
  isError 
} = useQuery({ 
  // Die contentId MUSS in den Query-Key, damit verschiedene Blogposts eigene Caches haben
  queryKey: ['comments', props.blogId], 
  queryFn: fetchAllTopLevelComments 
});

const sortedComments = computed(() => {
  if (!comments.value) return [];

  // [...comments.value] erstellt eine flache Kopie, damit der TanStack Cache heile bleibt
  return [...comments.value].sort((a, b) => {
    // Da wir vorhin echte Date-Objekte daraus gemacht haben, können wir getTime() nutzen
    const timeA = a.createdAt.getTime();
    const timeB = b.createdAt.getTime();

    if (currentSort.value === 'newest') {
      return timeB - timeA; // Descending (Neueste zuerst)
    } else {
      return timeA - timeB; // Ascending (Älteste zuerst)
    }
  });
});

// Berechnet alle Replies aus der ersten Ebene für den Header
const totalReplies = computed(() => {
  return comments.value?.reduce((acc, curr) => acc + (curr.replies?.length || 0), 0) || 0;
});

const onReplyAdded = (payload: { parentId: string, text: string }) => {
  console.log('Neuer Reply an Backend senden:', payload);
  // Logik um den Mock-State zu updaten, bis das Backend angebunden ist
};

</script>
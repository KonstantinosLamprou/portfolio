<template>
  <div v-if="isPending" class="text-center py-20 text-lg text-muted-foreground">
    Lade Blogpost...
  </div>

  <div v-else-if="currentPost">
    <BackButton v-show="!dialogState.open" class="hidden md:flex"/>

    <header class="text-center max-w-3xl mx-auto px-4 pt-16">
      <h1 class="text-5xl font-extrabold text-slate-600 mb-12">{{ currentPost.title }}</h1>
      <div class="grid grid-cols-2 text-sm max-md:gap-4 md:grid-cols-4">
        <div class='space-y-1 md:mx-auto'>
          <div class="text-muted-foreground">geschrieben von</div>
          <p class="flex items-center gap-2 justify-center">{{ currentPost.author.name }}</p>
        </div>
        <div class="space-y-1 md:mx-auto">
          <div class="text-muted-foreground">veröffentlicht am</div>
          <p class="flex items-center gap-2 justify-center">
            <time :datetime="currentPost.dateOfCreation.toISOString()">
              {{ formatDate(currentPost.dateOfCreation) }}
            </time>
          </p>
        </div>
        <div class="space-y-1 md:mx-auto">
          <div class="text-muted-foreground">Views</div>
          <p class="flex items-center gap-2 justify-center">{{ currentPost.views }}</p>
        </div>
        <div class="space-y-1 md:mx-auto">
          <div class="text-muted-foreground">Likes</div>
          <p class="flex items-center gap-2 justify-center">{{ currentPost.likesCount }}</p>
        </div>
      </div>
    </header>

    <div>
      <img 
        :src="currentPost.imgSrc" 
        :alt="currentPost.title" 
        class="w-full h-auto object-cover mt-8 rounded-lg shadow-md"
      />
    </div>

    <div class="mt-8 flex flex-col justify-between lg:flex-row items-start">
      <ContentRenderer class="w-full lg:max-w-2xl" :blocks="currentPost.content" />
      
      <aside class="w-full lg:w-68 px-4 lg:py-10 sticky top-34">
        <TableOfContents :toc="tableOfContents" />
        <LikeButton 
          :ContentId = "currentPost.id"
          :slug="currentPost.slug" 
          :initialLikes="currentPost.likesCount" 
          :currentUserLikes="currentPost.currentUserLikeCount"
          :contentType="'blog'" />
      </aside>
    </div>

    <div class="space-y-6 mt-12">
      <CommentPost
        :contentId="currentPost.id"
        :contentType="'blog'"
      />
      <CommentWrapper 
        :contentId="currentPost.id"
        :contentType="'blog'"
      /> 
    </div>
  </div>

  <div v-else-if="isError" class="text-center py-20 text-red-500">
    <h2>Diesen Blogpost gibt es leider nicht. (Oder er konnte nicht geladen werden)</h2>
  </div>
</template>

<script setup lang="ts">
import { computed, watch } from 'vue';
import { useRoute } from 'vue-router';
import { useQuery, useMutation, useQueryClient } from '@tanstack/vue-query';
import { toast } from 'vue-sonner';
import { isAxiosError } from 'axios';

import apiClient from '@/services/api.ts';
import { useSignInDialogStore } from "@/stores/useSignInDialogStore"
import ContentRenderer from '@/components/content/ContentRenderer.vue';
import BackButton from '@/components/ui/buttons/BackButton.vue';
import TableOfContents from '@/components/content/TableOfContents.vue';
import LikeButton from '@/components/content/LikeButton.vue';
import CommentPost from "@/components/comment/CommentPost.vue"
import CommentWrapper from "@/components/comment/Commentwrapper.vue"
import { formatDate } from '@/helper/DateHelper.ts';

import type { BlogApiResponse } from '@/types/blogTypes.ts';

const queryClient = useQueryClient();
const route = useRoute();
const slug = computed(() => route.params.slug as string);
const dialogState = useSignInDialogStore(); 

interface BlogDetailData extends Omit<BlogApiResponse, 'dateOfCreation'> {
  dateOfCreation: Date; // Im Frontend als Date
}

const fetchBlogDetails = async (): Promise<BlogDetailData> => {
  const { data } = await apiClient.get<BlogApiResponse>(`blogs/${slug.value}`);
  
  return {
    ...data,
    dateOfCreation: new Date(data.dateOfCreation)
  };
};

// TanStack Query Hook
const { 
  data: currentPost, 
  isPending, 
  isError, 
  isSuccess,
  error 
} = useQuery({
  queryKey: ['blog', slug.value],
  queryFn: fetchBlogDetails,
  retry: false 
});

// Fehler-Toast
watch(isError, (hasError) => {
  if (hasError && error.value) {
   if (isAxiosError(error.value) && error.value.response?.status === 404) {
       toast.error('Dieser Blog-Beitrag existiert nicht.');
    } else {
       toast.error('Fehler beim Laden des Beitrags.');
    }
  }
});

const tableOfContents = computed(() => {
  if (!currentPost.value) return [];
  
  return currentPost.value.content
    .filter(block => block.type === 'heading')
    .map(heading => ({
      url: heading.id,
      title: heading.data.text,
      depth: heading.data.level
    }));
});

// Views 
// TODO: Auslagern 
interface UpdateViewsContentResponse {
  id: number;
  views: number;
}

const { mutate: incrementView } = useMutation({
  mutationFn: async (contentId: number): Promise<UpdateViewsContentResponse> => {
    const { data } = await apiClient.patch(`/blogs/${contentId}/view?slug=${slug.value}`);

    return data;
  }, 

  onSuccess: (data) => {
    queryClient.setQueryData(['blog', slug.value], (old: any) => {

      if (!old) return old;

      return {
        ...old,
        views: data.views
      }
    });
  }

});

watch(isSuccess, (success) => {
  if (success && currentPost.value) {
    const contentId = currentPost.value.id;
    const storageKey = `viewed_post_${contentId}`;

    if (!sessionStorage.getItem(storageKey)) {
      incrementView(contentId);
      sessionStorage.setItem(storageKey, 'true');
    }
  }
});

</script>
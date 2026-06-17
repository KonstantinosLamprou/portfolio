<template>
  <div v-if="isPending" class="text-center py-20 text-lg text-muted-foreground">
    Lade Projekt...
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
        <LikeButton :slug="currentPost.slug" :initial-likes="currentPost.likesCount" :user-liked="currentPost.currentUserLikeCount" />
      </aside>
    </div>

    <div class="space-y-6 mt-12">
      <CommentPost />
      <CommentWrapper /> 
    </div>
  </div>

  <div v-else-if="isError" class="text-center py-20 text-red-500">
    <h2>Diesen Projekt-Beitrag gibt es leider nicht. (Oder er konnte nicht geladen werden)</h2>
  </div>
</template>

<script setup lang="ts">
import { computed, watch } from 'vue';
import { useRoute } from 'vue-router';
import { useQuery, useMutation, useQueryClient } from '@tanstack/vue-query';
import apiClient from '@/services/api.ts';
import { toast } from 'vue-sonner';
import { isAxiosError } from 'axios';
import { useSignInDialogStore } from "@/stores/useSignInDialogStore"

import ContentRenderer from '@/components/content/ContentRenderer.vue';
import BackButton from '@/components/ui/buttons/BackButton.vue';
import TableOfContents from '@/components/content/TableOfContents.vue';
import LikeButton from '@/components/content/LikeButton.vue';
import CommentPost from "@/components/comment/CommentPost.vue"
import CommentWrapper from "@/components/comment/Commentwrapper.vue"

const queryClient = useQueryClient();
const route = useRoute();
const slug = route.params.slug as string;
const dialogState = useSignInDialogStore(); 

//Auslagern 

interface AuthorDto {
  id: string | number;
  name: string;
  profilePictureUrl?: string;
  role?: string;
}

interface ContentBlock {
  id: string;
  type: string;
  data: any; 
}

interface ProjectApiResponse {
  id: number;
  title: string;
  slug: string;
  dateOfCreation: string; // Von API als String
  imgSrc: string;
  description: string;
  content: ContentBlock[];
  views: number;
  likesCount: number;
  commentsCount: number;
  currentUserLikeCount: number;
  author: AuthorDto;
}

interface ProjectDetailData extends Omit<ProjectApiResponse, 'dateOfCreation'> {
  dateOfCreation: Date; // Im Frontend als Date
}

// Fetch Funktion für TanStack Query
const fetchProjectDetails = async (): Promise<ProjectDetailData> => {
  // Wir feuern den Request an deinen aktualisierten Endpoint (mit Slug)
  const { data } = await apiClient.get<ProjectApiResponse>(`projects/${slug}`);
  
  // Datums-Transformation
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
  queryKey: ['projects', slug],
  queryFn: fetchProjectDetails,
  retry: false 
});

watch(isError, (hasError) => {
  if (hasError && error.value) {
   if (isAxiosError(error.value) && error.value.response?.status === 404) {
       toast.error('Dieser Projekt-Beitrag existiert nicht.');
    } else {
       toast.error('Fehler beim Laden des Beitrags.');
    }
  }
});

// Auslagern 
const formatDate = (date: Date) => {
  return date.toLocaleDateString('de-DE');
};

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

interface UpdateViewsContentResponse {
  id: number;
  views: number;
}

const { mutate: incrementView } = useMutation({
  mutationFn: async (contentId: number): Promise<UpdateViewsContentResponse> => {
    const { data } = await apiClient.patch(`/blogs/${contentId}/view`);

    return data;
  }, 

  onSuccess: (data) => {
    queryClient.setQueryData(['blog', slug], (old: any) => {

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

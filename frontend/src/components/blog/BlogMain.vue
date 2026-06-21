<template>
  <div class="container mt-25 divide-surface divide-y">

    <div class="space-y-2 pt-6 pb-8 md:space-y-5">
      <h2 class="text-4xl leading-9 font-extrabold tracking-tight sm:text-5xl sm:leading-10 md:text-6xl md:leading-14">
        Blog
      </h2>
      <p class="text-sm md:text-lg leading-5 md:leading-7 opacity-80">
        Hier sind ein paar Blogs von mir.
        Checke hier mein <RouterLink to="https://github.com/KonstantinosLamprou" class="underline text-sky">Github</RouterLink> um den Code zu sehen und lass mich wissen, was man verbessern kann.
      </p>
    </div>

    <div class="pt-8 container">
      <div v-if="isPending" class="flex gap-2 items-center justify-center text-center text-lg text-muted-foreground">
        <label>Lade Blogs...</label>
        <Spinner />
      </div>
      <div v-else-if="isError" class="text-center text-red-500">
        Blogs konnten leider nicht geladen werden.
      </div>

      <div v-else-if="blogs && blogs.length > 0" class="grid grid-cols-1 md:grid-cols-2 gap-8">
        <ContentCard v-for="data in blogs" :key="data.id"
          :id="data.id"
          :title="data.title"
          :slug="data.slug"
          :date="formatDate(data.dateOfCreation)" 
          :description="data.description"
          :content="data.content"
          :imgSrc="data.imgSrc"
          :likes="data.likesCount"
          :views="data.views"
          :comments="data.commentsCount" 
        />
      </div>
      <div v-else class="text-center text-lg text-muted-foreground">
        Keine Blogs gefunden.
      </div>
          
    </div>


  </div>
</template>

<script setup lang="ts">
import { watch } from 'vue';
import { useQuery } from '@tanstack/vue-query';
import { toast } from 'vue-sonner'
import { RouterLink } from 'vue-router';
import ContentCard from '../content/ContentCard.vue';
import apiClient from '@/services/api.ts';
import Spinner from '@/components/ui/loaders/SpinnerLoader.vue';
import type { BlogApiResponse } from '@/types/blogTypes.ts';
import { formatDate } from '@/helper/DateHelper.ts';

//TODO:
// neue ladeanimation 
// wenn keine Blogs vorhanden sind, was cooles anzeigen 

interface BlogData extends Omit<BlogApiResponse, 'dateOfCreation'> {
  dateOfCreation: Date; 
}

const fetchBlogs = async (): Promise<BlogData[]> => {

  const { data } = await apiClient.get<BlogApiResponse[]>('blogs');

  return data.map(blog => ({
    ...blog,
    dateOfCreation: new Date(blog.dateOfCreation) 
  }));
};

const { 
  data: blogs, 
  isPending,   
  isError, 
  error 
} = useQuery({
  queryKey: ['blogs'], 
  queryFn: fetchBlogs,
  retry: 1
});

watch(isError, (hasError) => {
  if (hasError && error.value) {
    toast.error(`Fehler beim Laden: ${error.value.message || 'Server nicht erreichbar'}`);
  }
});

</script>
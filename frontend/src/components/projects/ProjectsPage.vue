<template>
<div class="container mt-25 divide-surface divide-y">

    <div class="space-y-2 pt-6 pb-8 md:space-y-5">
      <h2 class="text-4xl leading-9 font-extrabold tracking-tight sm:text-5xl sm:leading-10 md:text-6xl md:leading-14">
        Projekte
      </h2>
      <p class="text-sm md:text-lg leading-5 md:leading-7 opacity-80">
          Hier sind ein paar Projekte von mir.
          Checke hier mein <RouterLink to="https://github.com/KonstantinosLamprou" class="underline text-sky">Github</RouterLink> um den Code zu sehen und lass mich wissen, was man verbessern kann.
      </p>
    </div>

    <div class="pt-8 container">
        
        <div v-if="isPending" class="flex gap-2 items-center justify-center text-center text-lg text-muted-foreground">
          <label>Lade Projekte...</label>
          <Spinner />
        </div>

        <div v-else-if="isError" class="text-center text-red-500">
          Projekte konnten leider nicht geladen werden.
        </div>

        <div v-else-if="projects && projects.length > 0" class="grid grid-cols-1 md:grid-cols-2 gap-8">
          <ContentCard v-for="data in projects" :key="data.id"
            :id="data.id"
            type="project"
            :title="data.title"
            :author="data.author"
            :date="formatDate(data.dateOfCreation)"
            :description="data.description"
            :content="data.content"
            :slug="data.slug"
            :imgSrc="data.imgSrc"
            :likes="data.likesCount"
            :views="data.views"
          />
        </div>

        <div v-else class="text-center text-lg text-muted-foreground">
          Keine Projekte gefunden.
        </div>
        
    </div>
</div>
</template>

<script setup lang="ts">
import { watch } from 'vue';
import { RouterLink } from 'vue-router';
import { useQuery } from '@tanstack/vue-query';
import { toast } from 'vue-sonner'
import ContentCard from '../content/ContentCard.vue';
import apiClient from '@/services/api.ts';
import Spinner from '@/components/ui/loaders/SpinnerLoader.vue'
import { formatDate } from '@/helper/DateHelper.ts';

//TODO:
// neue ladeanimation 
// wenn keine Blogs vorhanden sind, was cooles anzeigen 


interface ProjectApiResponse {
  id: number;
  contenttype?: 'project';
  title: string;
  author: string;
  slug: string;
  dateOfCreation: string; 
  description: string;
  content: object;
  imgSrc: string;
  likesCount?: number; // likes ist keine Num
  views?: number;
  commentsCount?: number; 
}


interface ProjectData extends Omit<ProjectApiResponse, 'dateOfCreation'> {
  dateOfCreation: Date; 
}

const fetchProjects = async (): Promise<ProjectData[]> => {

  const { data } = await apiClient.get<ProjectApiResponse[]>('projects');
  
  return data.map(project => ({
      ...project,
      dateOfCreation: new Date(project.dateOfCreation) 
    }));
};

const { 
  data: projects,
  isPending,  
  isError, 
  error 
} = useQuery({
  queryKey: ['projects'], 
  queryFn: fetchProjects,
  retry: 1 
});

watch(isError, (hasError) => {
  if (hasError && error.value) {
    toast.error(`Fehler beim Laden: ${error.value.message || 'Server nicht erreichbar'}`);
  }
});

</script>

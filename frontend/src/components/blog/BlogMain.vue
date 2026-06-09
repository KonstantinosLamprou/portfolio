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

      <div v-else class="grid grid-cols-1 md:grid-cols-2 gap-8">
        <ContentCard v-for="data in blogs" :key="data.id"
          :id="data.id"
          :type="data.contenttype"
          :title="data.title"
          :author="data.author"
          :slug="data.slug"
          :date="formatDate(data.dateOfCreation)" 
          :description="data.description"
          :content="data.content"
          :imgSrc="data.imgSrc"
          :likes="data.likesCount"
          :views="data.views"
          :comments="data.commentsCount" 
          :comment="data.comment"
        />
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
import Spinner from '@/components/ui/loaders/SpinnerLoader.vue';


// 1. Typ für die rohen Daten, die vom C# Backend kommen
interface BlogApiResponse {
  id: string;
  contenttype?: 'blog' | 'project';
  title: string;
  author: string;
  slug: string;
  dateOfCreation: string; // Von der API kommt es immer als String (ISO-Format)
  description: string;
  content: object;
  imgSrc: string;
  likesCount?: number; // likes ist keine Num
  views?: number;
  commentsCount?: number; //deprecated? 
  comment?: string; // deprecated?
}

// 2. Typ für unser lokales Frontend (nach unserer Transformation)
// Omit kopiert das API-Interface, aber wirft das alte 'date' raus, 
// damit wir es mit dem echten Date-Objekt überschreiben können.
interface BlogData extends Omit<BlogApiResponse, 'dateOfCreation'> {
  dateOfCreation: Date; // Hier ist es jetzt garantiert ein Date!
}

// 1. Definiere die Fetch-Funktion mit Axios
const fetchBlogs = async (): Promise<BlogData[]> => {
  // Axios wirft automatisch einen Fehler bei Status-Codes wie 404 oder 500,
  // wir brauchen also kein manuelles "if (!response.ok)" mehr.
  const { data } = await apiClient.get<BlogApiResponse[]>('blogs');

  // Datums-Mapping
  return data.map(blog => ({
    ...blog,
    dateOfCreation: new Date(blog.dateOfCreation) 
  }));
};

// 2. Nutze TanStack Query
const { 
  data: blogs, // Wir benennen 'data' in 'blogs' um für eine bessere Lesbarkeit im Template
  isPending,   // isPending ist true, während der erste Fetch läuft (Vue Query v5)
  isError, 
  error 
} = useQuery({
  queryKey: ['blogs'], // Eindeutiger Key für den Cache
  queryFn: fetchBlogs,
  retry: 1 // Optional: Versucht es bei einem Fehler noch 1x erneut
});

// 3. Fehlerbehandlung mit Toast
// Wir überwachen isError. Sobald es auf true springt, triggern wir den Toast.
watch(isError, (hasError) => {
  if (hasError && error.value) {
    // Gibt die genaue Axios-Fehlermeldung im Toast aus (oder eine generische Message)
    toast.error(`Fehler beim Laden: ${error.value.message || 'Server nicht erreichbar'}`);
  }
});

const formatDate = (date: Date) => {
  return date.toLocaleDateString('de-DE');
};

</script>
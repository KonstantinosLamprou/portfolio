<template>
  <div v-if="currentPost">
    <BackButton class="hidden md:flex"/>

    <header class="max-w-3xl mx-auto px-4 pt-16 text-center">
      <h1 class="text-4xl font-extrabold text-slate-900 mb-4">{{ currentPost.title }}</h1>
      <p class="text-slate-500">Von {{ currentPost.author }} am {{ formatDate(currentPost.date) }}</p>
    </header>

    <ContentRenderer :blocks="currentPost.content" />
  </div>

  <div v-else class="text-center py-20 text-red-500">
    <h2>Diesen Projekt-Post gibt es leider nicht.</h2>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import ContentRenderer from '@/components/content/ContentRenderer.vue';
import { projectsData } from '@/data/mockProjectData';
import BackButton from '@/components/ui/buttons/BackButton.vue';


// aktuelle Route
const route = useRoute();

// Extrahiere Slug aus der URL (z.B. "typescript-generics-einfuehrung")
const slug = route.params.slug as string;

// Suche passenden Artikel
const currentPost = computed(() => {
  return projectsData.find(post => post.slug === slug);
});


const formatDate = (date: Date | string) => {
  const d = new Date(date);
  return d.toLocaleDateString('de-DE');
};
</script>

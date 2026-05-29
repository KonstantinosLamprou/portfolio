<template>
  <div v-if="currentPost">
    <BackButton class="hidden md:flex"/>

    <!-- Headerkomponente -->
    <header class="text-center max-w-3xl mx-auto px-4 pt-16">
      <h1 class="text-4xl font-extrabold text-slate-600 mb-4">{{ currentPost.title }}</h1>
      <div class="grid grid-cols-2 text-sm max-md:gap-4 md:grid-cols-4">
        <div className='space-y-1 md:mx-auto'>
          <div class="text-muted-foreground">geschrieben von</div>
          <p class="flex items-center gap-2 justify-center">{{ currentPost.author }}</p>
        </div>
        <div class="space-y-1 md:mx-auto">
          <div class="text-muted-foreground">veröffentlicht am</div>
          <p class="flex items-center gap-2 justify-center">
            <time :datetime="currentPost.date.toISOString()">{{ formatDate(currentPost.date) }}</time>
          </p>
        </div>
        <div class="space-y-1 md:mx-auto">
          <div class="text-muted-foreground">Views</div>
          <p class="flex items-center gap-2 justify-center">{{ currentPost.views }}</p>
        </div>
        <div class="space-y-1 md:mx-auto">
          <div class="text-muted-foreground">Likes</div>
          <p class="flex items-center gap-2 justify-center">{{ currentPost.likes }}</p>
        </div>
      </div>
    </header>
    <div class="mt-8 flex flex-col justify-between lg:flex-row items-start">
      <ContentRenderer class="w-full lg:max-w-2xl" :blocks="currentPost.content" />
        <aside class="w-full lg:w-68 px-4 py-10 sticky top-34">
            <TableOfContents :toc="tableOfContents" />
            <LikeButton :slug="currentPost.slug" />
        </aside>
      </div>


      <div class="space-y-6">
        <CommentPost />
        <!-- <CommentSection /> -->

      </div>


  </div>

  <div v-else class="text-center py-20 text-red-500">
    <h2>Diesen Blogpost gibt es leider nicht.</h2>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import ContentRenderer from '@/components/content/ContentRenderer.vue';
import BackButton from '@/components/ui/buttons/BackButton.vue';
import { mockBlogData } from '@/data/mockBlogPostData';
import TableOfContents from '@/components/content/TableOfContents.vue';
import LikeButton from '@/components/content/LikeButton.vue';
import CommentPost from "@/components/comment/CommentPost.vue"
import CommentSection from '@/components/comment/CommentSection.vue';

// Hole dir die aktuelle Route
const route = useRoute();

// Extrahiere den Slug aus der URL (z.B. "typescript-generics-einfuehrung")
const slug = route.params.slug as string;

// Suche den passenden Artikel in den Mock-Daten
const currentPost = computed(() => {
  return mockBlogData.find(post => post.slug === slug);
});

// Kleine Hilfsfunktion für ein schöneres Datum
const formatDate = (date: Date | string) => {
  const d = new Date(date);
  return d.toLocaleDateString('de-DE');
};


const tableOfContents = computed(() => {
  return currentPost.value?.content
    .filter(block => block.type === 'heading')
    .map(heading => ({
      // WICHTIG: Die URL ist hier einfach die ID des Blocks (z.B. 'block-1')
      url: heading.id,
      title: heading.data.text,
      depth: heading.data.level
    })) || [];
})
</script>

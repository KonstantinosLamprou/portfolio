<template>
  <div class="container divide-surface divide-y">
    <div class="space-y-2 pt-6 pb-8 md:space-y-5">
      <h2 class="text-4xl leading-9 font-extrabold tracking-tight sm:text-5xl sm:leading-10 md:text-6xl md:leading-14">
        Latest
      </h2>
      <p class="text-lg leading-7">
        Hier kannst du meine aktuellsten Blog-Einträge lesen.
      </p>
    </div>

    <ul class="divide-surface divide-y">

      <li v-if="isError" class="py-12 text-gray-600">
        Zurzeit gibt es noch keine Beiträge. Schau später wieder vorbei!
      </li>

      <li v-else-if="isPending" class="py-12 text-gray-600">
        Lade Beiträge...
      </li>

      <li v-else-if="blogs && blogs.length === 0" class="py-12 text-gray-600 text-center">
        <TriangleAlertIcon class="inline w-6 h-6 mr-2" />
        Es wurden keine Beiträge gefunden.
      </li>

      <li
        v-else
        v-for="blog in blogs"
        :key="blog.slug"
        class="py-12"
      >
        <article>
          <div class="space-y-2 xl:grid xl:grid-cols-4 xl:items-baseline xl:space-y-0">

            <dl>
              <dt class="sr-only">Veröffentlicht am</dt>
              <dd class="text-base leading-6 font-medium">
                <time :datetime="blog.dateOfCreation.toISOString()">{{ formatDate(blog.dateOfCreation) }}</time>
              </dd>
            </dl>

            <div class="space-y-5 xl:col-span-3">
              <div class="space-y-6">
                <div>
                  <h3 class="text-2xl leading-8 font-medium tracking-tight transition-transform duration-300">
                    <RouterLink :to="`/blog/${blog.slug}`" class="text-heading hover:text-heading-hover transition-colors">
                      {{ blog.title }}
                    </RouterLink>
                  </h3>

                  <div class="flex flex-wrap gap-3 mt-2">
                    <span
                      v-for="tag in blog.tags"
                      :key="tag"
                      class="text-sm font-medium text-matcha uppercase tracking-wider border border-matcha/50 rounded-full px-2 py-1"
                    >
                      {{ tag }}
                    </span>
                  </div> 
                </div>

                <div class="prose max-w-none text-body opacity-80">
                  {{ blog.description }}
                </div>
              </div>

              <div class="text-base leading-6 font-medium">
                <RouterLink :to="`/blog/${blog.slug}`" class="text-link hover:text-sky transition-colors">
                  Mehr lesen &rarr;
                </RouterLink>
              </div>
            </div>

          </div>
        </article>
      </li>

    </ul>
  </div>
  <div className="flex justify-end text-base leading-6 font-medium">
    <RouterLink
        to="/blog"
        aria-label="All posts"
    >
        Alle Posts &rarr;
    </RouterLink>
  </div>
</template>


<script setup lang="ts">
import { watch } from 'vue';
import { useQuery } from '@tanstack/vue-query';
import { toast } from 'vue-sonner'
import { RouterLink } from 'vue-router';
import type { BlogLatestApiResponse } from '@/types/blogTypes.ts';
import apiClient from '@/services/api.ts';
import TriangleAlertIcon from '@/assets/triangle-alert.svg';


interface BlogData extends Omit<BlogLatestApiResponse, 'dateOfCreation'> {
  dateOfCreation: Date; 
}

const fetchLatestBlogs = async (): Promise<BlogData[]> => {
    const { data } = await apiClient.get<BlogLatestApiResponse[]>('blogs/latest');
    
    return data.map(blog => ({
    ...blog,
    dateOfCreation: new Date(blog.dateOfCreation) 
  }));
    
}

const { 
  data: blogs, 
  isPending,   
  isError, 
  error 
} = useQuery({
  queryKey: ['latest-blogs'], 
  queryFn: fetchLatestBlogs,
  retry: 1 
});


watch(isError, (hasError) => {
  if (hasError && error.value) {
    toast.error(`Fehler beim Laden: ${error.value.message || 'Server nicht erreichbar'}`);
  }
});

const formatDate = (date: Date) => {
  return new Intl.DateTimeFormat('de-DE', {
    day: '2-digit',
    month: 'long',  // 'short' für Feb, 'long' für Februar
    year: 'numeric'
  }).format(date);
};
</script>

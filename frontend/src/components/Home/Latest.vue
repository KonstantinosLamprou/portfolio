<template>
  <div class="container divide-surface divide-y">
    <div class="space-y-2 pt-6 pb-8 md:space-y-5">
      <h2 class="text-4xl leading-9 font-extrabold tracking-tight sm:text-5xl sm:leading-10 md:text-6xl md:leading-14">
        Latest
      </h2>
      <p class="text-lg leading-7">
        Hier kannst du meine letzten aktuellsten Blog-Einträge lesen.
      </p>
    </div>

    <ul class="divide-surface divide-y">

      <li v-if="mockBlogData.length === 0" class="py-12 text-gray-500">
        Zurzeit gibt es noch keine Beiträge. Schau später wieder vorbei!
      </li>

      <li
        v-else
        v-for="post in mockBlogData"
        :key="post.slug"
        class="py-12"
      >
        <article>
          <div class="space-y-2 xl:grid xl:grid-cols-4 xl:items-baseline xl:space-y-0">

            <dl>
              <dt class="sr-only">Veröffentlicht am</dt>
              <dd class="text-base leading-6 font-medium">
                <time :datetime="post.date.toISOString()">{{ formatDate(post.date) }}</time>
              </dd>
            </dl>

            <div class="space-y-5 xl:col-span-3">
              <div class="space-y-6">
                <div>
                  <h3 class="text-2xl leading-8 font-medium tracking-tight transition-transform duration-300 hover:scale-101">
                    <RouterLink :to="`/blog/${post.slug}`" class="text-heading ">
                      {{ post.title }}
                    </RouterLink>
                  </h3>

                  <div class="flex flex-wrap gap-3 mt-2">
                    <span
                      v-for="tag in post.tags"
                      :key="tag"
                      class="text-sm font-medium text-onlydarkmatcha uppercase tracking-wider"
                    >
                      {{ tag }}
                    </span>
                  </div>
                </div>

                <div class="prose max-w-none text-body opacity-80">
                  {{ post.description }}
                </div>
              </div>

              <div class="text-base leading-6 font-medium">
                <RouterLink :to="`/blog/${post.slug}`" class="text-link hover:text-sky transition-colors">
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
import { RouterLink } from 'vue-router';
import { mockBlogData } from '@/data/mockBlogPostData';

// Hilfsfunktion, um das Datum schön zu formatieren
const formatDate = (date: Date) => {
  return new Intl.DateTimeFormat('de-DE', {
    day: '2-digit',
    month: 'long',  // 'short' für Feb, 'long' für Februar
    year: 'numeric'
  }).format(date);
};
</script>

<template>

    <div class="h-full flex flex-col overflow-hidden rounded-md border-2 border-gray-200 dark:border-gray-800 p-2">

      <RouterLink :to="href" class="block shrink-0">
        <img
          :src="imgSrc"
          :alt="title"
          class="w-full aspect-video object-contain object-center p-4 bg-surface rounded-md"
          width="500"
          height="306"
        >
      </RouterLink>

      <div class="p-6 flex flex-col flex-1">
        <h2 class="text-textHeading-light dark:text-textHeading mb-3 text-2xl leading-8 font-bold tracking-tight">
          <RouterLink :to="href" :aria-label="title">
            {{ title }}
          </RouterLink>
        </h2>

        <p class="prose dark:text-textBody text-textBody-light mb-3 max-w-none">
          {{ description }}
        </p>

        <RouterLink
          :to="href"
          class="mt-auto text-koi hover:text-koiHover dark:hover:text-skyHover dark:text-sky text-base leading-6 font-medium"
          :aria-label="title"
        >
          Learn more &rarr;
        </RouterLink>
      </div>

    </div>

</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router';

// Hier definieren wir, welche Daten die Komponente von außen erwartet
defineProps<{
  title: string;
  description: string;
  imgSrc: string;
  href : string;
}>();
</script>

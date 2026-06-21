<template>
  <RouterLink :to="targetRoute">
    <div class="h-full flex flex-col overflow-hidden rounded-2xl border-2 border-tertiary p-2 transition duration-300 hover:border-3 hover:border-accent md:hover:scale-[1.01]">
        <img
          :src="imgSrc"
          :alt="title"
          class="w-full aspect-video object-contain object-center p-4 bg-surface rounded-md"
          width="500"
          height="306"
        >
      <div class="flex items-center justify-between gap-2 px-2 pt-4 text-sm text-muted-foreground">
          {{ date }}
        <div class="flex gap-2">
          <div>{{ likes }} likes</div>
          <div>&</div>
          <div>{{ views }} views</div>
        </div>
      </div>

      <div class="px-2 mt-3 flex flex-col flex-1">
        <h2 class="text-textHeading-light dark:text-textHeading mb-3 text-2xl leading-8 font-bold tracking-tight">
            {{ title }}
        </h2>

        <p class="prose dark:text-textBody text-textBody-light mb-3 max-w-none text-sm">
          {{ description }}
        </p>
<!--
        <RouterLink
          :to="href"
          class="mt-auto text-koi hover:text-koiHover dark:hover:text-skyHover dark:text-sky leading-6 font-medium text-sm"
          :aria-label="title"
        >
          Learn more &rarr;
        </RouterLink> -->
      </div>

    </div>
  </RouterLink>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { RouterLink } from 'vue-router';

const props = defineProps<{
  id: number;
  type?: 'blog' | 'project';
  title: string;
  slug: string;
  date: string;
  description: string;
  content: object;
  imgSrc: string;
  likes?: number;
  views?: number;
  comments?: number; 
}>();

const targetRoute = computed(() => {
  if (props.type === 'project') {

    return { name: 'ProjectPost', params: { slug: props.slug } };
  }
  return { name: 'BlogPost', params: { slug: props.slug } };
});

</script>

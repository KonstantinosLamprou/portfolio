<template>

    <div class="h-full flex flex-col overflow-hidden rounded-2xl border border-tertiary p-2 transition duration-300 hover:border-2 hover:border-accent md:hover:scale-[1.01]">

      <RouterLink :to="targetRoute" class="block shrink-0">
        <img
          :src="imgSrc"
          :alt="title"
          class="w-full aspect-video object-contain object-center p-4 bg-surface rounded-md"
          width="500"
          height="306"
        >
      </RouterLink>

      <div class="flex items-center justify-between gap-2 px-2 pt-4 text-sm text-muted-foreground">
          {{ formattedDate }}
        <div class="flex gap-2">
          <div>{{ likes }} likes</div>
          <div>&</div>
          <div>{{ views }} views</div>
        </div>
      </div>

      <div class="px-2 mt-3 flex flex-col flex-1">
        <h2 class="text-textHeading-light dark:text-textHeading mb-3 text-2xl leading-8 font-bold tracking-tight">
          <RouterLink :to="targetRoute" :aria-label="title">
            {{ title }}
          </RouterLink>
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

</template>

<script setup lang="ts">
import { computed } from 'vue';
import { RouterLink } from 'vue-router';

const props = defineProps<{
  id: string;
  type?: 'blog' | 'project';
  title: string;
  slug: string;
  author: string;
  date: Date;
  description: string;
  content: object;
  imgSrc: string;
  likes?: number;
  views?: number;
  comments?: number; //Anzahl
  comment?: string;
}>();

const targetRoute = computed(() => {
  if (props.type === 'project') {
    // Falls du in router/index.ts irgendwann eine ProjectPost Route einbaust,
    // kannst du diesen Namen entsprechend anpassen (z.B. 'ProjectPost').
    // Wenn aktuell noch keine da ist, geht das ins leere oder wir leiten testweise auf 'projekte'
    return { name: 'ProjectPost', params: { slug: props.slug } };
  }
  return { name: 'BlogPost', params: { slug: props.slug } };
});

const formattedDate = computed(() => {

  const months = ['Jan', 'Feb', 'Mär', 'Apr', 'Mai', 'Jun', 'Jul', 'Aug', 'Sep', 'Okt', 'Nov', 'Dez'];
  const month = months[props.date.getMonth()];
  const day = String(props.date.getDate()).padStart(2, '0');
  const year = props.date.getFullYear();

  return `${month} ${day}.${year}`; // Ergebnis: "Apr 02.2024"
});
</script>

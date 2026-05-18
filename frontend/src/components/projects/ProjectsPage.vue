
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
        <div  class="grid grid-cols-1 md:grid-cols-2 gap-8">
          <ContentCard v-for="data in projectsData" :key="data.id"
            :id="data.id"
            :type="data.type"
            :title="data.title"
            :author="data.author"
            :date="data.date"
            :description="data.description"
            :content="data.content"
            :slug="data.slug"
            :imgSrc="data.imgSrc"
            :likes="data.likes"
            :views="data.views"
          />

        </div>

      </div>

    </div>

</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router';
import ContentCard from '../content/ContentCard.vue';
import { projectsData } from '@/data/mockProjectData';
</script>

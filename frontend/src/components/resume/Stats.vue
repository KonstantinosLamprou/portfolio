<template>
  <div class="container my-20">

        <div class="mt-16 divide-y divide-surface">
          <div class="space-y-2 mb-8 mt-8">
            <h2 class="text-4xl leading-9 font-extrabold tracking-tight sm:text-5xl sm:leading-10 md:text-6xl md:leading-14">GitHub Stats</h2>
            <p class="text-sm md:text-lg leading-4 md:leading-7 opacity-80 mb-8">Metriken meines GitHub Profils</p>
          </div>
          <div class="grid grid-cols-1 gap-4 md:grid-cols-2 mb-10">
            <div
              v-for="stat in githubStats"
              :key="stat.title"
              class="border border-surface rounded-xl p-4 transition-transform duration-200 hover:scale-105 glass3d"
              :class="stat.highlight ? 'bg-matcha/10 border-matcha/30' : ''"
            >
              <h3 class="text-lg font-semibold tracking-tight text-body opacity-80">
                {{ stat.title }}
              </h3>
              <span class="text-3xl font-bold leading-tight tracking-tight text-heading">
                {{ stat.value }}
              </span>
            </div>
          </div>
        </div>

  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const githubStats = ref([
  { title: "Hireable", value: "Yes", highlight: true },
  { title: "Total Public Repositories", value: 34, highlight: false },
  { title: "Followers", value: 179, highlight: false },
  { title: "Following", value: 40, highlight: false },
  { title: "Current Company", value: "Meierhofer AG", highlight: false },
  { title: "Location", value: "München, Deutschland", highlight: false },
])

</script>

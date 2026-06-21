<template>
  <div class="container my-20">
    <div class="mt-16 divide-y divide-surface">
      <div class="space-y-2 mb-8 mt-8">
        <h2 class="text-4xl leading-9 font-extrabold tracking-tight sm:text-5xl sm:leading-10 md:text-6xl md:leading-14">GitHub Stats</h2>
        <p class="text-sm md:text-lg leading-4 md:leading-7 opacity-80 mb-8">Metriken meines GitHub Profils</p>
      </div>

      <div v-if="isLoading" class="text-body opacity-80 mb-10">
        Lade GitHub Statistiken...
      </div>

      <div v-else-if="isError" class="text-red-500 mb-10">
        Fehler beim Laden der GitHub Daten.
      </div>

      <div v-else class="grid grid-cols-1 gap-4 md:grid-cols-2 mb-10">
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
import { computed } from 'vue'
import { useQuery } from '@tanstack/vue-query'
import { toast } from 'vue-sonner'


// Späteres TODO: 
// In Backend verschieben da es ein ratelimit gibt für diese Route

const GITHUB_USERNAME = 'KonstantinosLamprou' 

const fetchGithubProfile = async () => {
  const response = await fetch(`https://api.github.com/users/${GITHUB_USERNAME}`)
  
  if (!response.ok) {
    throw new Error('Netzwerk-Antwort war nicht ok')
  }
  
  return response.json()
}

const { data, isLoading, isError } = useQuery({
  queryKey: ['github-profile', GITHUB_USERNAME],
  queryFn: fetchGithubProfile,
  staleTime: 1000 * 60 * 60, // 1 Stunde im Cache 
})

const githubStats = computed(() => {
  if (!data.value) {
    toast.error('Fehler beim Laden der GitHub Daten.')
    return []
  }

  return [
    { 
      title: "Hireable", 
      value: data.value.hireable ? "Yes" : "No", 
      highlight: true 
    },
    { 
      title: "Total Public Repositories", 
      value: data.value.public_repos, 
      highlight: false 
    },
    { 
      title: "Followers", 
      value: data.value.followers, 
      highlight: false 
    },
    { 
      title: "Following", 
      value: data.value.following, 
      highlight: false 
    },
    { 
      title: "Current Company", 
      value: data.value.company || "N/A", 
      highlight: false 
    },
    { 
      title: "Location", 
      value: data.value.location || "N/A", 
      highlight: false 
    },
  ]
})
</script>

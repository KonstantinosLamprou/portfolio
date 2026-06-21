<template>
  <div class="container my-20">
    <div class="mt-16 divide-y divide-surface">
      <div class="space-y-2 mb-8 pb-8">
        <h2 class="text-4xl leading-9 font-extrabold tracking-tight sm:text-5xl sm:leading-10 md:text-6xl md:leading-14">
          Über dieses Portfolio
        </h2>
        <p class="text-sm md:text-lg leading-4 md:leading-7 opacity-80">
          Einblicke und Metriken über diese Website
        </p>
      </div>

      <div class="mt-8 grid grid-cols-1 gap-6 md:grid-cols-2">
        <div class="relative flex flex-col p-8 overflow-hidden transition-all duration-300 border shadow-sm group glass3d rounded-xl border-surface hover:border-gray-500">
          <div class="absolute inset-0 transition-opacity duration-500 opacity-0 bg-linear-to-br from-purple-500/10 to-transparent group-hover:opacity-100"></div>
          
          <div class="relative z-10 space-y-2">
            <h3 class="flex items-center gap-2 text-xl font-semibold tracking-tight text-heading opacity-80">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="text-purple-500">
                <path d="M2 12s3-7 10-7 10 7 10 7-3 7-10 7-10-7-10-7Z" />
                <circle cx="12" cy="12" r="3" />
              </svg>
              Total Views
            </h3>
            <div class="h-px w-full bg-surface"></div>
          </div>

          <div class="relative z-10 flex flex-col items-center justify-center flex-1 py-6">
            <p class="text-5xl font-bold text-purple-500">{{ totalViews }}</p>
            <p class="text-sm text-body opacity-60 text-center mt-4">Einzigartige Besuche seit Juni-2026</p>
          </div>
        </div>

        <div class="relative flex flex-col p-8 overflow-hidden transition-all duration-300 border shadow-sm group glass3d rounded-xl border-surface hover:border-gray-500">
          <div class="absolute inset-0 transition-opacity duration-500 opacity-0 bg-linear-to-br from-koi/10 to-transparent group-hover:opacity-100"></div>

          <div class="relative z-10 space-y-2">
            <h3 class="flex items-center gap-2 text-xl font-semibold tracking-tight text-heading opacity-80">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="text-koi">
                <path d="M19 14c1.49-1.46 3-3.21 3-5.5A5.5 5.5 0 0 0 16.5 3c-1.76 0-3 .5-4.5 2-1.5-1.5-2.74-2-4.5-2A5.5 5.5 0 0 0 2 8.5c0 2.3 1.5 4.05 3 5.5l7 7Z" />
              </svg>
              Appreciation Count
            </h3>
            <div class="h-px w-full bg-surface"></div>
          </div>

          <div class="relative z-10 flex flex-col items-center justify-center flex-1 py-4">
            <p class="py-6 text-5xl font-bold text-koi">{{ totalLikes }}</p>
            
            <LoveButton 
              @increase-love="handleStatisticsUpdate(0, 1)"
              :isLoading="isUpdating"
              :storageKey="LIKE_STORAGE_KEY"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, watch } from 'vue'
import LoveButton from '@/components/ui/buttons/LoveButton.vue'
import { useStatistics, useUpdateStatisticsMutation } from '@/composables/useStatistics'

// Zentrale Definition der Storage Keys
const VIEW_STORAGE_KEY = 'portfolio_viewed'
const LIKE_STORAGE_KEY = 'portfolio_liked'

const { 
  data: statistics, 
  isSuccess: isStatisticsSuccess,
} = useStatistics()

const totalLikes = computed(() => Math.max(statistics.value?.totalLikes ?? 0, 0))
const totalViews = computed(() => Math.max(statistics.value?.totalViews ?? 0, 0))

const { mutate: updateStatistics, isPending: isUpdating } = useUpdateStatisticsMutation()

const handleStatisticsUpdate = (viewToAdd: number, likeToAdd: number) => {
  if (isUpdating.value) return; 

  updateStatistics({ viewToAdd, likeToAdd }, {
    onError: (error) => {
      console.error('Error updating statistics:', error);
    }
    // onSuccess ist hier nicht mehr nötig für das Setzen des Like-Storage, 
    // da die Child-Komponente das direkt nach dem Klick übernimmt (Optimistic UI).
  });
}

watch(isStatisticsSuccess, (success) => {
  if (success) {
    if (!sessionStorage.getItem(VIEW_STORAGE_KEY)) {
      handleStatisticsUpdate(1, 0);
      sessionStorage.setItem(VIEW_STORAGE_KEY, 'true');
    }
  }
});
</script>
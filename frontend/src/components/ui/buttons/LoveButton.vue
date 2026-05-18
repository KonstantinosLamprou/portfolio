<template>
  <button
    @click="handleLike"
    :disabled="hasLiked || isLoading"
    class="mt-4 px-6 py-3 cursor-pointer rounded-full transition-all duration-300 flex items-center gap-2 font-medium relative overflow-hidden"
    :class="[
      hasLiked
        ? 'bg-koi/10 text-gray-400 border border-koi/20 cursor-not-allowed'
        : 'bg-koi text-white hover:bg-koi/90 shadow-md hover:shadow-lg',
      isAnimating ? 'scale-105' : ''
    ]"
  >
    <span
      class="absolute inset-0 bg-white/10 opacity-0"
      :class="{ 'animate-pulse opacity-30': isAnimating && !hasLiked }"
    ></span>

    <svg
      xmlns="http://www.w3.org/2000/svg"
      width="16"
      height="16"
      viewBox="0 0 24 24"
      :fill="hasLiked ? 'currentColor' : 'none'"
      stroke="currentColor"
      stroke-width="2"
      stroke-linecap="round"
      stroke-linejoin="round"
      class="w-5 h-5 transition-transform duration-300"
      :class="[
        isAnimating && hasLiked ? 'animate-ping' : '',
        hasLiked ? 'text-koi' : ''
      ]"
    >
      <path d="M19 14c1.49-1.46 3-3.21 3-5.5A5.5 5.5 0 0 0 16.5 3c-1.76 0-3 .5-4.5 2-1.5-1.5-2.74-2-4.5-2A5.5 5.5 0 0 0 2 8.5c0 2.3 1.5 4.05 3 5.5l7 7Z" />
    </svg>

    <span class="transition-opacity duration-300 font-semibold" :class="{ 'opacity-70': isLoading }">
      {{ hasLiked ? "Danke <3" : "Ich mag dieses Portfolio" }}
    </span>

    <span v-if="isLoading && !hasLiked" class="ml-1 inline-block animate-spin">
      ⋯
    </span>
  </button>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'

const hasLiked = ref(false)
const isLoading = ref(false)
const isAnimating = ref(false)

// Wir senden ein Event an die Eltern-Komponente, um die Zahl zu erhöhen
const emit = defineEmits(['increaseLove'])

onMounted(() => {

  if (sessionStorage.getItem('portfolio_loved') === 'true') {
    hasLiked.value = true
  }
})

const handleLike = async () => {
  if (hasLiked.value || isLoading.value) return

  isLoading.value = true
  isAnimating.value = true

  //Mock
  // Fake Ladezeit (simuliert Backend-Aufruf)
  await new Promise(resolve => setTimeout(resolve, 800))

  //für die session
  hasLiked.value = true
  sessionStorage.setItem('portfolio_loved', 'true')

  // Zahl oben erhöhen
  emit('increaseLove')

  isLoading.value = false
  setTimeout(() => { isAnimating.value = false }, 600)
}
</script>

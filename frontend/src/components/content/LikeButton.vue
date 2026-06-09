<template>
  <div class="lg:mt-6 lg:mb-4 flex justify-center ">
    <button
      ref="buttonRef"
      class="flex items-center gap-3 rounded-4xl bg-gray-900 px-4 py-2 text-lg text-body  transition-transform duration-200 active:scale-95"
      @click="handleLikeButtonClick"
      aria-label="Like this post"
      type="button"
      data-testid="like-button"
    >
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width="28"
        height="28"
        viewBox="0 0 24 24"
        strokeWidth="2"
        stroke="#ef4444"
        class="relative overflow-hidden"
        fill="none"
        strokeLinecap="round"
        strokeLinejoin="round"
      >
        <defs>
          <clipPath id="clip-path">
            <path d="M19.5 12.572l-7.5 7.428l-7.5 -7.428a5 5 0 1 1 7.5 -6.566a5 5 0 1 1 7.5 6.572" />
          </clipPath>
        </defs>
        <path d="M19.5 12.572l-7.5 7.428l-7.5 -7.428a5 5 0 1 1 7.5 -6.566a5 5 0 1 1 7.5 6.572" />
        <g clip-path="url(#clip-path)">
          <!-- Der Füllstand wird über das 'y' Attribut mit einer CSS Transition animiert -->
          <rect
            x="0"
            :y="fillPercentage"
            width="24"
            height="24"
            fill="#ef4444"
            class="transition-all duration-500 ease-out"
          />
        </g>
      </svg>

      Like

      <Separator orientation="vertical" class="bg-body h-5" />
      <span v-if="isSuccess" data-testid="like-count">
        {{ data.totalLikes + cacheCount }}
      </span>
      <div v-else-if="isLoading">--</div>
      <div v-else-if="isError">Error</div>
    </button>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { toast } from 'vue-sonner'
import { useDebounceFn } from '@vueuse/core'
import { useCountLike, useIncrementLike } from '@/composables/likeQuery'
import Separator from '../ui/Separator.vue';

const props = defineProps<{
  slug: string
}>()

const cacheCount = ref(0)
const buttonRef = ref<HTMLButtonElement | null>(null)

// Annahme: Du nutzt Vue Query oder ähnliches, die Refs zurückgeben
const { data, isSuccess, isLoading, isError } = useCountLike({ slug: props.slug })
const { mutate: likePost } = useIncrementLike()

// useDebounceFn aus @vueuse/core ersetzt use-debounce
const onLikeSaving = useDebounceFn((value: number) => {
  likePost({ slug: props.slug, value })
  cacheCount.value = 0
}, 1000)

const handleLikeButtonClick = () => {
  if (isLoading.value || !data.value) return

  // Prüfen, ob das Limit von 3 Likes erreicht ist
  if (data.value.currentUserLikes + cacheCount.value >= 3) {
    toast.error('Du kannst einen Beitrag maximal 3 Mal liken!')
    return
  }

  const value = cacheCount.value === 3 ? cacheCount.value : cacheCount.value + 1
  cacheCount.value = value

  onLikeSaving(value)
}

// Berechnet den Y-Wert für das SVG-Rechteck, das das Herz ausfüllt (33% Schritte)
const fillPercentage = computed(() => {
  if (!data.value) return '100%'
  const totalLikes = data.value.currentUserLikes + cacheCount.value
  const percentage = 100 - totalLikes * 33.33
  // Stellt sicher, dass der Wert nicht unter 0 fällt
  return `${Math.max(0, percentage)}%`
})
</script>

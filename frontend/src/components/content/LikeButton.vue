<template>
  <div class="lg:mt-6 lg:mb-4 flex justify-center ">
    <button
      ref="buttonRef"
      class="flex items-center gap-2 rounded-4xl bg-transparent border border-[color:var(--border)] px-4 py-2 text-lg text-body transition-transform duration-200 active:scale-95"
      @click="handleLikeButtonClick"
      :disabled="isPending"
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
      <span data-testid="like-count">
        {{ props.initialLikes }}
      </span>
    </button>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { toast } from 'vue-sonner'
import { useMutation, useQueryClient } from '@tanstack/vue-query'
import apiClient from '@/services/api.ts'
import { isAxiosError } from 'axios';
import Separator from '../ui/Separator.vue';


const props = defineProps<{
  ContentId: number
  slug: string
  initialLikes: number
  currentUserLikes: number
  contentType: 'blog' | 'project'
}>()

const queryClient = useQueryClient()
const buttonRef = ref<HTMLButtonElement | null>(null)

const { mutate: likePost, isPending } = useMutation({
  mutationFn: async () => {

    const { data } = await apiClient.post(`/like/${props.ContentId}`)
    return data
  },
  onMutate: async () => {
    await queryClient.cancelQueries({ queryKey: [props.contentType, props.slug] })

    const previousContentData = queryClient.getQueryData([props.contentType, props.slug])

    queryClient.setQueryData([props.contentType, props.slug], (old: any) => {
      if (!old) return old
      return {
        ...old,
        likesCount: old.likesCount + 1,
        currentUserLikeCount: old.currentUserLikeCount + 1
      }
    })

    return { previousContentData }
  },

  // Bei einem Fehler (z.B. User nicht eingeloggt, Server down, oder Limit überschritten)
  onError: (err, variables, context) => {
    // UI auf den Stand vor dem Klick zurücksetzen
    if (context?.previousContentData) {
      queryClient.setQueryData([props.contentType, props.slug], context.previousContentData)
    }

    if (isAxiosError(err)) {
      if (err.response?.status === 404) {
        toast.error('Dieser Beitrag existiert nicht.');
      } else if (err.response?.status === 400) {
        toast.error(err.response.data?.message || 'Limit erreicht.');
      } else if (err.response?.status === 401) {
        toast.error('Du musst angemeldet sein, um zu liken.');
      } else {
        toast.error('Fehler beim Vergeben des Likes.');
      }
    } else {
      toast.error('Ein unerwarteter Fehler ist aufgetreten.');
    }
  },
  // im Hintergrund nochmal frische Daten vom Server holen
  onSettled: () => {
    queryClient.invalidateQueries({ queryKey: [props.contentType, props.slug] })
  }
})

const handleLikeButtonClick = () => {
  if (props.currentUserLikes >= 3) {
    toast.error('Du kannst einen Beitrag maximal 3 Mal liken!')
    return
  }

  // Mutation auslösen
  likePost()
}

const fillPercentage = computed(() => {
  const percentage = 100 - props.currentUserLikes * 33.33
  return `${Math.max(0, percentage)}%`
})
</script>

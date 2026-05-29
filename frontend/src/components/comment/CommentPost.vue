<template>
  <div v-if="!isMounted" class="flex h-32.5 items-center justify-center">
    <Spinner />
  </div>

  <form v-else class="w-full" @submit.prevent="submitComment">
    <div class="relative">
      <CommentEditor
        v-model="content"
        v-model:tabsValue="tabsValue"
        data-testid="comment-textarea-post"
      />
      
      <Button
        variant="ghost"
        size="sm"
        class="absolute right-3 bottom-3"
        type="submit"
        data-testid="comment-submit-button"
        :disabled="!isAuthenticated || content.trim() === ''"
      >
        <SendIcon class="size-4" />
      </Button>

      <UnauthenticatedOverlay v-if="!isAuthenticated" />
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { SendIcon } from 'lucide-vue-next'
import { Button } from '@/components/ui/buttons/'
import Spinner from '../ui/loaders/SpinnerLoader.vue'
import CommentEditor from './CommentEditor.vue'
import UnauthenticatedOverlay from './UnauthenticatedOverlay.vue'
import { useSession } from '@/composables/useSession' // Dein neuer TanStack Hook

// --- State & Composables ---
const content = ref('')
const tabsValue = ref<'write' | 'preview'>('write')
const { data: session, isPending: isSessionLoading } = useSession()

// Hydration Handling
const isMounted = ref(false)
onMounted(() => {
  isMounted.value = true
})

// --- Computed Properties ---
// !! wandelt das Objekt/null/undefined sicher in true oder false um
const isAuthenticated = computed(() => !!session.value && !isSessionLoading.value)

// --- Methods ---
const submitComment = () => {
  if (!isAuthenticated.value) return;
  // Hier dann später deine Logik: useCreatePostComment.mutate(...)
  console.log('Kommentar wird gesendet:', content.value)
}
</script>

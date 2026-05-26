<template>
  <div v-if="!isMounted" class="flex h-32.5 items-center justify-center">
    <Spinner />
  </div>

  <form v-else @submit.prevent="submitComment">
    <div class="relative">
      <CommentEditor
        v-model="content"
        v-model:tabsValue="tabsValue"
        @mod-enter="submitComment"
        :disabled="disabled"
        data-testid="comment-textarea-post"
      />
      
      <Button
        variant="ghost"
        size="sm"
        class="absolute right-3 bottom-3"
        type="submit"
        :disabled="disabled || !content.trim()"
        :aria-disabled="disabled || !content.trim()"
        data-testid="comment-submit-button"
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
// Angenommene Importe für deine Architektur:
import { toast } from 'vue-sonner'
import { Button } from '@/components/ui/buttons/'
import Spinner from '../ui/loaders/SpinnerLoader.vue'
import CommentEditor from './CommentEditor.vue'
import UnauthenticatedOverlay from './UnauthenticatedOverlay.vue'
import { useCommentsContext } from '@/contexts/comments.context'
import { useCreatePostComment } from '@/hooks/queries/comment.query'
import { useSession } from '@/lib/auth-client'

// --- State & Composables ---
const { slug } = useCommentsContext()
const content = ref('')
const tabsValue = ref<'write' | 'preview'>('write')
const { data: session, isPending: isSessionLoading } = useSession()

// Hydration Handling (ähnlich zu useIsHydrated)
const isMounted = ref(false)
onMounted(() => {
  isMounted.value = true
})

// --- API Mutation ---
// Angenommen, du nutzt @tanstack/vue-query für die Anbindung an dein Backend
const { mutate: createComment, isPending: isCreating } = useCreatePostComment(
  { slug },
  {
    onSuccess: () => {
      content.value = ''
      toast.success('success.comment-posted', { testId: 'comment-posted-toast' })
      tabsValue.value = 'write'
    }
  }
)

// --- Computed Properties ---
const isAuthenticated = computed(() => session.value !== null && !isSessionLoading.value)
const disabled = computed(() => !isAuthenticated.value || isCreating.value)

// --- Methoden ---
function submitComment() {
  if (isCreating.value) return

  if (!content.value.trim()) {
    toast.error('error.comment-cannot-be-empty')
    return
  }

  createComment({
    slug: slug.value,
    content: content.value.trim(),
  })
}
</script>

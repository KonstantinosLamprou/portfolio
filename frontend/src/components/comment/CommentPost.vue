<template>
  <div v-if="isSessionLoading" class="flex h-32.5 items-center justify-center">
    <Spinner />
  </div>

  <form v-else class="w-full" @submit.prevent="submitComment">
    <div class="relative">
      <CommentEditor
        v-model="content"
        v-model:tabsValue="tabsValue"
        :placeholder="'Lass ein Kommentar da'"
        data-testid="comment-textarea-post"
      />
      
      <Button
        variant="ghost"
        size="sm"
        class="absolute right-3 bottom-1.5 cursor-pointer"
        type="submit"
        data-testid="comment-submit-button"
        :disabled="!isAuthenticated || content.trim() === ''"
      >
        <Spinner v-if="isPending" class="size-4" />
        <SendIcon class="size-4 " />
      </Button>

      <UnauthenticatedOverlay v-if="!isAuthenticated" />
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue' 
import { toast } from 'vue-sonner'
import { useMutation, useQueryClient } from '@tanstack/vue-query'
import { isAxiosError } from 'axios'

import type { CreateCommentRequest } from '@/types/comment'
import apiClient from '@/services/api.ts'
import { useSession } from '@/composables/useSession'
import { SendIcon } from 'lucide-vue-next'
import { Button } from '@/components/ui/buttons/'
import Spinner from '../ui/loaders/SpinnerLoader.vue'
import CommentEditor from './CommentEditor.vue'
import UnauthenticatedOverlay from './UnauthenticatedOverlay.vue'

const queryClient = useQueryClient(); 

const props = defineProps<{
  blogId: number
}>()

// --- State & Composables ---
const content = ref('')
const tabsValue = ref<'write' | 'preview'>('write')
const { data: session, isPending: isSessionLoading } = useSession()

// --- Computed Properties ---
// !! wandelt das Objekt/null/undefined sicher in true oder false um
const isAuthenticated = computed(() => !!session.value && !isSessionLoading.value)


const { mutate: createComment, isPending } = useMutation({
  mutationFn: async (newComment: CreateCommentRequest) => {
    const { data } = await apiClient.post('/comments', newComment);
    return data;
  },
  onSuccess: () => {
    toast.success('Kommentar erfolgreich erstellt!');
    content.value = ''; 
    tabsValue.value = 'write';

    // TanStack Query wird gesagt das die kommentare für diesen Blogpost veraltet sind und neu gefetcht werden müssen.
    // Dadurch triggert TanStack im CommentWrapper automatisch einen neuen GET-Request
    queryClient.invalidateQueries({ queryKey: ['comments', props.blogId] })    
  },
  onError: (error) => {
    if (isAxiosError(error)) {
      toast.error(`Fehler beim Erstellen des Kommentars: ${error.response?.data?.message || error.message}`);
    } else {
      toast.error('Ein unerwarteter Fehler ist aufgetreten.');
    }
  },
});

const submitComment = () => {
  if (content.value.trim() === '') return;

  const payload: CreateCommentRequest = {
    Text: content.value.trim(),
    ContentType: 'blog', 
    ContentId: props.blogId, 
    // ParentCommentId lassen wir weg für Top-Level
  };
  
  createComment(payload);
}

</script>

<template>
  <div class="mt-4 flex flex-col gap-3">
    <form class="w-full" @submit.prevent="submitReply">
      <div class="relative">
        <CommentEditor
          v-model="content"
          v-model:tabsValue="tabsValue"
          :placeholder="'Antworte auf diesen Kommentar'"
          data-testid="reply-textarea-post"
        />
        <Button
          variant="ghost"
          size="sm"
          class="absolute right-3 bottom-1.5 cursor-pointer"
          type="submit"
          data-testid="reply-submit-button"
          :disabled="!isAuthenticated || content.trim() === ''"
        >
          <SendIcon class="size-4" />
        </Button>
        
        <UnauthenticatedOverlay v-if="!isAuthenticated" />
      </div>
    </form>

    <div class="flex gap-2">
      <!-- <button 
        @click="submitReply"
        class="px-4 py-1.5 text-chat-text hover:bg-chat-hover hover:text-chat-text-strong rounded-full text-sm font-medium transition"
        :disabled="!content.trim()"
      >
      <Spinner v-if="isPending" class="size-4" />
        Reply
      </button> -->
      <button 
        @click="$emit('cancel')"
        class="px-4 py-1.5 text-chat-text hover:bg-chat-hover hover:text-chat-text-strong rounded-full text-sm font-medium transition"
      >
        Cancel
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import CommentEditor from './CommentEditor.vue';
import UnauthenticatedOverlay from '@/components/comment/UnauthenticatedOverlay.vue';
import { SendIcon } from 'lucide-vue-next';
import { useSession } from '@/composables/useSession'
import { Button } from '@/components/ui/buttons/'
import Spinner from '../ui/loaders/SpinnerLoader.vue'
import { useMutation, useQueryClient } from '@tanstack/vue-query';
import { toast } from 'vue-sonner';
import { isAxiosError } from 'axios';

import type { CreateCommentRequest } from '@/types/comment'
import apiClient from '@/services/api.ts'


const queryClient = useQueryClient();
const emit = defineEmits(['cancel', 'submit']);


const props = defineProps<{
  commentId: string;
  contentId: number;
  contentType: string;
}>();

// TODO: Hier der reply fetch? 

const { data: session, isPending: isSessionLoading } = useSession()
// --- Computed Properties ---
// !! wandelt das Objekt/null/undefined sicher in true oder false um
const isAuthenticated = computed(() => !!session.value && !isSessionLoading.value); 
const content = ref(''); 
const tabsValue = ref<'write' | 'preview'>('write')


const { mutate: createComment, isPending } = useMutation({
  mutationFn: async (newComment: CreateCommentRequest) => {
    const { data } = await apiClient.post('/comments', newComment);
    return data;


  },
  onSuccess: () => {
    toast.success('Kommentar erfolgreich erstellt!');
    content.value = ''; 
    tabsValue.value = 'write';

    queryClient.invalidateQueries({ queryKey: ['comments', props.contentId] })    
    emit('submit'); 

  },
  onError: (error) => {
    if (isAxiosError(error)) {
      toast.error(`Fehler beim Erstellen des Kommentars: ${error.response?.data?.message || error.message}`);
    } else {
      toast.error('Ein unerwarteter Fehler ist aufgetreten.');
    }
  },
});

const submitReply = () => {
  if (!isAuthenticated.value) {
    toast.error('Bitte melde dich an, um einen Kommentar zu erstellen.');
    return;
  }

  if (content.value.trim() === '') {
      toast.error('Der Kommentar darf nicht leer sein.');
      return;
  }

  if (content.value.trim()) {

    const payload: CreateCommentRequest = {
      Text: content.value.trim(),
      ContentType: props.contentType, 
      ContentId: props.contentId, 
      ParentCommentId: props.commentId, 
    };
    
    createComment(payload);
    emit('submit', content.value);
    content.value = '';
  }  


}

</script>
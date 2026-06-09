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
          <SendIcon class="size-4 " />
        </Button>
        
        <UnauthenticatedOverlay v-if="!isAuthenticated" />
      </div>
    </form>

    <div class="flex gap-2">
      <button 
        @click="submitReply"
        class="px-4 py-1.5 bg-gray-700 hover:bg-gray-600 text-white rounded-full text-sm font-medium transition"
        :disabled="!content.trim()"
      >
        Reply
      </button>
      <button 
        @click="$emit('cancel')"
        class="px-4 py-1.5 hover:bg-gray-800 text-gray-300 rounded-full text-sm font-medium transition"
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


const emit = defineEmits(['cancel', 'submit']);
const content = ref('')
const tabsValue = ref<'write' | 'preview'>('write')

// TODO: Hier der reply fetch? 
const submitReply = () => {
  if (!isAuthenticated.value) return;


  // Hier dann später deine Logik: useCreatePostComment.mutate(...)
  // Wird die Logik hier weiter zum Parent durchgereicht?
  if (content.value.trim()) {
    emit('submit', content.value);
    content.value = ''; // Reset nach Submit
  }
};

const { data: session, isPending: isSessionLoading } = useSession()
// --- Computed Properties ---
// !! wandelt das Objekt/null/undefined sicher in true oder false um
const isAuthenticated = computed(() => !!session.value && !isSessionLoading.value)


</script>
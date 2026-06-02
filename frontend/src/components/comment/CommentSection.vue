<template>
  <div class="mb-6">
    <div class="flex gap-4">
      <img 
        :src="comment.author.profilePictureUrl || 'https://i.pravatar.cc/150?u=default'" 
        alt="Avatar" 
        class="w-10 h-10 rounded-full bg-gray-800 object-cover"
      />
      
      <div class="flex-1">
        <div class="flex items-center gap-2 mb-1">
          <span class="font-medium text-white">{{ comment.author.name }}</span>
          <span class="text-sm text-gray-500">{{ formatDate(comment.createdAt) }}</span>
        </div>
        
        <p class="text-gray-300 mb-3 whitespace-pre-wrap">{{ comment.text }}</p>
        
        <div class="flex items-center gap-2 text-gray-400 text-sm">
          <button class="flex items-center gap-1.5 px-3 py-1.5 bg-gray-800 hover:bg-gray-700 rounded-full transition">
            <span>👍</span> <span v-if="comment.upvotes > 0">{{ comment.upvotes }}</span>
          </button>
          <button class="flex items-center gap-1.5 px-3 py-1.5 bg-gray-800 hover:bg-gray-700 rounded-full transition">
            <span>👎</span> <span v-if="comment.downvotes > 0">{{ comment.downvotes }}</span>
          </button>
          
          <button 
            @click="isReplying = true"
            class="flex items-center gap-1.5 px-3 py-1.5 bg-gray-800 hover:bg-gray-700 rounded-full transition ml-2"
          >
            <span>💬</span> Reply
          </button>
        </div>

        <CommentReply 
          v-if="isReplying" 
          @cancel="isReplying = false"
          @submit="handleReplySubmit"
        />

        <div v-if="comment.replies && comment.replies.length > 0" class="mt-3">
          <button 
            @click="showReplies = !showReplies"
            class="text-blue-400 hover:text-blue-300 text-sm font-medium flex items-center gap-1"
          >
            <span :class="{'rotate-180': showReplies}" class="transition-transform">▼</span>
            {{ comment.replies.length }} reply
          </button>
        </div>

        <div v-if="showReplies" class="mt-4 border-l-2 border-gray-800 pl-4">
          <CommentReply
            v-for="reply in comment.replies" 
            :key="reply.id" 
            :comment="reply"
            @reply-added="$emit('reply-added', $event)" 
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import type { CommentResponseDto } from '@/types/comment.ts';
import CommentReply from './CommentReply.vue';

const props = defineProps<{
  comment: CommentResponseDto;
}>();

const emit = defineEmits(['reply-added']);

const isReplying = ref(false);
const showReplies = ref(false);

const formatDate = (dateString: string) => {
  const date = new Date(dateString);
  return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
};

const handleReplySubmit = (text: string) => {
  // Hier würdest du das Event nach oben durchreichen oder direkt den API-Call machen
  emit('reply-added', { parentId: props.comment.id, text });
  isReplying.value = false;
  showReplies.value = true; // Replies direkt aufklappen, wenn man selbst geantwortet hat
};
</script>

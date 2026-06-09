<template>
  <div class="mb-6">
    <div class="flex gap-4">
      <img 
        :src="comment.author.profilePictureUrl || 'https://i.pravatar.cc/150?u=default'" 
        alt="Avatar" 
        class="w-6 h-6 rounded-full bg-gray-800 object-cover"
      />
      
      <div class="flex-1">
        <div class="flex items-center gap-2 mb-1">
          <span class="font-medium text-sm lg:text-base text-white">{{ comment.author.name }}</span>
          <span class="text-xs lg:text-sm text-gray-500">{{ formatDate(comment.createdAt) }}</span>
          <button class="ml-auto bg-main-800 text-gray-400 hover:text-gray-200 hover:bg-gray-900 p-1 rounded-full transition cursor-pointer">
            <EllipsisIcon class="w-4 h-4"/>
          </button>
        </div>
        
        <p class="text-gray-300 lg:text-base text-sm mb-3 pr-3 whitespace-pre-wrap">{{ comment.text }}</p>
        
        <div class="flex items-center gap-1 text-gray-400 lg:text-sm text-xs ">
          <button class="flex items-center gap-1.5 px-3 py-1.5 bg-main-800 hover:bg-gray-900 rounded-full transition cursor-pointer">
            <ThumbsUpIcon
              class="w-4 h-4"
            />
            <span v-if="comment.upvotes > 0 ? comment.upvotes : '0'">{{ comment.upvotes }}</span>
          </button>
          <button class="flex items-center gap-1.5 px-3 py-1.5 bg-main-800 hover:bg-gray-900 rounded-full transition cursor-pointer">
            <ThumbsDownIcon
              class="w-4 h-4"
            />
            <span v-if="comment.downvotes > 0 ? comment.downvotes : '0'">{{ comment.downvotes }}</span>
          </button>
          
          <button 
            @click="isReplying = true"
            class="flex items-center gap-1.5 px-3 py-1.5 bg-main-800 hover:bg-gray-900 rounded-full transition cursor-pointer ml-2"
          >
            <CommentIcon
              class="w-4 h-4"
            />
            Reply
          </button>
        </div>

        <!-- Antwort verschicken -->
        <CommentReply 
          v-if="isReplying" 
          @cancel="isReplying = false"
          @submit="handleReplySubmit"
        />

        <!-- Replies anzeigen -->
        <div v-if="comment.replies && comment.replies.length > 0" class="mt-3 ">
          <button 
            @click="showReplies = !showReplies"
            class="text-gray-300 rounded-4xl hover:bg-gray-900 text-sm font-light flex items-center px-3 py-1 gap-1 "
          >
            <ChevronDownIcon 
              :class="{'rotate-180': showReplies}" 
              class="transition-transform w-4 h-4"
            />
            {{ comment.replies.length }} reply
          </button>
        </div>

        <div v-if="showReplies" class="mt-4 border-l-2 border-gray-800 pl-2">
          <Reply
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
import ThumbsUpIcon  from '@/assets/thumb-up.svg';
import ThumbsDownIcon  from '@/assets/thumb-down.svg';
import CommentIcon  from '@/assets/comment.svg';
import Reply from './Reply.vue';
import ChevronDownIcon from '@/assets/chevron-down.svg';
import EllipsisIcon from '@/assets/ellipsis.svg';

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




// TODO: Antwort verschicken
const handleReplySubmit = (text: string) => {
  // Hier würdest du das Event nach oben durchreichen oder direkt den API-Call machen
  emit('reply-added', { parentId: props.comment.id, text });
  isReplying.value = false;
  showReplies.value = true; // Replies direkt aufklappen, wenn man selbst geantwortet hat
};
</script>

<template>
  <div class="mb-6">
    <div class="flex gap-4">
      <img 
        :src="comment.author.profilePictureUrl || 'https://i.pravatar.cc/150?u=default'" 
        alt="Avatar" 
        class="w-6 h-6 rounded-full bg-chat-bg object-cover"
        referrerpolicy="no-referrer"
      />
      
      <div class="flex-1">
        <div class="flex items-center gap-2 mb-1">
          <span class="font-medium text-sm lg:text-base text-chat-text-strong">{{ comment.author.name }}</span>
          <time :datetime="comment.createdAt.toISOString()" class="text-xs lg:text-sm text-chat-text">{{ formatDate(comment.createdAt) }}</time>
          <button class="ml-auto text-chat-text hover:text-chat-text-strong hover:bg-chat-hover p-1 rounded-full transition cursor-pointer">
            <EllipsisIcon class="w-4 h-4"/>
          </button>
        </div>
        
        <p class="text-foreground lg:text-base text-sm mb-3 pr-3 whitespace-pre-wrap">{{ comment.text }}</p>
        
        <div class="flex items-center gap-1 lg:text-sm text-xs ">
          <button class="flex items-center gap-1.5 px-3 py-1.5 text-chat-text hover:bg-chat-hover hover:text-chat-text-strong rounded-full transition cursor-pointer">
            <ThumbsUpIcon class="w-4 h-4" />
            <span v-if="comment.upvotes > 0 ? comment.upvotes : '0'">{{ comment.upvotes }}</span>
          </button>
          
          <button class="flex items-center gap-1.5 px-3 py-1.5 text-chat-text hover:bg-chat-hover hover:text-chat-text-strong rounded-full transition cursor-pointer">
            <ThumbsDownIcon class="w-4 h-4" />
            <span v-if="comment.downvotes > 0 ? comment.downvotes : '0'">{{ comment.downvotes }}</span>
          </button>
          
          <button 
            @click="isReplying = true"
            class="flex items-center gap-1.5 px-3 py-1.5 text-chat-text hover:bg-chat-hover hover:text-chat-text-strong rounded-full transition cursor-pointer ml-2"
          >
            <CommentIcon class="w-4 h-4" />
            Reply
          </button>
        </div>

        <CommentReply 
          v-if="isReplying" 
          @cancel="isReplying = false"
          @submit="handleReplySubmit"
        />

        <div v-if="comment.replies && comment.replies.length > 0" class="mt-3 ">
          <button 
            @click="showReplies = !showReplies"
            class="text-chat-text rounded-4xl hover:bg-chat-hover hover:text-chat-text-strong text-sm font-light flex items-center px-3 py-1 gap-1 "
          >
            <ChevronDownIcon 
              :class="{'rotate-180': showReplies}" 
              class="transition-transform w-4 h-4"
            />
            {{ comment.replies.length }} reply
          </button>
        </div>

        <div v-if="showReplies" class="mt-4 border-l-2 border-chat-border pl-2">
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
import type { CommentResponseDtoExtended } from './Commentwrapper.vue';
import CommentReply from './CommentReply.vue';
import ThumbsUpIcon  from '@/assets/thumb-up.svg';
import ThumbsDownIcon  from '@/assets/thumb-down.svg';
import CommentIcon  from '@/assets/comment.svg';
import Reply from './Reply.vue';
import ChevronDownIcon from '@/assets/chevron-down.svg';
import EllipsisIcon from '@/assets/ellipsis.svg';

const props = defineProps<{
  comment: CommentResponseDtoExtended;
}>();



const emit = defineEmits(['reply-added']);
const isReplying = ref(false);
const showReplies = ref(false);

// TODO: Antwort verschicken
const handleReplySubmit = (text: string) => {
  // Hier würdest du das Event nach oben durchreichen oder direkt den API-Call machen
  emit('reply-added', { parentId: props.comment.id, text });
  isReplying.value = false;
  showReplies.value = true; // Replies direkt aufklappen, wenn man selbst geantwortet hat
};

const formatDate = (date: Date) => {
  return date.toLocaleDateString('de-DE');
};
</script>

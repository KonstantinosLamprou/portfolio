<template>
  <div class="ml-2 mb-4">
    <div class="flex gap-4">
      <img 
        :src="comment.author.profilePictureUrl || 'https://i.pravatar.cc/150?u=default'" 
        alt="Avatar" 
        class="w-6 h-6 rounded-full bg-gray-800 object-cover"
      />
      
      <div class="flex-1">
        <div class="flex items-center gap-2 mb-1">
          <span class="font-medium text-sm lg:text-base text-white">{{ comment.author.name }}</span>
          <span class="text-xs lg:text-sm text-gray-500 ">{{ formatDate(comment.createdAt) }}</span>
          <button class="ml-auto bg-main-800 text-gray-400 hover:text-gray-200 hover:bg-gray-900 p-1 rounded-full transition cursor-pointer">
            <EllipsisIcon class="w-4 h-4"/>
          </button>
        </div>
        
        <p class="text-gray-300 lg:text-base text-sm mb-3 pr-3 whitespace-pre-wrap">{{ comment.text }}</p>
        
        <div class="flex items-center gap-1 text-gray-400 lg:text-sm text-xs">
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
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { CommentReplyDto } from '@/types/comment.ts';
import ThumbsUpIcon  from '@/assets/thumb-up.svg';
import ThumbsDownIcon  from '@/assets/thumb-down.svg';
import EllipsisIcon from '@/assets/ellipsis.svg';

const props = defineProps<{
  comment: CommentReplyDto;
}>();


const formatDate = (dateString: string) => {
  const date = new Date(dateString);
  return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
};


//TODO: Votes fetchen 

</script>
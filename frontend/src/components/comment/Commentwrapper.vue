<template>
  <div class="w-full mx-auto p-4 bg-main"> 
    <div>
      <CommentHeader 
        :totalComments="comments.length" 
        :totalReplies="totalReplies" 
      />

      <div class="mt-6">
        <CommentSection 
          v-for="comment in comments" 
          :key="comment.id" 
          :comment="comment"
          @reply-added="onReplyAdded"
        />
      </div>
    </div>

  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import CommentHeader from './CommentHeaderSection.vue';
import CommentSection from './CommentSection.vue';
import { mockComments } from '@/data/mockCommentData.ts';
import type { CommentResponseDto } from '@/types/comment.ts';


// TODO: Comments von API fetchen
const comments = ref<CommentResponseDto[]>(mockComments);

// Berechnet alle Replies aus der ersten Ebene für den Header
const totalReplies = computed(() => {
  return comments.value.reduce((acc, curr) => acc + (curr.replies?.length || 0), 0);
});

const onReplyAdded = (payload: { parentId: string, text: string }) => {
  console.log('Neuer Reply an Backend senden:', payload);
  // Logik um den Mock-State zu updaten, bis das Backend angebunden ist
};
</script>
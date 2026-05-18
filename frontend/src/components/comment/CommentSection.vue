<template>

      <div
        v-for="comment in mockComments"
        :key="comment.id"
        class="border-b border-gray-200 pb-4 last:border-0"
      >
        <div class="flex justify-between items-center mb-2">
          <strong class="text-gray-900">{{ comment.author }}</strong>
          <span class="text-sm text-gray-500">{{ comment.createdAt }}</span>
        </div>
        <p class="text-gray-700 leading-relaxed whitespace-pre-wrap">
          {{ comment.content }}
        </p>
      </div>

      <p v-if="mockComments.length === 0" class="text-gray-500 italic text-center py-4">
        Noch keine Kommentare. Sei der Erste!
      </p>


</template>

<script setup lang="ts">
import { mockComments } from '@/data/mockComment';
</script>

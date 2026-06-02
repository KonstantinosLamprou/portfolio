<template>
  <div class="flex items-center justify-between mb-6 text-gray-300">
    <div class="font-semibold">
      <span>{{ totalComments }} comments</span>
      <span v-if="totalReplies > 0"> · {{ totalReplies }} replies</span>
    </div>
    
    <button class="flex items-center gap-2 px-3 py-1.5 bg-gray-800 hover:bg-gray-700 rounded-full text-sm transition">
      <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 4h13M3 8h9M3 12h5m0 0l-5-5m5 5v12"></path></svg>
      Sort by
    </button>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  totalComments: number;
  totalReplies: number;
}>();
</script>
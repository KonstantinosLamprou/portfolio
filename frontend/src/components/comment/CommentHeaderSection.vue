<template>
  <div class="flex items-center justify-between mb-6 text-gray-300">
    <div class="font-light">
      <span>{{ totalComments }} comments</span>
      <span v-if="totalReplies > 0"> · {{ totalReplies }} replies</span>
    </div>
    
    <button class="glass3d rounded-full text-nowrap overflow-hidden inline-block">
      <GlareHover
        glareColor="#ffffff"
        :glareOpacity="0.3"
        :glareAngle="-30"
        :glareSize="300"
        :transitionDuration="800"
        :playOnce="false"
        width="100%"
        height="100%"
        background="transparent"
        borderRadius="9999px"
        borderColor="transparent"
        className="px-3 py-1.5 flex items-center gap-2 text-sm"
      >
        <FilterIcon 
          class="w-4 h-4"
        />
        Sort by
      </GlareHover>
    </button>
  </div>
</template>

<script setup lang="ts">
import FilterIcon from '@/assets/list-filter.svg';
import GlareHover from '../ui/GlareHover.vue';

defineProps<{
  totalComments: number;
  totalReplies: number;
}>();
</script>
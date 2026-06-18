<template>
  <div class="flex items-center justify-between mb-6">
    <div class="font-light text-chat-text">
      <span>{{ totalComments }} comments</span>
      <span v-if="totalReplies > 0"> · {{ totalReplies }} replies</span>
    </div>
    
    <PopoverRoot>
      <PopoverTrigger as-child>
        <button class="glass3d rounded-full text-nowrap overflow-hidden inline-block outline-none cursor-pointer">
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
            <FilterIcon class="w-4 h-4"/>
            Sort by
          </GlareHover>
        </button>
      </PopoverTrigger>

      <PopoverPortal>
        <PopoverContent
          class="z-50 w-44 rounded-xl bg-[#1f1f1f] p-1.5 shadow-xl border border-white/10 text-sm text-[#fafafa] mt-2 outline-none"
          :side-offset="4"
          align="end"
        >
          <button
            @click="$emit('update-sort', 'newest')"
            class="flex w-full items-center justify-between rounded-lg px-3 py-2 text-left hover:bg-white/10 transition-colors cursor-pointer"
          >
            Newest
            <svg v-if="sortOrder === 'newest'" xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <polyline points="20 6 9 17 4 12"></polyline>
            </svg>
          </button>
          
          <button
            @click="$emit('update-sort', 'oldest')"
            class="flex w-full items-center justify-between rounded-lg px-3 py-2 text-left hover:bg-white/10 transition-colors cursor-pointer"
          >
            Oldest
            <svg v-if="sortOrder === 'oldest'" xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <polyline points="20 6 9 17 4 12"></polyline>
            </svg>
          </button>
        </PopoverContent>
      </PopoverPortal>
    </PopoverRoot>

  </div>
</template>

<script setup lang="ts">
import FilterIcon from '@/assets/list-filter.svg';
import GlareHover from '../ui/GlareHover.vue';
import { PopoverRoot, PopoverTrigger, PopoverPortal, PopoverContent } from 'reka-ui';

defineProps<{
  totalComments: number;
  totalReplies: number;
  sortOrder: 'newest' | 'oldest';
}>();


defineEmits<{
  (e: 'update-sort', value: 'newest' | 'oldest'): void;
}>();

</script>
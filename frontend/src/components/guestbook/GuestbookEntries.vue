<template>
<div class="mb-6">
    <div class="flex gap-4">
      <img 
        :src="DefaultAvatar" 
        alt="Avatar" 
        class="w-6 h-6 rounded-full bg-chat-bg object-cover shrink-0"
        referrerpolicy="no-referrer"
      />
      
      <div class="flex-1 min-w-0">
        <div class="flex items-center gap-2 mb-1">
        <!-- <TooltipProvider>
          <Tooltip>
            <TooltipTrigger as-child>
              <span class="font-medium truncate text-sm lg:text-base text-chat-text-strong cursor-default">{{ comment.isDeleted ? 'Gelöschter Benutzer' : comment.author.name }}</span>
            </TooltipTrigger>
            <TooltipContent>
              {{ comment.isDeleted ? 'Gelöschter Benutzer' : comment.author.name }}
            </TooltipContent>
          </Tooltip>
        </TooltipProvider>         -->
        <!-- <time :datetime="String()" class="text-xs lg:text-sm text-chat-text">
          {{ formatDate() }}
        </time> -->
        <p class="text-foreground lg:text-base text-sm mb-3 whitespace-pre-wrap break-words"></p>
      </div>
    </div>
  </div>
</div>

</template>

<script setup lang="ts">
import DefaultAvatar from '@/assets/default-avatar.svg?url';



</script>   
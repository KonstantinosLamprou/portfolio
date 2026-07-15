<template>
<div class="flex flex-col items-center mt-6">
  
  <div v-if="entries?.length === 0" class="text-center flex flex-col gap-4 text-muted-foreground mt-10">
    <TriangleAlertIcon class="w-12 h-12 mx-auto text-warning" />
    Es gibt noch keine Einträge. Sei der Erste, der einen Gruß hinterlässt!  
  </div>

  <div 
    v-for="entry in entries"
    :key="entry.id" 
    class="mb-4 rounded-xl border w-full lg:w-2/3 border-[color:var(--border)] p-4">
    
    <div class="flex gap-4">
      <img 
        :src="entry.author.profilePictureUrl || DefaultAvatar" 
        alt="Avatar" 
        class="w-10 h-10 rounded-full bg-chat-bg object-cover shrink-0"
        referrerpolicy="no-referrer"
      />
      
      <div class="flex-1 min-w-0 flex flex-col justify-start">
        
        <div class="flex flex-col mb-1">
          <TooltipProvider>
            <Tooltip>
              <TooltipTrigger as-child>
                <span class="font-medium truncate text-sm lg:text-base text-chat-text-strong cursor-default">
                  {{ entry.author.name }}
                </span>
              </TooltipTrigger>
              <TooltipContent>
                <p>{{ entry.author.name }}</p>
              </TooltipContent>
            </Tooltip>
          </TooltipProvider>         
          <time :datetime="String(entry.createdAt)" class="text-xs font-light text-chat-text/70">
            {{ formatDate(entry.createdAt) }}
          </time> 
        </div>

        <p class="text-foreground lg:text-base text-sm whitespace-pre-wrap break-words mt-1">
          {{ entry.message }}
        </p>
        
      </div>
    </div>
  </div>    
</div>
</template>

<script setup lang="ts">
import DefaultAvatar from '@/assets/default-avatar.svg?url';
import { useGetGuestbookEntries } from '@/composables/useGuestbook';
import { Tooltip, TooltipContent, TooltipTrigger, TooltipProvider } from '@/components/ui/tooltip';
import TriangleAlertIcon from '@/assets/triangle-alert.svg';
const { data: entries } = useGetGuestbookEntries();

// Helper für das Datum
const formatDate = (date: Date | string) => {
  const value = typeof date === 'string' ? new Date(date) : date;

  return value.toLocaleString('de-US', {
    month: 'long',
    day: 'numeric',
    year: 'numeric',
    hour: 'numeric',
    minute: '2-digit',
    hour12: true,
  });
};
</script>
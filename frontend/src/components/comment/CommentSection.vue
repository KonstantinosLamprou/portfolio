<template>
  <div class="mb-6">
    <div 
      :id="`comment-${comment.id}`" 
      class="flex gap-4 scroll-mt-24"
      ref="commentNode"
      >
      <img 
        :src="comment.isDeleted ? DefaultAvatar : comment.author.profilePictureUrl || DefaultAvatar" 
        alt="Avatar" 
        class="w-6 h-6 rounded-full bg-chat-bg object-cover shrink-0"
        referrerpolicy="no-referrer"
      />
      
      <div class="flex-1 min-w-0">
        <div class="flex items-center gap-2 mb-1">
        <TooltipProvider>
          <Tooltip>
            <TooltipTrigger as-child>
              <span class="font-medium truncate text-sm lg:text-base text-chat-text-strong cursor-default">{{ comment.isDeleted ? 'Gelöschter Benutzer' : comment.author.name }}</span>
            </TooltipTrigger>
            <TooltipContent>
              {{ comment.isDeleted ? 'Gelöschter Benutzer' : comment.author.name }}
            </TooltipContent>
          </Tooltip>
        </TooltipProvider>        
        <time :datetime="String(comment.createdAt)" class="text-xs lg:text-sm text-chat-text">
          {{ formatDate(comment.createdAt) }}
        </time>
          <div class="ml-auto shrink-0">
            <PopoverRoot v-model:open="isPopoverOpen" >
              <PopoverTrigger as-child>
                <button class="text-chat-text hover:text-chat-text-strong hover:bg-chat-hover p-1 rounded-full transition cursor-pointer">
                  <EllipsisIcon class="w-4 h-4"/>
                </button>
              </PopoverTrigger>
              <PopoverPortal>
                <PopoverContent
                  class="z-50 w-auto flex flex-row lg:flex-col lg:w-44 gap-1 rounded-xl bg-[#1f1f1f] p-1.5 shadow-xl border border-white/10 text-sm text-[#fafafa] mt-2 outline-none"
                  :side-offset="4"
                  align="end"
                >
                  <Dialog>
                    <DialogTrigger as-child>
                      <Button
                        v-if="isAuthor"
                        class="flex items-center justify-center lg:justify-start bg-transparent hover:bg-white/10 text-[#fafafa] rounded-lg p-2 lg:px-3 lg:py-2 transition-colors cursor-pointer"
                      >
                        <TrashIcon class="w-5 h-5 lg:w-4 lg:h-4 lg:mr-2 shrink-0"/>
                        <p class="hidden lg:block">Löschen</p>
                      </Button>
                    </DialogTrigger>
                    <DialogContent>
                      <DialogHeader>
                        <DialogTitle>Kommentar löschen</DialogTitle>
                        <DialogDescription>Willst du diesen Kommentar wirklich löschen? Diese Aktion kann nicht rückgängig gemacht werden.</DialogDescription>
                      </DialogHeader>
                      <DialogFooter >
                        <DialogClose class="lg:mr-2">
                          <Button 
                          variant="outline"
                          size="sm"
                          >
                          Abbrechen
                        </Button>
                        </DialogClose>
                        <DialogClose>
                          <Button 
                          @click="handleDelete" 
                          :disabled="isDeleting" 
                          class="transition"
                          size="sm"  
                            >
                            <Spinner v-if="isDeleting" />
                            Löschen
                          </Button>
                        </DialogClose>
                      </DialogFooter>
                    </DialogContent>
                  </Dialog>

                  <Button
                    v-if="isAuthor"
                    @click="startEditing"
                    class="flex items-center justify-center lg:justify-start bg-transparent hover:bg-white/10 text-[#fafafa] rounded-lg p-2 lg:px-3 lg:py-2 transition-colors cursor-pointer"
                  >
                    <PencilIcon class="w-5 h-5 lg:w-4 lg:h-4 lg:mr-2 shrink-0"/>
                    <p class="hidden lg:block">Bearbeiten</p>
                  </Button>
                  <button
                    @click="copyCommentLink"
                    class="flex items-center justify-center lg:justify-start bg-transparent hover:bg-white/10 text-[#fafafa] rounded-lg p-2 lg:px-3 lg:py-2 transition-colors cursor-pointer"
                  >
                    <CopyIcon class="w-5 h-5 lg:w-4 lg:h-4 lg:mr-2 shrink-0"/>
                    <p class="hidden lg:block">Link kopieren</p>
                  </button>
                </PopoverContent>
              </PopoverPortal>
            </PopoverRoot>
          </div>
        </div>
        <div v-if="isEditingComment">
          <textarea 
            v-model="updatedText"
            rows="2"
            class="w-full rounded-md lg:text-base resize-none border border-chat-border bg-transparent text-foreground p-3 text-sm focus:ring-2 focus:ring-ring focus:outline-none"
          />
          <div class="flex justify-end gap-2 mt-2 mb-2">
            <Button 
              @click="cancelEdit" 
              variant="outline"
              size="sm"
              
              >
              <p class="text-xs lg:text-sm">Abbrechen</p>
            </Button>
            <Button 
              @click="handleUpdate" 
              :disabled="isUpdating"
              size="sm"
              >
              <Spinner v-if="isUpdating" />
              <p class="text-xs lg:text-sm">Speichern</p>
            </Button>
          </div>
        </div>

        <p v-else class="text-foreground lg:text-base text-sm mb-3 whitespace-pre-wrap break-words">{{ comment.isDeleted ? 'Dieser Kommentar wurde gelöscht.' : comment.text }}</p>
        
        <div class="flex items-center gap-1 lg:text-sm text-xs ">
          <CommentVote 
            :comment="comment"
            :isDeleted="comment.isDeleted"
          />
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
          :commentId="comment.id"
          :contentId="contentId"
          :contentType="contentType"
          :isDeleted="comment.isDeleted"
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
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, nextTick } from 'vue';
import { useSession } from '@/composables/useSession.ts';
import { useDeleteCommentMutation, useUpdateCommentMutation } from '@/composables/useComment';

import type { CommentResponseDto } from '@/types/comment.ts';

import CommentIcon  from '@/assets/comment.svg';
import ChevronDownIcon from '@/assets/chevron-down.svg';
import EllipsisIcon from '@/assets/ellipsis.svg';
import PencilIcon from '@/assets/pencil.svg';
import TrashIcon from '@/assets/trash.svg';
import CopyIcon from '@/assets/copy.svg';
import DefaultAvatar from '@/assets/default-avatar.svg?url';

import Button from '../ui/button/Button.vue';
import { Tooltip, TooltipTrigger, TooltipContent, TooltipProvider } from '@/components/ui/tooltip'
import { PopoverRoot, PopoverTrigger, PopoverPortal, PopoverContent } from 'reka-ui';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger, DialogClose } from '@/components/ui/dialog'
import Spinner from '@/components/ui/spinner/Spinner.vue';

import Reply from './Reply.vue';
import CommentReply from './CommentReply.vue';
import { toast } from 'vue-sonner';
import CommentVote from './CommentVote.vue';

const props = defineProps<{
  comment: CommentResponseDto;
  contentId: number;
  contentType: string;
}>();

//Helper für Links
const commentNode = ref<HTMLElement | null>(null);

onMounted(() => {
  // Ein kurzes Timeout gibt dem Browser Zeit für das Layout-Rendering
  setTimeout(() => {
    const expectedHash = `#comment-${props.comment.id}`;
    
    if (window.location.hash === expectedHash && commentNode.value) {
      commentNode.value.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }, 150); // 150 Millisekunden warten
});

const { data: session } = useSession()

const isAuthor = computed(() => {
  if (!session.value || !props.comment.author) return false;
  return session.value.userId === props.comment.author.id;
})

// --- Reply Logik ---

const isReplying = ref(false);
const showReplies = ref(false);

// fürs rendering der replies
const handleReplySubmit = () => {

  isReplying.value = false;
  showReplies.value = true; 
};


// helper 
const formatDate = (date: Date | string) => {
  if (typeof date === 'string') {
    date = new Date(date);
  }
  return date.toLocaleDateString('de-DE');
};

const copyCommentLink = () => {
  // Erstellt Link wie: https://deineseite.de/blog/42#comment-8a4b...
  const commentUrl = `${window.location.origin}${window.location.pathname}#comment-${props.comment.id}`;
  
navigator.clipboard.writeText(commentUrl)
    .then(() => {
      // Hier optional einen Toast oder State für "Kopiert!" anzeigen
      toast("Link in die Zwischenablage kopiert!"); 
    })
    .catch(err => {
      console.error("Fehler beim Kopieren:", err)
      toast.error("Fehler beim Kopieren des Links.");
    }
    );
};

// --- Mutations für Löschen und Aktualisieren  ---

const { mutate: deleteComment, isPending: isDeleting } = useDeleteCommentMutation();
const { mutate: updateComment, isPending: isUpdating } = useUpdateCommentMutation();
const isPopoverOpen = ref(false);
const isEditingComment = ref(false);
const updatedText = ref(props.comment.text);

const startEditing = () => {
  isEditingComment.value = true;
  isPopoverOpen.value = false; 
  updatedText.value = props.comment.text; 
};

const cancelEdit = () => {
  isEditingComment.value = false;
  updatedText.value = props.comment.text; 
};

const handleDelete = () => {
  deleteComment(props.comment.id, {
    onSuccess: () => {
      toast("Kommentar erfolgreich entfernt.");
      isPopoverOpen.value = false;
      
    },
    onError: () => {
      toast.error("Fehler beim Entfernen des Kommentars.");
      isPopoverOpen.value = false;
    }
  });
};

const handleUpdate = () => {

  if (updatedText.value.trim() === "") {
    toast.error("Der Kommentartext darf nicht leer sein.");
    return;
  }

  if (updatedText.value === props.comment.text) {
    toast("Keine Änderungen vorgenommen.");
    return;
  }
  
  updateComment({ commentId: props.comment.id, text: updatedText.value }, {
    onSuccess: () => {
      toast("Kommentar aktualisiert.");
      isEditingComment.value = false;
    }, 
    onError: () => {
      toast.error("Fehler beim Aktualisieren des Kommentars.");
      isPopoverOpen.value = false;
    }
  });

};

</script>

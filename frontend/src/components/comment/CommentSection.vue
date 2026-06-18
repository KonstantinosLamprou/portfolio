<template>
  <div :id="`comment-${comment.id}`" class="mb-6">
    <div class="flex gap-4">
      <img 
        :src="comment.IsDeleted ? DefaultAvatar : comment.author.profilePictureUrl || DefaultAvatar" 
        alt="Avatar" 
        class="w-6 h-6 rounded-full bg-chat-bg object-cover"
        referrerpolicy="no-referrer"
      />
      
      <div class="flex-1">
        <div class="flex items-center gap-2 mb-1">
          <span class="font-medium text-sm lg:text-base text-chat-text-strong">{{ comment.IsDeleted ? 'Gelöschter Benutzer' : comment.author.name }}</span>
          <time :datetime="comment.createdAt.toISOString()" class="text-xs lg:text-sm text-chat-text">{{ formatDate(comment.createdAt) }}</time>
          <div class="ml-auto">
            <PopoverRoot v-model:open="isPopoverOpen" class="ml-auto">
              <PopoverTrigger as-child>
                <button class="text-chat-text hover:text-chat-text-strong hover:bg-chat-hover p-1 rounded-full transition cursor-pointer">
                  <EllipsisIcon class="w-4 h-4"/>
                </button>
              </PopoverTrigger>
              <PopoverPortal>
                <PopoverContent
                  class="z-50 w-44 rounded-xl bg-[#1f1f1f] p-1.5 shadow-xl border border-white/10 text-sm text-[#fafafa] mt-2 outline-none"
                  :side-offset="4"
                  align="start"
                >
                  <Dialog>
                    <DialogTrigger as-child>
                      <Button
                        v-if="isAuthor"
                        class="flex w-full items-center bg-[#1f1f1f] text-[#fafafa] justify-between rounded-lg px-3 py-2 text-left hover:bg-white/10 transition-colors cursor-pointer"
                      >
                        <TrashIcon class="w-4 h-4 mr-2"/>
                        Löschen
                      </Button>
                    </DialogTrigger>
                    <DialogContent>
                      <DialogHeader>
                        <DialogTitle>Kommentar löschen</DialogTitle>
                        <DialogDescription>Willst du diesen Kommentar wirklich löschen? Diese Aktion kann nicht rückgängig gemacht werden.</DialogDescription>
                      </DialogHeader>
                      <DialogFooter>
                        <DialogClose class="mr-2">
                          <Button variant="outline">Abbrechen</Button>
                        </DialogClose>
                        <DialogClose>
                          <Button @click="handleDelete" :disabled="isDeleting" class="px-4 py-2 rounded-md transition">
                            <Spinner v-if="isDeleting" />
                            Löschen
                          </Button>
                        </DialogClose>
                      </DialogFooter>
                    </DialogContent>
                  </Dialog>
                  <Dialog>
                    <DialogTrigger as-child>
                      <Button
                        v-if="isAuthor"
                        class="flex w-full items-center bg-[#1f1f1f] text-[#fafafa] justify-between rounded-lg px-3 py-2 text-left hover:bg-white/10 transition-colors cursor-pointer"
                      >
                        <PencilIcon class="w-4 h-4 mr-2"/>
                        Aktualisieren
                      </Button>
                    </DialogTrigger>
                    <DialogContent>
                      <DialogHeader>
                        <DialogTitle>Kommentar aktualisieren</DialogTitle>
                        <DialogDescription>Hier kannst du deinen Kommentar bearbeiten und aktualisieren.</DialogDescription>
                      </DialogHeader>
                      <Input 
                        :disabled="isUpdating" 
                        class="mb-4" 
                        v-model="updatedText"/>
                      <DialogFooter>
                        <DialogClose class="mr-2">
                          <Button variant="outline">Abbrechen</Button>
                        </DialogClose>  
                        <DialogClose>
                          <Button @click="handleUpdate" :disabled="isUpdating">
                            <Spinner v-if="isUpdating"/>
                            Aktualisieren
                          </Button>
                        </DialogClose>
                      </DialogFooter>
                    </DialogContent>
                  </Dialog>

                  <button
                    @click="copyCommentLink"
                    class="flex w-full items-center justify-between rounded-lg px-3 py-2 text-left hover:bg-white/10 transition-colors cursor-pointer"
                  >
                    <CopyIcon class="w-4 h-4 mr-2"/>
                    Link kopieren
                  </button>
                </PopoverContent>
              </PopoverPortal>
            </PopoverRoot>
          </div>
        </div>
        
        <p class="text-foreground lg:text-base text-sm mb-3 pr-3 whitespace-pre-wrap">{{ comment.IsDeleted ? 'Dieser Kommentar wurde gelöscht.' : comment.text }}</p>
        
        <div class="flex items-center gap-1 lg:text-sm text-xs ">
          <button class="flex items-center gap-1.5 px-3 py-1.5 text-chat-text hover:bg-chat-hover hover:text-chat-text-strong rounded-full transition cursor-pointer">
            <ThumbsUpIcon class="w-4 h-4" />
            <span v-if="comment.upvotes > 0 ? comment.upvotes : '0'">{{ comment.IsDeleted ? '0' : comment.upvotes }}</span>
          </button>
          
          <button class="flex items-center gap-1.5 px-3 py-1.5 text-chat-text hover:bg-chat-hover hover:text-chat-text-strong rounded-full transition cursor-pointer">
            <ThumbsDownIcon class="w-4 h-4" />
            <span v-if="comment.downvotes > 0 ? comment.downvotes : '0'">{{ comment.IsDeleted ? '0' : comment.downvotes }}</span>
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
import { ref, computed } from 'vue';
import { useSession } from '@/composables/useSession.ts';
import { useDeleteCommentMutation, useUpdateCommentMutation } from '@/composables/useComment';

import type { CommentResponseDto } from '@/types/comment.ts';
import type { CommentResponseDtoExtended } from './Commentwrapper.vue';

import ThumbsUpIcon  from '@/assets/thumb-up.svg';
import ThumbsDownIcon  from '@/assets/thumb-down.svg';
import CommentIcon  from '@/assets/comment.svg';
import ChevronDownIcon from '@/assets/chevron-down.svg';
import EllipsisIcon from '@/assets/ellipsis.svg';
import PencilIcon from '@/assets/pencil.svg';
import TrashIcon from '@/assets/trash.svg';
import CopyIcon from '@/assets/copy.svg';
import DefaultAvatar from '@/assets/default-avatar.svg?url';

import Input from '../ui/input/Input.vue';
import Button from '../ui/button/Button.vue';
import { PopoverRoot, PopoverTrigger, PopoverPortal, PopoverContent } from 'reka-ui';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger, DialogClose } from '@/components/ui/dialog'
import Spinner from '@/components/ui/spinner/Spinner.vue';

import Reply from './Reply.vue';
import CommentReply from './CommentReply.vue';
import { toast } from 'vue-sonner';

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


const { data: session } = useSession()

const isAuthor = computed(() => {
  if (!session.value || !props.comment.author) return false;
  return session.value.userId === props.comment.author.id;
})

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

const isPopoverOpen = ref(false);

const { mutate: deleteComment, isPending: isDeleting } = useDeleteCommentMutation();
const { mutate: updateComment, isPending: isUpdating } = useUpdateCommentMutation();

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


const updatedText = ref(props.comment.text);

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
      isPopoverOpen.value = false;
    }, 
    onError: () => {
      toast.error("Fehler beim Aktualisieren des Kommentars.");
      isPopoverOpen.value = false;
    }
  });

};

</script>

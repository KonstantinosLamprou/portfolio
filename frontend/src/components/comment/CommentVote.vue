<template>
  <div class="flex items-center gap-1 lg:text-sm text-xs">
    <button
        @click="handleVote(true)"
        :class="[
          'flex items-center gap-1.5 px-3 py-1.5 rounded-full transition cursor-pointer',
          currentUserVote === true 
            ? 'text-blue-500 bg-blue-500/10 hover:bg-blue-500/20' 
            : 'text-chat-text hover:bg-chat-hover hover:text-chat-text-strong'
        ]"
    >
      <ThumbsUpIcon :class="['w-4 h-4', currentUserVote === true ? 'fill-current' : '']" />
      <span>{{ props.comment.isDeleted ? '0' : props.comment.upvotes }}</span>
    </button>
    
    <button 
        @click="handleVote(false)"
        :class="[
          'flex items-center gap-1.5 px-3 py-1.5 rounded-full transition cursor-pointer',
          currentUserVote === false 
            ? 'text-red-500 bg-red-500/10 hover:bg-red-500/20' 
            : 'text-chat-text hover:bg-chat-hover hover:text-chat-text-strong'
        ]"
    >
      <ThumbsDownIcon :class="['w-4 h-4', currentUserVote === false ? 'fill-current' : '']" />
      <span>{{ props.comment.isDeleted ? '0' : props.comment.downvotes }}</span>
    </button>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import ThumbsUpIcon from '@/assets/thumb-up.svg';
import ThumbsDownIcon from '@/assets/thumb-down.svg';
import type { CommentResponseDto } from '@/types/comment';
import { 
  useUpdateCommentVoteMutation, 
  useCreateCommentVoteMutation,
  useDeleteCommentVoteMutation 
} from '@/composables/useCommentVote';
import { toast } from 'vue-sonner';
import { useSession } from '@/composables/useSession';

const { data: session, isPending: isSessionLoading } = useSession()
const isAuthenticated = computed(() => !!session.value && !isSessionLoading.value)

const props = defineProps<{
    comment: CommentResponseDto
}>();

// true = Upvote, false = Downvote, null = Keine Stimme
const currentUserVote = ref<boolean | null | undefined>(props.comment.currentUserVote);
    
const { mutate: createVote } = useCreateCommentVoteMutation();
const { mutate: updateVote } = useUpdateCommentVoteMutation();
const { mutate: deleteVote } = useDeleteCommentVoteMutation();

const handleVote = (isUpvote: boolean) => {
    const commentId = props.comment.id;
    const contentId = props.comment.contentId;
    

    if (!isAuthenticated.value) {
        toast.error('Bitte melde dich an, um abzustimmen.');
        return;
    }

    if (props.comment.isDeleted || props.comment.isDeleted) {
        toast.info("Gelöschte Kommentare können nicht bewertet werden.");
        return;
    }

    if (currentUserVote.value === isUpvote) {
        deleteVote({commentId: commentId, contentId}, {
            onSuccess: () => {
                currentUserVote.value = null;
                toast.success("Stimme erfolgreich gelöscht");
            }, 
            onError: () => {
                toast.error("Fehler beim Löschen der Stimme");
            }
        });
    } else if (currentUserVote.value !== null) {
        updateVote({ commentId, isUpvote, contentId}, {
            onSuccess: () => {
                currentUserVote.value = isUpvote;
                toast.success("Stimme erfolgreich aktualisiert");
            }, 
            onError: () => {
                toast.error("Fehler beim Aktualisieren der Stimme");
            }
        });
    } else {
        createVote({ commentId, isUpvote, contentId }, {
            onSuccess: () => {
                currentUserVote.value = isUpvote;
                toast.success("Stimme erfolgreich abgegeben");
            }, 
            onError: () => {
                toast.error("Fehler beim Abgeben der Stimme");  
            }
        });
    }
};
</script>
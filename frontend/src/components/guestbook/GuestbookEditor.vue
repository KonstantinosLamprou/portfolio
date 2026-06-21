<template>
    <div class="flex justify-center w-full">
        <form class="flex w-full items-center flex-col mt-10 space-y-2" @submit.prevent="submitEntry">
            <div class="flex w-full lg:w-2/3 gap-3">
                <div class="shrink-0 pt-1"> 
                    <img 
                        :src="session?.profilePictureUrl || DefaultAvatar"
                        alt="Avatar" 
                        class="w-8 h-8 rounded-full bg-chat-bg object-cover"
                        referrerpolicy="no-referrer"
                    />
                </div>
                <div class="w-full">   
                    <InputGroup
                        class="rounded-xl border border-[color:var(--border)] bg-[color:var(--surface-color)] shadow-[0_6px_20px_rgba(0,0,0,0.25)]"
                    >
                        <InputGroupTextarea
                            ref="textareaRef"
                            v-model="content"
                            class="min-h-24 w-full resize-none px-4 py-2 lg:text-base text-sm text-[color:var(--foreground)] placeholder:text-[color:var(--muted-foreground)] leading-relaxed"
                            :placeholder="placeholder"
                        />
                    </InputGroup>
                </div> 
            </div>

            <div class="flex justify-end w-full lg:w-2/3 gap-2">
                <Button
                    v-if="!isAuthenticated"
                    @click="dialogState.openDialog"
                    type="button"
                    variant="none" 
                    class="self-end mb-1 rounded-xl border"
                >
                    <SignInicon class="w-4 h-4 mr-2" />
                    Sign In
                </Button>
                <Button 
                    v-else
                    @click="handleLogout"
                    variant="none"
                    type="button"
                    class="self-end mb-1 rounded-xl border"
                >
                <SignOutIcon v-if="!isLogoutLoading" class="w-4 h-4 mr-2" />
                <Spinner v-else size="sm" class="mx-auto mr-2"/>
                    Sign Out
                </Button>
                <Button 
                    variant="none"
                    type="submit"
                    class="self-end mb-1 rounded-xl border"
                    :disabled="!isAuthenticated || content.trim() === ''"
                >
                    Absenden
                </Button>
            </div> 
        </form>              
    </div>
</template>

<script setup lang="ts">
import { InputGroup, InputGroupTextarea } from '@/components/ui/inputGroup';
import { ref } from 'vue';
import { useSession } from '@/composables/useSession';
import { computed } from 'vue';
import DefaultAvatar from '@/assets/default-avatar.svg?url';
import Button from '../ui/buttons/Button.vue';
import SignInicon from '@/assets/login.svg';
import { useCreateGuestbookEntry } from '@/composables/useGuestbook';
import type { CreateGuestbookEntryRequest } from '@/types/GuestbookTypes';
import SignOutIcon from '@/assets/sign-out.svg';
import { toast } from 'vue-sonner';
import { useSignInDialogStore } from "@/stores/useSignInDialogStore"
import { useLogout } from "@/composables/useLogout"
import Spinner from '@/components/ui/spinner/Spinner.vue';

const  dialogState  = useSignInDialogStore()
const placeholder = "Hinterlasse hier einen netten Gruß oder Feedback! :)";
const content = ref('');

const { data: session, isPending: isSessionLoading } = useSession(); 
const isAuthenticated = computed(() => !!session.value && !isSessionLoading.value); 

const { mutate: createEntry } = useCreateGuestbookEntry();

const submitEntry = () => {
    if (!isAuthenticated.value) {
        toast.error('Bitte melde dich an, bevor du einen Eintrag hinzufügen kannst.');
        return;
    }

    if (content.value.trim() === '') {
        toast.error('Bitte gib eine Nachricht ein, bevor du sie absendest.');
        return;
    }

    const payload: CreateGuestbookEntryRequest = {
        message: content.value.trim()
    };

    createEntry(payload, {
        onSuccess: () => {
            toast.success('Dein Eintrag wurde erfolgreich hinzugefügt!');
            content.value = '';
        },
        onError: (error) => {
            toast.error('Beim Hinzufügen deines Eintrags ist ein Fehler aufgetreten. Bitte versuche es später erneut.');
            console.error('Fehler beim Einreichen des Gästebucheintrags:', error);
        },
    });
};

const { mutate: logout, isPending: isLogoutLoading } = useLogout();

const handleLogout = () => {
  logout(
    undefined,
    {
      onSuccess: () => {
        toast.success('Erfolgreich ausgeloggt!');
      },
      onError: (error) => {
        console.error('Logout-Fehler:', error);
        toast.error('Beim Logout ist ein Fehler aufgetreten. Bitte versuche es später erneut.');
      },
    }
  );
};
</script>   
<template>
    <div class="flex justify-center w-full">
        <div class="flex w-full items-center flex-col mt-10 space-y-2">
            
            <div class="flex w-2/3 gap-3">
                <div class="shrink-0 pt-1"> <img 
                        :src="DefaultAvatar"
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

            <div class="flex justify-end w-2/3 gap-2">
                <Button
                    v-if="isAuthenticated"
                    variant="none" 
                    class="self-end mb-1 rounded-xl border"
                >
                    <SignInicon class="w-4 h-4 mr-2" />
                    Sign In
                </Button>
                <Button 
                    v-else
                    variant="none"
                    class="self-end mb-1 rounded-xl border"
                >
                    Sign Out
                </Button>
                <Button 
                    variant="none"
                    class="self-end mb-1 rounded-xl border"
                    :disabled="!isAuthenticated || content.trim() === ''"
                >
                    Absenden
                </Button>
            </div>               
        </div>
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

const placeholder = "Hinterlasse hier einen netten Gruß oder Feedback! :)";
const content = ref('');

const { data: session, isPending: isSessionLoading } = useSession(); 
const isAuthenticated = computed(() => !!session.value && !isSessionLoading.value); 




</script>   
<template>
    <div class="mt-12 justify-center flex">
        <div class="p-5 rounded-lg bg-surface/50 border border-surface/50 w-full lg:w-2/3">
            <div class=" flex justify-between ">
                <div class="flex flex-row gap-3 mb-2">
                    <MessageCircleIcon class="w-8 h-8 text-foreground/70" />
                    <h1 class="font-bold text-lg text-foreground text-center">Hey, was geht ab?</h1> 
                </div>
                <div>
                    <PinIcon class="w-6 h-6 text-foreground/70" />
                </div>

            </div>
            <div>
                <p class="text-foreground/70 text-base mt-2">
                    Erstmal danke für das Besuchen meiner Website. 
                    Wenn du einen Moment Zeit hast hinterlasse doch gerne einen Gruß oder Feedback da. 
                    Ich freue mich über jede Nachricht! <br> Danke, Kostas
                </p>  
            </div>

        </div>
    </div>
</template>

<script setup lang="ts">
import MessageCircleIcon from '@/assets/message-circle.svg';
import PinIcon from '@/assets/pin.svg';
</script>
<template>
  <div class="absolute inset-0 flex items-start pt-13 justify-center rounded-xl bg-main/20 backdrop-blur-[2px]">
    <Button 
      type="button"
      variant="none"
      size="nosize"
      @click="dialogState.openDialog"
      class="glass3d rounded-xl text-center items-center text-nowrap overflow-hidden inline-block outline-none cursor-pointer"
     >
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
        className="px-5 py-1 shadow-sm flex items-center text-sm"
      
      >
      <LoginIcon class="w-4 h-4 mr-2"/>
        Sign In     
      </GlareHover>
    </Button>
  </div>
</template>

<script setup lang="ts">
import { Button } from "@/components/ui/buttons"
import { useSignInDialogStore } from "@/stores/useSignInDialogStore"
import GlareHover from "../ui/GlareHover.vue";
import LoginIcon from '@/assets/login.svg';

const  dialogState  = useSignInDialogStore()

</script>

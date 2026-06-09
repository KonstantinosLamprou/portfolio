<template>
  <div class="absolute inset-0 flex items-start pt-13 justify-center rounded-xl bg-main/20 backdrop-blur-[2px] text-gray-300">
    <Button 
      type="button"
      size="nosize"
      @click="dialogState.openDialog"
      class="glass3d rounded-full text-center text-nowrap overflow-hidden inline-block"
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
        className="rounded-full px-5 py-1 shadow-sm flex items-center text-sm"
      
      >
        Sign In     
      </GlareHover>
    </Button>
  </div>
</template>

<script setup lang="ts">
import { Button } from "@/components/ui/buttons"
import { useSignInDialogStore } from "@/stores/useSignInDialogStore"
import GlareHover from "../ui/GlareHover.vue";

const  dialogState  = useSignInDialogStore()

</script>

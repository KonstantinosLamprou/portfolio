<template>
  <div class="absolute inset-0 flex items-center justify-center rounded-xl bg-main/20 backdrop-blur-[2px]">
    <Button size="sm" class="rounded-full bg-heading text-main px-4 py-2 shadow-sm hover:bg-heading/80" @click="openDialog" >
        Sign in
    </Button>
  </div>
</template>

<script setup lang="ts">
import { Button } from "@/components/ui/buttons"
import { useSignInDialogStore } from "@/stores/useSignInDialogStore"
const { openDialog } = useSignInDialogStore()
</script>

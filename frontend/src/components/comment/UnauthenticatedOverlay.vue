<template>
  <div class="absolute inset-0 flex items-center justify-center rounded-md bg-black/30 backdrop-blur-sm">
    <Button size="sm" class="rounded-full bg-[color:var(--bright)] text-black px-4 py-2 shadow-sm" @click="openDialog">
        Sign in
    </Button>
  </div>
</template>

<script setup lang="ts">
import { Button } from "@/components/ui/buttons"
import { useSignInDialogStore } from "@/stores/useSignInDialogStore"
const { openDialog } = useSignInDialogStore()
</script>

// useSignInDialogStore.ts
import { defineStore } from "pinia"
import { ref } from "vue"

export const useSignInDialogStore = defineStore("signInDialog", () => {
  const open = ref(false)

  const openDialog = () => {
    open.value = true
  }

  const closeDialog = () => {
    open.value = false
  }

  return { open, openDialog, closeDialog }
})
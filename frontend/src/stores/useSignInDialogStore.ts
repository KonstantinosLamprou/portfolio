// useSignInDialogStore.ts
import { defineStore } from "pinia"
import { ref } from "vue"

export const useSignInDialogStore = defineStore("signInDialog", () => {
  const open = ref(false)

  const openDialog = () => {
    console.log("1. Button wurde geklickt! Alter State:", open.value)
    open.value = true
    console.log("2. State wurde geändert auf:", open.value)
  }

  const closeDialog = () => {
    open.value = false
  }

  return { open, openDialog, closeDialog }
})
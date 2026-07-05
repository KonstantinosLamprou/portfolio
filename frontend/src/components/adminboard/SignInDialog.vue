<template>
  <div
    v-if="dialogState.open"
    class="fixed inset-0 z-[9999] flex items-center justify-center bg-main/40 backdrop-blur-sm"
    @click.self="dialogState.closeDialog"
  >
    <div class="w-full m-4 max-w-md rounded-xl bg-drawer-bg p-6 text-drawer-text shadow-xl">
      <div class="flex items-start justify-between">
        <div>
          <div class="text-xl font-semibold">Sign in</div>
          <div class="text-sm text-drawer-text-muted">fahre fort mit</div>
        </div>

        <button
          type="button"
          class="text-drawer-text-muted hover:text-drawer-text"
          @click="dialogState.closeDialog"
          aria-label="Close"
        >
          <CancelIcon class="w-5 h-5"/>
        </button>
      </div>

      <div class="mt-6 space-y-3">
        <button
          type="button"
          class="flex w-full text-sm lg:text-base justify-center border-drawer-border border rounded-full bg-drawer-card px-4 py-2 font-medium"
          @click="startOAuth('github')"
        >
          <GithubIcon class="w-5 h-5 mr-2"/>
          GitHub
        </button>

        <button
          type="button"
          class="flex w-full text-sm lg:text-base rounded-full border justify-center text-center border-drawer-border px-4 py-2 font-medium"
          @click="startOAuth('google')" 
        >
          <GoogleIcon class="w-5 h-5 mr-2"/>
          Google
        </button>
      </div>
      <div class="mt-6 text-center">
        <p class="text-xs text-drawer-text-muted">
          Mit der Anmeldung über einen Drittanbieter stimmst du den 
          <RouterLink to="/terms-of-service" @click="dialogState.closeDialog" class="text-link hover:text-sky hover:underline">Nutzungsbedingungen</RouterLink>
          und der 
          <RouterLink to="/privacy-policy" @click="dialogState.closeDialog" class="text-link hover:text-sky hover:underline">Datenschutzerklärung</RouterLink> zu.
        </p>
      </div>
  </div>
</div>
</template>

<script setup lang="ts">
import { useSignInDialogStore } from "@/stores/useSignInDialogStore"
import apiClient from "@/services/api"
import CancelIcon from "@/assets/cancel.svg"
import GithubIcon from "@/assets/github.svg"
import GoogleIcon from "@/assets/google.svg"

const dialogState  = useSignInDialogStore()

const startOAuth = (provider: "github" | "google") => {
    // Merken, wo der User gerade ist (Pfad + Parameter + Anker)
    const returnUrl = window.location.pathname + window.location.search + window.location.hash;
    
    // Im Browser zwischenspeichern
    sessionStorage.setItem('auth:returnUrl', returnUrl);

    sessionStorage.setItem('auth:scrollY', window.scrollY.toString());

    // Ab zum Backend-Login!
    window.location.href = `${apiClient}/auth/${provider}/login`;
}
</script>
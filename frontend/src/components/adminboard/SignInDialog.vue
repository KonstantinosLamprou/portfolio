<template>
  <div
    v-if="dialogState.open"
    class="fixed inset-0 z-[9999] flex items-center justify-center bg-main/40 backdrop-blur-sm"
    @click.self="dialogState.closeDialog"
  >
    <div class="w-full max-w-md rounded-xl bg-neutral-900 p-6 text-white shadow-xl">
      <div class="flex items-start justify-between">
        <div>
          <div class="text-xl font-semibold">Sign in</div>
          <div class="text-sm text-white/70">to continue</div>
        </div>

        <button
          type="button"
          class="text-white/60 hover:text-white"
          @click="dialogState.closeDialog"
          aria-label="Close"
        >
          X
        </button>
      </div>

      <div class="mt-6 space-y-3">
        <button
          type="button"
          class="w-full rounded-full bg-white text-black px-4 py-2 font-medium"
          @click="startOAuth('github')"
        >
          Continue with GitHub
        </button>

        <button
          type="button"
          class="w-full rounded-full border border-white/20 px-4 py-2 font-medium"
          @click="startOAuth('google')"
        >
          Continue with Google
        </button>
      </div>

      <p class="mt-4 text-xs text-white/60">
        By continuing, you agree to our Terms of Service and Privacy Policy
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useSignInDialogStore } from "@/stores/useSignInDialogStore"

const dialogState  = useSignInDialogStore()


const startOAuth = (provider: "github" | "google") => {
  // 1. Merken, wo der User gerade ist (Pfad + Parameter + Anker)
  const returnUrl = window.location.pathname + window.location.search + window.location.hash;
  
  // Im Browser zwischenspeichern
  sessionStorage.setItem('auth:returnUrl', returnUrl);

  sessionStorage.setItem('auth:scrollY', window.scrollY.toString());

  // 2. Ab zum Backend-Login!
  window.location.href = `http://localhost:5132/api/auth/${provider}/login`;
}

</script>
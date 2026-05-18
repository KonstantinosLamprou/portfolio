<template>
      <!-- Fehler vom Composable anzeigen -->
      <div class="min-h-14 mt-10">
        <div
          v-if="displayedError"
          class="p-3 bg-red-100 text-red-700 rounded-md text-sm"
        >
          {{ authError }}
        </div>
      </div>

      <div class="">

        <LoginForm
        :is-loading="authLoading"
        @login="handleLogin"
        />

      </div>
      <p v-if="authLoading" class="flex flex-col items-center"><Loader /></p>
</template>

<script setup lang="ts">
import { ref, watch, onUnmounted } from 'vue'
import LoginForm from '../components/adminboard/AdminLogin.vue'
import Loader from '@/components/loaders/ArcadeLoader.vue'
import { useAuth } from '@/composables/useAuth'
import type { LoginCredentials } from '@/types/auth'

const { login, isLoading: authLoading, error: authError } = useAuth()

// Lokale Kopie, die nach Zeit ausgeblendet wird
const displayedError = ref<string | null>(null)
let errorTimer: ReturnType<typeof setTimeout> | null = null

// Bei neuem Fehler: alten Timer löschen, neuen Fehler anzeigen und Timer starten
watch(authError, (newError) => {
  if (errorTimer) {
    clearTimeout(errorTimer)
    errorTimer = null
  }
  displayedError.value = newError
  if (newError) {
    errorTimer = setTimeout(() => {
      displayedError.value = null
      errorTimer = null
    }, 5000) // verschwindet nach 5 Sekunden
  }
})

// Timer beim Verlassen der Seite aufräumen
onUnmounted(() => {
  if (errorTimer) clearTimeout(errorTimer)
})

async function handleLogin(credentials: LoginCredentials) {
  await login(credentials)
  // Falls erfolgreich, wird in useAuth automatisch weitergeleitet
  // Falls Fehler, wird authError gesetzt und hier angezeigt
}
</script>

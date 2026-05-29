<template>
  <div class="flex h-[50vh] w-full items-center justify-center">
    <div class="flex flex-col items-center gap-4 text-white/70">
      <div class="text-lg">Anmeldung wird abgeschlossen...</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

onMounted(() => {
  // 1. Schau nach, ob wir uns vor dem Login eine Seite gemerkt haben. 
  // Fallback ist die Startseite ('/'), falls nichts gefunden wird.
  const returnUrl = sessionStorage.getItem('auth:returnUrl') || '/'

  // 2. Räume den Speicher sofort wieder auf, damit es sauber bleibt
  sessionStorage.removeItem('auth:returnUrl')

  // 3. Schicke den User exakt dorthin zurück!
  router.push(returnUrl)
})
</script>
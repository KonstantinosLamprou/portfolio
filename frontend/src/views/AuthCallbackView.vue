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
  // Schau nach, ob wir uns vor dem Login eine Seite gemerkt haben. 
  // Fallback ist die Startseite ('/'), falls nichts gefunden wird.
  const returnUrl = sessionStorage.getItem('auth:returnUrl') || '/'
  const savedScrollY = sessionStorage.getItem('auth:scrollY')

  // Räume den Speicher sofort wieder auf, damit es sauber bleibt
  sessionStorage.removeItem('auth:returnUrl')

router.push(returnUrl).then(() => {
    
    // Wenn wir eine Scroll-Position gespeichert haben...
    if (savedScrollY) {
      // Warten wir kurz, bis Vue und TanStack Query das HTML aufgebaut haben
      setTimeout(() => {
        window.scrollTo({
          top: parseInt(savedScrollY), // String zurück in eine Zahl wandeln
          behavior: 'smooth'           // Erzeugt einen geschmeidigen Scroll-Effekt
        });
        
        // aufräumen
        sessionStorage.removeItem('auth:scrollY');
      }, 500); 
    }
    
  })
})
</script>
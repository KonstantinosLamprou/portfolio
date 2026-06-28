<template>
  <div class="flex h-[50vh] w-full items-center justify-center">
    <div class="flex flex-col items-center gap-4 text-white/70">
      <div class="text-lg">Anmeldung wird abgeschlossen...</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { toast } from 'vue-sonner'

const route = useRoute()
const router = useRouter()

onMounted(() => {
  
  const returnUrl = sessionStorage.getItem('auth:returnUrl') || '/'
  const savedScrollY = sessionStorage.getItem('auth:scrollY')

  sessionStorage.removeItem('auth:returnUrl')
  sessionStorage.removeItem('auth:scrollY')

  if (route.query.error) {
    toast.error("Die Anmeldung ist fehlgeschlagen oder abgebrochen worden.")
    
    router.replace(returnUrl) 
    return 
  }
  
  toast.success("Du hast dich erfolgreich angemeldet.")
  
  router.replace(returnUrl).then(() => {
    
    if (savedScrollY) {
      setTimeout(() => {
        window.scrollTo({
          top: parseInt(savedScrollY), 
          behavior: 'smooth'           
        })
      }, 500) 
    }
    
  })
})
</script>
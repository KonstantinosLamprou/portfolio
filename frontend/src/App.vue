
<template>
  <SplashScreen />

  <section class="relative overflow-clip bg-main text-body transition-colors duration-300">
    <BackgroundGradient position="top"/>
    <BackgroundGradient position="bottom"/>
    <DefaultLayout>
        <RouterView />

      </DefaultLayout>

    </section>

    <Toaster position="bottom-right" />


</template>

<script setup lang="ts">
import DefaultLayout from './layouts/DefaultLayout.vue';
import { onMounted } from 'vue'
import { useTheme } from './composables/useTheme'
import SplashScreen from './components/ui/SplashScreen.vue';
import BackgroundGradient from './components/ui/BackgroundGradient.vue'
import { Toaster } from 'vue-sonner'
import 'vue-sonner/style.css'


const { initTheme } = useTheme()

// Führt die Prüfung direkt beim Laden der App aus
onMounted(() => {
  initTheme()
})
</script>

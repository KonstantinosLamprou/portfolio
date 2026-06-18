<template>
  <Transition name="fade">
    <div
      v-if="showOverlay"
      class="fixed inset-0 z-100 flex flex-col items-center justify-center bg-background text-body transition-colors duration-300"
      @click="handleScreenClick"
      :class="{ 'cursor-pointer': phase === 'welcome' }"
    >

      <Transition name="fade" mode="out-in">

        <div v-if="phase === 'loading'" class="flex flex-col items-center">
          <Loader />
        </div>

        <div v-else-if="phase === 'welcome'" class="flex flex-col items-center text-center ">

            <TextType
              className="max-w-fit text-lg md:text-5xl min-h-10 "
              :text="currentTagline || 'Willkommen'"
              :typingSpeed="45"
              :pauseDuration="5000"
              :loop="false"
              :variableSpeed="{ min: 40, max: 90 }"
            />

          <p class="animate-blink absolute bottom-12 left-0 right-0 text-center text-lg opacity-70">
            Klicke irgendwo, um fortzufahren
          </p>
        </div>

      </Transition>

    </div>
  </Transition>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted} from 'vue'
import Loader from './loaders/ArcadeLoader.vue'
import TextType from './TextType.vue'

// Zustände
const showOverlay = ref(true)
const phase = ref<'loading' | 'welcome'>('loading')

onMounted(() => {
  // Checken, ob wir das Intro heute schon gezeigt haben
  const hasSeenIntro = sessionStorage.getItem('hasSeenIntro')

  if (hasSeenIntro) {
    // Wenn ja, sofort verstecken, kein Loading zeigen!
    showOverlay.value = false
  } else {
    // Wenn nein, zeige den Loader für 2 Sekunden
    setTimeout(() => {
      phase.value = 'welcome'
      startTaglineRotation()
    }, 2000)
  }
})

const handleScreenClick = () => {
  if (phase.value === 'welcome') {
    showOverlay.value = false
    // Nach dem Klick speichern wir: "Der User hat das Intro gesehen!"
    sessionStorage.setItem('hasSeenIntro', 'true')
  }
}

const WelcomeTaglines = ['Willkommen!', 'Bienvenue!', 'Bienvenido!', 'Welcome!', 'Benvenuto!']
const currentTagline = ref(WelcomeTaglines[0])
const currentIndex = ref(0)
let timer: ReturnType<typeof setInterval> | null = null

const startTaglineRotation = () => {
  timer = setInterval(() => {
    // Erhöhe den Index und nutze Modulo (%), um wieder bei 0 anzufangen
    currentIndex.value = (currentIndex.value + 1) % WelcomeTaglines.length
    currentTagline.value = WelcomeTaglines[currentIndex.value]
  }, 5000) // Alle 3 Sekunden wechseln
}

onUnmounted(() => {
  if (timer) clearInterval(timer)
})

</script>

<style scoped>
/* Vue's magische Transition-Klassen für ein weiches Ein- und Ausblenden */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.8s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>

<template>
  <div class="relative z-1">
    <div class="container mt-25 mb-3 flex min-h-[75vh] flex-col items-center justify-center sm:mt-0">
      <div class="container flex w-full flex-col items-center justify-center md:mt-45">
        <p class="text-lg">
          <span class="text-lg text-onlydarkmatcha">Hey, </span>
          ich heiße
        </p>
        <h1 class="mb-5 text-center text-5xl md:text-6xl">
          Konstantinos Lamprou
        </h1>
        <div class="inline-block text-center whitespace-pre-wrap tracking-tight mb-8 max-w-fit text-sm md:text-3xl">
          <span v-if="isLoading" class="flex items-center justify-center gap-3 animate-pulse text-gray-500">
              DeepSeek denkt nach...
              <SpinnerLoader />
          </span>

          <TextType
              v-else
              className="max-w-fit text-sm md:text-3xl min-h-10"
              :text="tagline"
              :typingSpeed="45"
              :pauseDuration="5000"
              :loop="false"
              :variableSpeed="{ min: 40, max: 90 }"
            />
          <!--
          <span v-else class="inline transition-all duration-500">
              {{ tagline }}
          </span> -->
        </div>

        <div class="mb-4 flex flex-col items-center justify-center gap-5 md:flex-row z-1">

          <RouterLink
            to="/lebenslauf"
            aria-label="my projects"
            class="glass3d rounded-full text-nowrap overflow-hidden inline-block"
          >
            <GlareHover
              glareColor="#ffffff"
              :glareOpacity="0.3"
              :glareAngle="-30"
              :glareSize="300"
              :transitionDuration="800"
              :playOnce="false"
              width="100%"
              height="100%"
              background="transparent"
              borderRadius="9999px"
              borderColor="transparent"
              className="px-7 py-3"
            >
              Lebenslauf
            </GlareHover>
          </RouterLink>


          <RouterLink
            to="/projekte"
            aria-label="my projects"
            class="glass3d rounded-full text-nowrap overflow-hidden inline-block"
          >
            <GlareHover
              glareColor="#ffffff"
              :glareOpacity="0.3"
              :glareAngle="-30"
              :glareSize="300"
              :transitionDuration="800"
              :playOnce="false"
              width="100%"
              height="100%"
              background="transparent"
              borderRadius="9999px"
              borderColor="transparent"
              className="px-7 py-3"
            >
              Meine Projekte
            </GlareHover>
          </RouterLink>

          <button
            @click="fetchTagline"
            :disabled="isLoading"
            class="glass3d rounded-full text-nowrap overflow-hidden inline-block"
            :class="{ 'opacity-50 cursor-not-allowed': isLoading }"
            aria-label="Generate new line"
          >
            <GlareHover
              glareColor="#ffffff"
              :glareOpacity="0.3"
              :glareAngle="-30"
              :glareSize="300"
              :transitionDuration="800"
              :playOnce="false"
              width="100%"
              height="100%"
              background="transparent"
              borderRadius="9999px"
              borderColor="transparent"
              className="px-7 py-3"
            >
            <div v-if="isLoading" class="flex items-center justify-center gap-3 ">
              Generiere...
              <SpinnerLoader />
            </div>
            <div v-else>
              Generiere einen neuen Satz
            </div>
            </GlareHover>

          </button>

          <button 
              @click="dialogState.openDialog"
              class="glass3d rounded-full text-center text-nowrap overflow-hidden inline-block"
              aria-label="Sign-In"
            >
              <GlareHover
                glareColor="#ffffff"
                :glareOpacity="0.3"
                :glareAngle="-30"
                :glareSize="300"
                :transitionDuration="800"
                :playOnce="false"
                width="100%"
                height="100%"
                background="transparent"
                borderRadius="9999px"
                borderColor="transparent"
                className="px-7 py-3"
              
              >
                Sign In     
              </GlareHover>
            </button>

        </div>

        <div class="mt-4 flex w-full flex-col items-center justify-center">
          <p class="mb-2">
            Derzeitiges KI-Modell im Einsatz:
          </p>
          <div class="relative w-64">
            <button class="glass3d flex w-full items-center justify-between rounded-full px-4 py-2 shadow-sm transition hover:border-gray-400">
              <div class="flex w-full items-center justify-center">
                DeepSeek
              </div>
              </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink } from 'vue-router'
import GlareHover from '@/components/ui/GlareHover.vue'
import SpinnerLoader from '@/components/ui/loaders/SpinnerLoader.vue'
import TextType from '@/components/ui/TextType.vue'
import { Button } from "@/components/ui/buttons"
import { useSignInDialogStore } from "@/stores/useSignInDialogStore"

const  dialogState  = useSignInDialogStore()


// 1. Die reaktiven Variablen
const tagline = ref('Ich baue Dinge für das Web. Manchmal funktionieren sie sogar.')
const isLoading = ref(false)

// 2. Das Fake Backend für die Design-Phase
const fetchTagline = async () => {
  if (isLoading.value) return
  isLoading.value = true

  try {
    // Wir simulieren, dass das .NET Backend 2 Sekunden zum Antworten braucht
    await new Promise(resolve => setTimeout(resolve, 2000))

    // Ein paar lustige Dummy-Sätze zum Testen des Layouts
    const dummySentences = [
      "Ich zähle in Nullen und Einsen. Meistens Nullen.",
      "Vom Kaffee direkt in den Code. Bugfrei (meistens).",
      "Fullstack-Entwickler mit einer leichten Dark-Mode-Sucht."
    ]

    // Wählt zufällig einen Satz aus und speichert ihn
    tagline.value = dummySentences[Math.floor(Math.random() * dummySentences.length)] || "Ich liebe Code."

  } catch (error) {
    console.error(error)
  } finally {
    isLoading.value = false
  }
}
</script>




// // 1. Unsere reaktiven Variablen

// // 2. Die Funktion, die das Backend aufruft
// const fetchTagline = async () => {
//   // Wenn schon geladen wird, Klicks ignorieren
//   if (isLoading.value) return

//   isLoading.value = true

//   try {
//     // Hier kommt die URL deines Backends rein
//     const response = await fetch('/api/generateTagline')

//     if (!response.ok) throw new Error('Netzwerkfehler')

//     const data = await response.json()
//     // Den Text mit dem neuen KI-Satz überschreiben
//     tagline.value = data.tagline
//   } catch (error) {
//     console.error('Fehler beim Laden des Taglines:', error)
//     tagline.value = 'Hoppla, die KI schläft gerade. Versuch es nochmal!'
//   } finally {
//     // Lade-Animation beenden
//     isLoading.value = false
//   }
// }


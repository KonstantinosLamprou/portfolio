<template>
  <div class="container my-20 divide-surface divide-y">
    <div class="pb-8 space-y-4">
      <h2 class="text-4xl leading-9 font-extrabold tracking-tight sm:text-5xl sm:leading-10 md:text-6xl md:leading-14">
        Skills & Tools
      </h2>
      <p class="text-sm md:text-lg leading-4 md:leading-7 opacity-80">
        Ein Überblick über die Technologien, mit denen ich arbeite.
      </p>
    </div>
    <div class="pt-8">

      <div class="flex flex-wrap items-center gap-3">

        <Badge
          v-for="skill in skillsList"
          :key="skill.name"
          variant="default"
        >
          <template #icon>
            <component
              :is="skill.iconComponent"
              class="mr-2 h-6 w-6"
              aria-hidden="true"
            />
          </template>

          {{ formatName(skill.name) }}
        </Badge>

      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { shallowRef, onMounted } from 'vue'
import Badge from '../ui/Bagde.vue'

// Typdefinition für unsere Skill-Objekte
interface Skill {
  name: string;
  iconComponent: any;
}

const skillsList = shallowRef<Skill[]>([])

// 1. Magie: Wir laden alle SVGs aus dem Ordner als Vue-Komponenten (Dank Vite!)
// Das ?component am Ende ist wichtig, falls du vite-svg-loader nutzt.
const svgModules = import.meta.glob('@/assets/skills/*.svg', { eager: true, as: 'component' })

onMounted(() => {
  const loadedSkills: Skill[] = []

  // 2. Wir iterieren über die gefundenen Dateien
  for (const path in svgModules) {
    // Extrahieren den Dateinamen ohne .svg (z.B. 'vuejs' aus '/src/assets/skills/vuejs.svg')
    const match = path.match(/\/([^/]+)\.svg$/)
    const name = match ? match[1] : 'Unknown'

    loadedSkills.push({
      name: name,
      // Die SVG-Datei als renderbare Vue-Komponente
      iconComponent: svgModules[path]
    })
  }

  // 3. Speichern die Liste
  skillsList.value = loadedSkills
})

// Hilfsfunktion: Macht den ersten Buchstaben groß (z.B. 'javascript' -> 'Javascript')
const formatName = (name: string) => {
  if (name.toLowerCase() === 'csharp') return 'C#'
  if (name.toLowerCase() === 'cpp') return 'C++'
  return name.charAt(0).toUpperCase() + name.slice(1)
}
</script>

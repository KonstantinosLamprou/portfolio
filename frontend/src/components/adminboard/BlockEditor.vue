<template>
  <div class="space-y-4">
    
    <div v-if="type === 'heading'" class="flex gap-4">
      <div class="w-1/4">
        <label class="block text-xs font-medium text-gray-500 uppercase">Ebene</label>
        <select 
          v-model="model.level" 
          class="mt-1 w-full rounded border p-2 bg-white"
        >
          <option :value="1">H1 (Haupttitel)</option>
          <option :value="2">H2 (Sektion)</option>
          <option :value="3">H3 (Untersektion)</option>
        </select>
      </div>
      <div class="w-3/4">
        <label class="block text-xs font-medium text-gray-500 uppercase">Überschrift Text</label>
        <input 
          v-model="model.text" 
          type="text" 
          placeholder="Deine Überschrift..."
          class="mt-1 w-full rounded border p-2 bg-white" 
        />
      </div>
    </div>

    <div v-else-if="type === 'paragraph'">
      <label class="block text-xs font-medium text-gray-500 uppercase">Textblock</label>
      <textarea 
        v-model="model.text" 
        rows="4" 
        placeholder="Schreibe deinen Text hier..."
        class="mt-1 w-full rounded border p-2 bg-white"
      ></textarea>
    </div>

    <div v-else-if="type === 'image'" class="space-y-3">
      <div>
        <label class="block text-xs font-medium text-gray-500 uppercase">Bild URL (src)</label>
        <input v-model="model.src" type="text" placeholder="https://..." class="mt-1 w-full rounded border p-2 bg-white" />
      </div>
      <div class="flex gap-4">
        <div class="w-1/2">
          <label class="block text-xs font-medium text-gray-500 uppercase">Alt-Text (für SEO/Barrierefreiheit)</label>
          <input v-model="model.alt" type="text" placeholder="Beschreibung des Bildes..." class="mt-1 w-full rounded border p-2 bg-white" />
        </div>
        <div class="w-1/2">
          <label class="block text-xs font-medium text-gray-500 uppercase">Bildunterschrift (Optional)</label>
          <input v-model="model.caption" type="text" placeholder="z.B. Foto von..." class="mt-1 w-full rounded border p-2 bg-white" />
        </div>
      </div>
    </div>

    <div v-else-if="type === 'code'" class="space-y-3">
      <div>
        <label class="block text-xs font-medium text-gray-500 uppercase">Programmiersprache</label>
        <input v-model="model.language" type="text" placeholder="z.B. typescript, csharp, html" class="mt-1 w-full rounded border p-2 bg-white" />
      </div>
      <div>
        <label class="block text-xs font-medium text-gray-500 uppercase">Code</label>
        <textarea 
          v-model="model.code" 
          rows="6" 
          class="mt-1 w-full rounded border p-2 bg-slate-900 text-slate-100 font-mono text-sm"
        ></textarea>
      </div>
    </div>

    <div v-else class="text-red-500">
      Unbekannter Block-Typ: {{ type }}
    </div>

  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'

const props = defineProps<{
  type: string
}>()

// Das model (die `data` Eigenschaft deines Blocks)
const model = defineModel<Record<string, any>>({ required: true })

// Wenn der Block neu erstellt wird, ist das `data` Objekt noch leer {}.
// Wir befüllen es hier direkt mit Standardwerten passend zu deinen Types!
onMounted(() => {
  if (Object.keys(model.value).length === 0) {
    switch (props.type) {
      case 'heading':
        // Standard: H2 mit leerem Text? 
        model.value = { level: 2, text: '' }
        break
      case 'paragraph':
        model.value = { text: '' }
        break
      case 'image':
        model.value = { src: '', alt: '', caption: '' }
        break
      case 'code':
        model.value = { language: 'typescript', code: '' }
        break
    }
  }
})
</script>
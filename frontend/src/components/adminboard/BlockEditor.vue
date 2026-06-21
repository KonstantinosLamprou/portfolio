<template>
  <div class="space-y-4">
    
    <div v-if="type === 'heading'" class="flex gap-4">
      <div class="w-1/4">
        <label class="block text-xs font-medium text-gray-500 uppercase">Ebene</label>
        <select 
          v-model="model.level" 
          class="mt-1 w-full rounded border p-2 bg-white "
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
      
      <div class="p-4 border-2 border-dashed border-gray-300 rounded-lg text-center">
        <label class="block text-sm font-medium text-gray-700 mb-2">Bild hochladen</label>
        
        <input 
          v-if="!model.src"
          type="file" 
          accept="image/*" 
          @change="handleImageUpload" 
          class="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100"
        />

        <div v-else class="flex flex-col items-center gap-2">
          <img :src="model.src" class="h-32 object-contain rounded" />
          <div class="flex gap-2">
            <input v-model="model.src" type="text" class="text-xs rounded border p-1 w-full bg-gray-50" readonly />
            <button 
              type="button" 
              @click="removeImage" 
              class="text-red-500 text-sm">
              Bild löschen
            </button>
          </div>
          </div>
      </div>

      <div class="flex gap-4">
        <div class="w-1/2">
          <label class="block text-xs font-medium text-gray-500 uppercase">Alt-Text</label>
          <input v-model="model.alt" type="text" class="mt-1 w-full rounded border p-2 bg-white" />
        </div>
        <div class="w-1/2">
          <label class="block text-xs font-medium text-gray-500 uppercase">Bildunterschrift</label>
          <input v-model="model.caption" type="text" class="mt-1 w-full rounded border p-2 bg-white" />
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
import apiClient from '@/services/api'
import { toast } from 'vue-sonner'

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

const handleImageUpload = async (event: Event) => {
  const target = event.target as HTMLInputElement
  const file = target.files?.[0]
  
  if (!file) return

  // Wir verpacken die Datei in ein FormData-Objekt
  const formData = new FormData()
  formData.append('file', file)

  // Lade-Toast anzeigen
  const toastId = toast.loading('Bild wird hochgeladen...')

  try {
    // Schickt das Bild an deinen C# FilesController
    const response = await apiClient.post('/files/upload', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }, 
      withCredentials: true 
    })

    // Der Controller schickt { url: "https://..." } zurück
    model.value.src = response.data.url
    
    toast.success('Bild erfolgreich hochgeladen!', { id: toastId })
    
  } catch (error) {
    console.error(error)
    toast.error('Fehler beim Bild-Upload', { id: toastId })
    // Input-Feld wieder zurücksetzen
    target.value = '' 
  }
}



const removeImage = async () => {
  if (!model.value.src) return

  try {
    // Sende die URL, die du beim Upload vom Server bekommen hast, zurück an den neuen Endpunkt
    await apiClient.delete('/files/delete', {
      params: { fileUrl: model.value.src }, // Hängt die URL als ?fileUrl=... an
      withCredentials: true
    })

    // Bild aus dem Frontend-State entfernen
    model.value.src = ''
    toast.success('Bild entfernt')
    
  } catch (error) {
    console.error('Fehler beim Löschen des Bildes:', error)
    toast.error('Bild konnte auf dem Server nicht gelöscht werden.')
  }
}

</script>
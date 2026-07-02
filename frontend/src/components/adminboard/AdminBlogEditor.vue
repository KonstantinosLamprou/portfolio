<template>
  <div class="container mx-auto py-12">
    <h1 class="text-3xl font-bold mb-8">
      Neuen {{ post.contentType === 'blog' ? 'Blogpost' : 'Projekt' }} erstellen
    </h1>

    <form @submit.prevent="handleSubmit">
      <section class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-10">
        <div class="md:col-span-2 mb-4">
          <label class="block text-sm font-medium mb-2">Was möchtest du erstellen?</label>
          <div class="flex gap-4">
            <label class="flex items-center gap-2 cursor-pointer">
              <input type="radio" v-model="post.contentType" value="blog" class="w-4 h-4 text-blue-600" />
              <span>Blogpost</span>
            </label>
            <label class="flex items-center gap-2 cursor-pointer">
              <input type="radio" v-model="post.contentType" value="project" class="w-4 h-4 text-blue-600" />
              <span>Projekt</span>
            </label>
          </div>
        </div>        
        <div>
          <label class="block text-sm font-medium">Titel</label>
          <input 
            v-model="post.title" 
            type="text" 
            required
            class="mt-1 w-full rounded-md border p-2 bg-surface" 
          />
        </div>
        <div>
          <label class="block text-sm font-medium">Slug (z.B. mein-post)</label>
          <input 
            v-model="post.slug" 
            type="text" 
            required
            class="mt-1 w-full rounded-md border p-2 bg-surface" 
          />
        </div>
        <div class="md:col-span-2">
          <label class="block text-sm font-medium mb-2">Vorschaubild (Cover-Bild)</label>
          <!-- Bilder -->
          <div class="p-4 border-2 border-dashed border-gray-300 rounded-lg text-center bg-surface/50">
            <input 
              v-if="!post.imgSrc"
              type="file" 
              accept="image/*" 
              @change="handleCoverImageUpload" 
              class="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100 cursor-pointer"
            />

            <div v-else class="flex flex-col items-center gap-3">
              <img 
                :src="post.imgSrc" 
                class="object-contain object-center p-4 rounded-lg shadow-md w-full max-w-md" 
                alt="Cover Vorschau"

                 />
              
              <div class="flex gap-2 w-full max-w-md">
                <input v-model="post.imgSrc" type="text" class="text-xs rounded border p-2 w-full bg-gray-50 text-gray-500" readonly />
                <button 
                  type="button" 
                  @click="removeImage" 
                  class="px-4 py-1 bg-red-100 text-red-600 rounded hover:bg-red-200 transition-colors text-sm font-medium">
                  Bild löschen
                </button>
              </div>
            </div>
          </div>
        </div>
        <div class="md:col-span-2">
          <label class="block text-sm font-medium">Kurzbeschreibung (für Karte)</label>
          <textarea 
            v-model="post.description" 
            rows="2" 
            class="mt-1 w-full rounded-md border p-2 bg-surface"
          ></textarea>
        </div>
          <TagsInput v-model="post.tags" class="md:col-span-2 ">
            <TagsInputItem v-for="tag in post.tags" :key="tag" :value="tag">
              <TagsInputItemText />
              <TagsInputItemDelete />
            </TagsInputItem>
            <TagsInputInput placeholder="Cplusplus..." />
          </TagsInput>
      </section>

      <section class="mb-10">
        <div class="flex flex-col mb-4 sm:flex-row sm:justify-between sm:items-center">
          <h2 class="text-2xl font-semibold">Inhaltsblöcke</h2>
          <div class="flex gap-2 flex-wrap ">
            <button type="button" @click="addBlock('heading')" class="px-3 py-1 text-foreground border border-input rounded hover:opacity-80">+ Überschrift</button>
            <button type="button" @click="addBlock('paragraph')" class="px-3 py-1 text-foreground border border-input rounded hover:opacity-80">+ Text</button>
            <button type="button" @click="addBlock('image')" class="px-3 py-1 text-foreground border border-input rounded hover:opacity-80">+ Bild</button>
            <button type="button" @click="addBlock('code')" class="px-3 py-1 text-foreground border border-input  rounded hover:opacity-80">+ Code</button>
          </div>
        </div>

        <div v-if="post.content.length === 0" class="text-gray-500 italic p-4 border border-dashed rounded-lg text-center">
          Noch keine Blöcke vorhanden. Füge oben einen hinzu!
        </div>

        <div class="space-y-4">
          <div 
            v-for="(block, index) in post.content" 
            :key="block.id" 
            class="border rounded-lg p-4 bg-surface relative"
          >
            <div class="absolute top-2 right-2 flex gap-2">
              <!-- Maybe hier noch Buttons für "Nach oben"/"Nach unten" hinzufügen? -->
               <!-- maybe beim löschen aufpassen bei Bildern, dass die auch vom Server gelöscht werden? Oder einfach in der Warnung sagen "Vergiss nicht, ungenutzte Bilder im Medienmanager zu löschen" oder so -->
              <button 
                type="button" 
                @click="removeBlock(index)" 
                class="text-red-500 text-sm">
                  Entfernen
                </button>
            </div>
            
            <div class="mb-4 font-semibold text-sm text-foreground border-b pb-2">
              Block: {{ block.type.toUpperCase() }}
            </div>
            
            <BlockEditor class="text-black" :type="block.type" v-model="block.data" />
          </div>
        </div>
      </section>

      <button 
        type="submit" 
        :disabled="isPending"
        class="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-50 flex items-center gap-2"
      >
        <span v-if="isPending" class="animate-spin inline-block w-4 h-4 border-2 border-white border-t-transparent rounded-full"></span>
        {{ isPending ? 'Speichert...' : 'Beitrag speichern' }}
      </button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import BlockEditor from './BlockEditor.vue'
import { type CreateBlogRequest } from '@/types/blogTypes.ts'
import { useCreateContentblogs, useCreateContentprojects } from '@/composables/useCreateContent.ts'
import { toast } from 'vue-sonner'
import apiClient from '@/services/api.ts'
import { isAxiosError } from 'axios'
import { TagsInput, TagsInputInput, TagsInputItem, TagsInputItemDelete, TagsInputItemText } from '@/components/ui/tags-input'
import { useSession } from '@/composables/useSession.ts'



// const { data: session, isPending: isSessionLoading } = 
useSession(); 
// const isAuthenticated = computed(() => !!session.value && !isSessionLoading.value); 


const { 
  mutate: mutateBlog, 
  isPending: isPendingBlog 
} = useCreateContentblogs()

const { 
  mutate: mutateProject, 
  isPending: isPendingProject 
} = useCreateContentprojects()

// damit der Spinner auf dem Button bei beiden Calls reagiert
const isPending = computed(() => isPendingBlog.value || isPendingProject.value)

// reaktiver Zustand für das Formular
const post = ref<CreateBlogRequest>({
  contentType: 'blog', 
  title: '',
  slug: '',
  imgSrc: '',
  description: '',
  tags: [],
  content: []
})

// Methode um einen neuen Block hinzuzufügen
const addBlock = (type: string) => {
  post.value.content.push({
    id: crypto.randomUUID(), 
    type: type,
    data: {} 
  })
}

// Methode um einen Block zu entfernen
const removeBlock = (index: number) => {
  post.value.content.splice(index, 1)
}

  // Mutate ausführen
const handleSubmit = () => {
  // Bevor wir absenden, ein kleiner Check, ob überhaupt Inhalt da ist
  if (post.value.content.length === 0) {
    toast.warning('Dein Beitrag braucht mindestens einen Inhaltsblock!')
    return
  }

  // Gemeinsame Callback-Logik für Erfolg und Fehler
  const callbacks = {
    onSuccess: (data: any) => {
      toast.success('Erfolg!', {
        description: `Der Beitrag "${data.title || post.value.title}" wurde gespeichert.`
      })
      
      // Formular nach Erfolg leeren (behält den ausgewählten contentType)
      const currentType = post.value.contentType
      post.value = { 
        contentType: currentType, 
        title: '', 
        slug: '', 
        imgSrc: '', 
        description: '', 
        tags: [],
        content: [] 
      }
    },
    onError: (error: Error) => {
      toast.error('Speichern fehlgeschlagen', {
        description: error.message
      })
    }
  }

  if (post.value.contentType === 'blog') {
    mutateBlog(post.value, callbacks)
  } else if (post.value.contentType === 'project') {
    mutateProject(post.value, callbacks)
  }
}


const handleCoverImageUpload = async (event: Event) => {
  const target = event.target as HTMLInputElement
  const file = target.files?.[0]
  
  if (!file) return

  const formData = new FormData()
  formData.append('file', file)

  const toastId = toast.loading('Vorschaubild wird hochgeladen...')

  try {
    const response = await apiClient.post('/files/upload', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }, 
      withCredentials: true 
    })

    post.value.imgSrc = response.data.url
    
    toast.success('Vorschaubild erfolgreich hochgeladen!', { id: toastId })
    
} catch (error: any) { 
console.error('Detail-Fehler vom Server:', error)
    
    let errorMessage = 'Ein unbekannter Fehler ist aufgetreten.'

    if (isAxiosError(error)) {
      if (error.response?.status === 401) {
        errorMessage = 'Du bist nicht angemeldet oder deine Sitzung ist abgelaufen.'
      } else if (error.response?.data) {
        const data = error.response.data
        if (typeof data === 'string' && data.trim() !== '') {
          errorMessage = data
        } else if (data.title) {
          errorMessage = data.title
        }
      } else if (error.message) {
         errorMessage = error.message
      }
    } else if (error instanceof Error) {
      errorMessage = error.message
    }

    toast.error(errorMessage, { id: toastId })
    
    target.value = ''
  } 
}

const removeImage = async () => {
  if (!post.value.imgSrc) return

  try {
    // Sende die URL, die du beim Upload vom Server bekommen hast, zurück an den neuen Endpunkt
    await apiClient.delete('/files/delete', {
      params: { fileUrl: post.value.imgSrc }, // Hängt die URL als ?fileUrl=... an
      withCredentials: true
    })

    // Bild aus dem Frontend-State entfernen
    post.value.imgSrc = ''
    toast.success('Bild entfernt')
    
  } catch (error) {
    console.error('Fehler beim Löschen des Bildes:', error)
    toast.error('Bild konnte auf dem Server nicht gelöscht werden.')
  }
}

</script>

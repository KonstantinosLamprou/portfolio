<template>
  <Tabs v-model="currentTab">
    <TabsList class="dark:bg-gray-800/80">
      <TabsTrigger value="write">Write</TabsTrigger>
      <TabsTrigger value="preview">Preview</TabsTrigger>
    </TabsList>

    <TabsContent value="write">
      <div class="mt-2 space-y-2">
        <InputGroup>
          <InputGroupTextarea
            ref="textareaRef"
            v-model="content"
            class="min-h-10 resize-none"
            :placeholder="placeholder"
            :disabled="disabled"
            @keydown="handleKeydown"
          />
          <InputGroupAddon align="block-end">
            <InputGroupButton
              variant="ghost"
              size="icon-xs"
              :disabled="disabled"
              aria-label="Bold"
              @click.prevent="decorate('bold')"
            >
              <Bold />
            </InputGroupButton>
            <InputGroupButton
              variant="ghost"
              size="icon-xs"
              :disabled="disabled"
              aria-label="Italic"
              @click.prevent="decorate('italic')"
            >
              <Italic />
            </InputGroupButton>
            <InputGroupButton
              variant="ghost"
              size="icon-xs"
              :disabled="disabled"
              aria-label="Strikethrough"
              @click.prevent="decorate('strikethrough')"
            >
              <Strikethrough />
            </InputGroupButton>
          </InputGroupAddon>
        </InputGroup>
      </div>
    </TabsContent>

    <TabsContent value="preview">
      <div class="rounded-md border border-gray-300 dark:border-gray-600 bg-gray-50 dark:bg-gray-800/50 px-3 py-2 min-h-[100px] prose dark:prose-invert max-w-none text-sm">
        <p v-if="!content" class="text-gray-400 italic">Nothing to preview...</p>
        <div v-else>{{ content }}</div> 
      </div>
    </TabsContent>
  </Tabs>
</template>

<script setup lang="ts">
import { ref, nextTick } from 'vue'
import { Bold, Italic, Strikethrough } from 'lucide-vue-next'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../ui/tabs'
import { InputGroup, InputGroupAddon, InputGroupButton, InputGroupTextarea } from '../ui/inputGroup'

// Props (Dinge, die nicht 2-Way-Bound sind)
defineProps<{
  placeholder?: string
  disabled?: boolean
}>()

// 2-Way-Bindings mittels Vue 3.4+ defineModel
const content = defineModel<string>({ default: '' })
const currentTab = defineModel<'write' | 'preview'>('tabsValue', { default: 'write' })

const emit = defineEmits<{
  (e: 'modEnter'): void
  (e: 'escape'): void
}>()

const textareaRef = ref<any>(null)

type DecorationType = 'bold' | 'italic' | 'strikethrough'

function decorate(type: DecorationType) {
  // WICHTIG: Da InputGroupTextarea eine Vue-Komponente ist, müssen wir an das native DOM-Element ($el) kommen.
  // Je nach UI-Bibliothek kann das auch textareaRef.value.input o.ä. sein.
  const el = textareaRef.value?.$el || textareaRef.value
  
  if (!el || typeof el.selectionStart !== 'number') return

  const start = el.selectionStart
  const end = el.selectionEnd
  const selected = content.value.substring(start, end)
  
  let prefix = ''
  let suffix = ''
  
  switch (type) {
    case 'bold': prefix = '**'; suffix = '**'; break
    case 'italic': prefix = '*'; suffix = '*'; break
    case 'strikethrough': prefix = '~~'; suffix = '~~'; break
  }
  
  content.value = 
    content.value.substring(0, start) + 
    prefix + selected + suffix + 
    content.value.substring(end)

  nextTick(() => {
    el.focus()
    // Setzt den Cursor wieder an die richtige Stelle
    el.setSelectionRange(start + prefix.length, start + prefix.length + selected.length)
  })
}

function handleKeydown(e: KeyboardEvent) {
  if ((e.metaKey || e.ctrlKey) && e.key === 'Enter') {
    emit('modEnter')
  }
  if (e.key === 'Escape') {
    emit('escape')
  }
}
</script>
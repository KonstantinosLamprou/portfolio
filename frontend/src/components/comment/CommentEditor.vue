<template>
  <Tabs v-model="currentTab" class="flex flex-col w-full">
    <TabsList class="self-start bg-[color:var(--muted)] rounded-full p-1">
      <TabsTrigger
        value="write"
        class="rounded-full text-[color:var(--muted-foreground)] data-[state=active]:bg-[color:var(--accent-background)] data-[state=active]:text-[color:var(--foreground)]"
      >
        Write
      </TabsTrigger>
      <TabsTrigger
        value="preview"
        class="rounded-full text-[color:var(--muted-foreground)] data-[state=active]:bg-[color:var(--accent-background)] data-[state=active]:text-[color:var(--foreground)]"
      >
        Preview
      </TabsTrigger>
    </TabsList>

    <TabsContent value="write" class="w-full">
      <div class="w-full mt-2 space-y-2">
        <InputGroup
          class="rounded-xl border border-[color:var(--border)] bg-[color:var(--surface-color)] shadow-[0_6px_20px_rgba(0,0,0,0.25)]"
        >
          <InputGroupTextarea
            ref="textareaRef"
            v-model="content"
            class="min-h-24 resize-none px-4 py-2 lg:text-base text-sm  text-[color:var(--color-body)] placeholder:text-[color:var(--muted-foreground)] leading-relaxed"
            :placeholder="placeholder"
            :disabled="disabled"
            @keydown="handleKeydown"
            @blur="handleBlur"

          />
          <InputGroupAddon
            align="block-end"
            class="border-t rounded-b-xl border-[color:var(--border)] bg-[color:var(--tab-background)]"
          >
            <InputGroupButton
              variant="ghost"
              size="icon-xs"
              class="cursor-pointer"
              :disabled="disabled"
              aria-label="Bold"
              @click.prevent="decorate('bold')"
            >
              <Bold />
            </InputGroupButton>
            <InputGroupButton
              variant="ghost"
              size="icon-xs"
              class="cursor-pointer"
              :disabled="disabled"
              aria-label="Italic"
              @click.prevent="decorate('italic')"
            >
              <Italic />
            </InputGroupButton>
            <InputGroupButton
              variant="ghost"
              size="icon-xs"
              class="cursor-pointer"
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

    <TabsContent value="preview" class="w-full mt-2 space-y-2">
      <div class="rounded-xl border border-[color:var(--border)] bg-gray-50 dark:bg-gray-800/50 px-4 pt-3 pb-12 min-h-[136px] prose dark:prose-invert max-w-none text-sm whitespace-pre-wrap break-words">
        <p v-if="!content.trim()" class="text-gray-400 italic">Nothing to preview...</p>
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
    content.value = content.value.trim();   
    emit('escape')
  }
}

function handleBlur() {
  content.value = content.value.trim()
}


</script>
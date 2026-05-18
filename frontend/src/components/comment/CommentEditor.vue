<template>

      <Tabs default-value="write">
    <TabsList class="dark:bg-gray-800/80">
      <TabsTrigger value="write">
        Write
      </TabsTrigger>
      <TabsTrigger  value="preview">
        Preview
      </TabsTrigger>
    </TabsList>
    <TabsContent value="write">
    <div v-if="tab === 'write'" class="mt-2 space-y-2">
      <InputGroup>
        <InputGroupTextarea
          ref="textareaRef"
          v-model="text"
          class='min-h-10 resize-none'
          placeholder="Write your comment in markdown..."
          @keydown.enter="onModEnter"
          @keydown.esc="onEscape"
        />
        <InputGroupAddon align="block-end">
          <InputGroupButton
            variant='ghost'
            size='icon-xs'
            @click="decorate('bold')"
            aria-label="Bold">
          <Bold  />
          </InputGroupButton>
          <InputGroupButton
            variant='ghost'
            size='icon-xs'
            @click="decorate('italic')"
            aria-label="Italic">
          <Italic />
          </InputGroupButton>
          <InputGroupButton
            variant='ghost'
            size='icon-xs'
            @click="decorate('strikethrough')"
            aria-label="Strikethrough">
          <Strikethrough  />
          </InputGroupButton>
        </InputGroupAddon>
      </InputGroup>
    </div>
    </TabsContent>
    <TabsContent value="preview">
          <!-- Preview-Panel -->
      <div
        class="rounded-md border border-gray-300 dark:border-gray-600 bg-gray-50 dark:bg-gray-800/50 px-3 py-2 min-h-[100px] prose dark:prose-invert max-w-none text-sm"
      >
      </div>
    </TabsContent>
  </Tabs>


</template>

<script setup lang="ts">
import { ref, computed, nextTick } from 'vue'
import { Bold, Italic, Strikethrough } from 'lucide-vue-next' // npm install lucide-vue-next
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../ui/tabs'
import { InputGroup, InputGroupAddon, InputGroupButton, InputGroupTextarea } from '../ui/inputGroup'


const props = defineProps({
  modelValue: { type: String, default: '' },
})
const emit = defineEmits(['update:modelValue', 'modEnter', 'escape'])

const tab = ref('write')
const textareaRef = ref(null)

const text = computed({
  get: () => props.modelValue,
  set: (val) => emit('update:modelValue', val),
})


function decorate(type) {
  const el = textareaRef.value
  if (!el) return
  const start = el.selectionStart
  const end = el.selectionEnd
  const selected = text.value.substring(start, end)
  let prefix = '', suffix = ''
  switch (type) {
    case 'bold': prefix = '**'; suffix = '**'; break
    case 'italic': prefix = '*'; suffix = '*'; break
    case 'strikethrough': prefix = '~~'; suffix = '~~'; break
    default: return
  }
  const newText = text.value.substring(0, start) + prefix + selected + suffix + text.value.substring(end)
  text.value = newText

  nextTick(() => {
    el.focus()
    el.setSelectionRange(start + prefix.length, start + prefix.length + selected.length)
  })
}

function onModEnter(e) {
  if ((e.metaKey || e.ctrlKey) && e.key === 'Enter') {
    emit('modEnter')
  }
}
function onEscape() {
  emit('escape')
}
</script>

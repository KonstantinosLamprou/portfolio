<script setup lang="ts">
import type { HTMLAttributes } from "vue"
import { useAttrs } from "vue"
import { useVModel } from "@vueuse/core"
import { cn } from "@/utils/cn"
import { Textarea } from "@/components/ui/textarea"

defineOptions({ inheritAttrs: false })

const props = defineProps<{
  class?: HTMLAttributes["class"]
  defaultValue?: string | number
  modelValue?: string | number
}>()

const emits = defineEmits<{
  (e: "update:modelValue", payload: string | number): void
}>()

const attrs = useAttrs()

const modelValue = useVModel(props, "modelValue", emits, {
  passive: true,
  defaultValue: props.defaultValue,
})
</script>

<template>
  <Textarea
    v-model="modelValue"
    data-slot="input-group-control"
    data-control="textarea"
    v-bind="attrs"
    :class="cn(
      'flex-1 resize-none rounded-none border-0 bg-transparent py-3 shadow-none focus-visible:ring-0 dark:bg-transparent',
      props.class,
    )"
  />
</template>

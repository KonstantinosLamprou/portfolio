<template>
  <span
    :class="[
      'inline-flex items-center justify-center rounded-md border px-3 py-1.5 text-md font-medium w-fit whitespace-nowrap shrink-0 transition-colors',
      variantClasses
    ]"
  >
    <slot name="icon" />
    <slot />
  </span>
</template>

<script setup lang="ts">
import { computed } from 'vue'

const props = withDefaults(defineProps<{
  variant?: 'default' | 'secondary' | 'outline'
}>(), {
  variant: 'default'
})

const variantClasses = computed(() => {
  switch (props.variant) {
    case 'secondary':
      return 'border-transparent bg-surface text-textBody hover:bg-surface/80'
    case 'outline':
      return 'border-gray-200 dark:border-gray-700 text-textBody hover:bg-gray-100 dark:hover:bg-gray-800'
    case 'default':
    default:
      return 'border border-surface bg-bg text-heading'
  }
})
</script>

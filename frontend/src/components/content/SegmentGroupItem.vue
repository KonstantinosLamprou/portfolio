<script setup lang="ts">
import { SegmentGroup } from '@ark-ui/vue/segment-group'
import { cn } from '@/utils/cn'

const props = defineProps<{
  value: string
  class?: any
}>()
</script>

<template>
  <SegmentGroup.Item
    data-slot="segment-group-item"
    :value="props.value"
    :class="cn(
      'cursor-pointer text-left leading-snug font-medium text-muted-foreground transition-colors hover:text-accent-foreground data-checked:text-foreground data-disabled:cursor-not-allowed data-disabled:text-muted-foreground data-disabled:opacity-50 data-horizontal:px-1 data-horizontal:pb-3 data-vertical:px-3 data-vertical:py-1.5',
      props.class
    )"
    v-bind="$attrs"
  >
    <SegmentGroup.ItemText class="block w-full">
      <slot />
    </SegmentGroup.ItemText>
    <SegmentGroup.ItemControl />
    <SegmentGroup.ItemHiddenInput />
  </SegmentGroup.Item>
</template>

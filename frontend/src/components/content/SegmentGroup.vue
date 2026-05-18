<script setup lang="ts">
import { SegmentGroup } from '@ark-ui/vue/segment-group'
import { cn } from '@/utils/cn'

// Props definieren (mit Default-Wert für orientation)
const props = withDefaults(defineProps<{
  orientation?: 'horizontal' | 'vertical'
  class?: any
}>(), {
  orientation: 'horizontal'
})
</script>

<template>
  <SegmentGroup.Root
    data-slot="segment-group"
    :data-orientation="props.orientation"
    :orientation="props.orientation"
    :class="cn(
      'flex items-start border-muted-foreground data-[orientation=horizontal]:gap-4 data-[orientation=horizontal]:border-b data-[orientation=vertical]:flex-col data-[orientation=vertical]:gap-1 data-[orientation=vertical]:border-l',
      props.class
    )"
    v-bind="$attrs"
  >
    <SegmentGroup.Indicator
      data-slot="segment-group-indicator"
      :class="cn(
        'border-accent-background data-[orientation=horizontal]:bottom-0 data-[orientation=horizontal]:w-(--width) data-[orientation=horizontal]:translate-y-px data-[orientation=horizontal]:border-b-2 data-[orientation=vertical]:h-(--height) data-[orientation=vertical]:-translate-x-px data-[orientation=vertical]:border-l-2'
      )"
    />
    <slot />
  </SegmentGroup.Root>
</template>

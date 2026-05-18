<script setup lang="ts">
import type { TabsTriggerProps } from "reka-ui"
import type { HTMLAttributes } from "vue"
import { reactiveOmit } from "@vueuse/core"
import { TabsTrigger, useForwardProps } from "reka-ui"
import { cn } from "@/utils/cn"

const props = defineProps<TabsTriggerProps & { class?: HTMLAttributes["class"] }>()

const delegatedProps = reactiveOmit(props, "class")

const forwardedProps = useForwardProps(delegatedProps)
</script>

<template>
  <TabsTrigger
    data-slot="tabs-trigger"
    :class="cn(
      'relative inline-flex h-[calc(100%-1px)] flex-1 text-bright/50 items-center justify-center gap-1.5 rounded-4xl border border-transparent px-2 py-1 text-sm font-medium whitespace-nowrap transition-all after:absolute after:bg-foreground after:opacity-0 after:transition-opacity focus-visible:border-ring focus-visible:ring-2 focus-visible:ring-ring/50 focus-visible:outline-1 focus-visible:outline-ring aria-disabled:pointer-events-none aria-disabled:opacity-50 hover:text-bright data-[state=active]:bg-tab/30 data-[state=active]:text-bright data-[state=active]:border-input',
      props.class,
    )"
    v-bind="forwardedProps"
  >
    <slot />
  </TabsTrigger>
</template>

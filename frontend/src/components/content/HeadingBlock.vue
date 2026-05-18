<template>
  <component :is="headingTag" :class="tailwindStyles" :id="block.id">
    {{ block.data.text }}
  </component>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps({
  block: {
    type: Object,
    required: true
  }
});

//HTML-Tag (Fallback auf h2, falls kein Level angegeben wurde)
const headingTag = computed(() => `h${props.block.data.level || 2}`);

const tailwindStyles = computed(() => {
  switch (props.block.data.level) {
    case 1:
      return 'text-center text-4xl md:text-5xl font-extrabold text-slate-600 mb-6 mt-8';
    case 2:
      return 'text-center text-3xl font-bold text-slate-600 mb-4 mt-8';
    case 3:
      return 'text-center text-2xl font-semibold text-slate-600 mb-3 mt-6';
    default:
      return 'text-center text-xl font-medium text-slate-600 mb-2 mt-4';
  }
});
</script>

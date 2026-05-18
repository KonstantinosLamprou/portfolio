<template>
  <figure class="my-8">
    <!-- Das eigentliche Bild -->
    <img
      :src="block.data.src"
      :alt="block.data.alt"
      class="w-full h-auto rounded-lg shadow-md object-cover"
    />

    <!-- Die Bildunterschrift (wird nur gerendert, wenn eine im JSON steht) -->
    <figcaption
      v-if="block.data.caption"
      class="mt-3 text-center text-sm text-slate-500 italic"
    >
      {{ block.data.caption }}
    </figcaption>
  </figure>
</template>

<script setup lang="ts">
defineProps({
  block: {
    type: Object,
    required: true
  }
});
</script>

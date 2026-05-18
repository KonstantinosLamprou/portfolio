<template>
  <p class="text-base md:text-lg leading-relaxed text-slate-600 mb-5">
    <!-- Wenn dein Editor reinen Text liefert: -->
    {{ block.data.text }}

  </p>
</template>

<script setup lang="ts">
defineProps({
  block: {
    type: Object,
    required: true
  }
});

</script>

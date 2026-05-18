<template>
  <div class="my-6 rounded-lg overflow-hidden bg-slate-900 shadow-lg">
    <!-- Kleine "Kopfzeile" für den Code-Block (Optional, sieht aber cool aus) -->
    <div class="flex items-center px-4 py-2 bg-slate-800 text-xs text-slate-400 font-mono uppercase tracking-wider">
      {{ block.data.language }}
    </div>

    <!-- Der eigentliche Code -->
    <div class="p-4 overflow-x-auto">
      <pre><code class="text-sm text-slate-50 font-mono leading-relaxed">{{ block.data.code }}</code></pre>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps({
  block: {
    type: Object,
    required: true
  }
});
</script>

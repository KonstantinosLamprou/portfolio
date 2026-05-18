<template>
  <button
    @click="goBack"
    class="sticky top-32 z-52 mt-2 w-fit flex items-center gap-2 px-4 py-2 mb-6 text-sm bg-main/80 backdrop-blur-md rounded-full border-transparent text-body hover:text-sky transition-all group cursor-pointer shadow-sm"
  >
    <Backward
      viewBox="0 0 24 24"
      class="w-6 h-6 transition-transform group-hover:-translate-x-1"
    />
    <span>Zurück</span>
  </button>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';
import Backward from '@/assets/backward.svg';

// Vue Router importieren, um navigieren zu können
const router = useRouter();
//TODo: Hier ändern wegen den Überschriften klappt das ganze nicht so gut
const goBack = () => {
  // router.back() ist das Äquivalent zu einem Klick auf den "Zurück"-Button des Browsers (-1 History)
  router.back();
};
</script>

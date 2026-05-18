

<template>
  <div class="mx-auto flex flex-col max-w-3xl px-4 sm:px-6 xl:max-w-5xl xl:px-0 min-h-screen">
    <Navbar />

    <main class="mb-auto mx-5">
      <slot />
    </main>

    <Footer />
  </div>
</template>

<script setup lang="ts">
import Navbar from '@/components/navbar/Navbar.vue'
import Footer from '@/components/footer/Footer.vue'
</script>



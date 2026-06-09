

<template>
  <div class="mx-auto flex flex-col max-w-3xl sm:px-6 xl:max-w-5xl xl:px-0 min-h-screen">
    <Navbar />

    <main class="mb-auto mx-4">
      <slot />
    </main>

    <Footer />
    
    <SignInDialog />

  </div>
</template>

<script setup lang="ts">
import Navbar from '@/components/navbar/Navbar.vue'
import Footer from '@/components/footer/Footer.vue'
import SignInDialog from '@/components/adminboard/SignInDialog.vue'
</script>



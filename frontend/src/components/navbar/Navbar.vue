<template>
  <header class=" flex w-[95%] max-w-5xl shadow-xs backdrop-blur-md bg-black/30 py-3 px-6 md:px-10 mx-auto mt-5 sm:mt-15 rounded-2xl sticky top-5 md:top-10 z-50">



    <div class="flex items-center">
      <RouterLink to="/" class="relative transition-all transform-gpu rounded-full px-3 py-2 hover:opacity-50">
        <span class="text-lg">~</span>
      </RouterLink>

      <nav class=" hidden md:flex gap-2 rounded-3xl px-1 py-2">
        <RouterLink to="/lebenslauf" class="relative transition-all transform-gpu rounded-full px-3 py-2 hover:opacity-50">Lebenslauf</RouterLink>
        <RouterLink to="/projekte" class="relative transition-all transform-gpu rounded-full px-3 py-2 hover:opacity-50">Projekte</RouterLink>
        <RouterLink to="/blog" class="relative transition-all transform-gpu rounded-full px-3 py-2 hover:opacity-50">Blog</RouterLink>
        <RouterLink to="/guestbook" class="relative transition-all transform-gpu rounded-full px-3 py-2 hover:opacity-50">Gästebuch</RouterLink>
      </nav>


    </div>
    <div class="ml-auto flex flex-row">
      <!-- <Search /> -->
      <ThemeSwitch />
      <MobileNav />


    </div>

  </header>
</template>

<script setup lang="ts">
// import Search from "@/components/navbar/Search.vue"
import ThemeSwitch from "@/components/navbar/ThemeSwitch.vue"
import MobileNav from "./MobileNav.vue";
import { RouterLink } from 'vue-router'

</script>


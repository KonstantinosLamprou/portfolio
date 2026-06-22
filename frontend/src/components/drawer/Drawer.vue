<template>
  <DrawerRoot v-model:open="isOpen">

    <DrawerTrigger class="md:hidden flex items-center justify-center p-2 transition-transform duration-300 hover:scale-110">
      <ToggleIcon class="w-6 h-6 text-body" />
    </DrawerTrigger>

    <DrawerPortal>
      <DrawerOverlay class="fixed inset-0 z-50 bg-black/80" />

      <DrawerContent class="fixed inset-x-0 bottom-0 z-50 mt-24 flex h-auto flex-col rounded-t-[10px] border border-drawer-border bg-drawer-bg text-drawer-text outline-none">

        <div class="mx-auto mt-4 h-2 w-25 rounded-full bg-drawer-border" />

        <div class="grid gap-1.5 pl-4 pt-2 text-center sm:text-left">
          <h2 class="text-lg font-semibold leading-none tracking-tight flex justify-start">
            Navigation
          </h2>
        </div>

        <ul @click="isOpen = false" class="grid gap-3 p-4 h-fit grid-cols-2">
          <li>
            <RouterLink to="/" class="block rounded-md p-4 bg-drawer-card hover:bg-drawer-card-hover transition-colors h-full">
              <div class="font-medium">Home</div>
              <p class="text-xs text-drawer-text-muted">Startseite</p>
            </RouterLink>
          </li>
          <li>
            <RouterLink to="/lebenslauf" class="block rounded-md p-4 bg-drawer-card hover:bg-drawer-card-hover transition-colors h-full">
              <div class="font-medium">Lebenslauf</div>
              <p class="text-xs text-drawer-text-muted">Mein Werdegang</p>
            </RouterLink>
          </li>
          <li>
            <RouterLink to="/projekte" class="block rounded-md p-4 bg-drawer-card hover:bg-drawer-card-hover transition-colors h-full">
              <div class="font-medium">Projekte</div>
              <p class="text-xs text-drawer-text-muted">Meine Arbeit</p>
            </RouterLink>
          </li>
          <li>
            <RouterLink to="/blog" class="block rounded-md p-4 bg-drawer-card hover:bg-drawer-card-hover transition-colors h-full">
              <div class="font-medium">Blog</div>
              <p class="text-xs text-drawer-text-muted">News</p>
            </RouterLink>
          </li>
          <li class="col-span-2"> 
            <RouterLink to="/guestbook" class="block rounded-md p-4 bg-drawer-card hover:bg-drawer-card-hover transition-colors h-full text-center">
              <div class="font-medium">Gästebuch</div>
              <p class="text-xs text-drawer-text-muted">Teile mir deine Meinung mit!</p>
            </RouterLink>
          </li>
        </ul>

        <div class="mt-auto flex flex-col gap-2 p-4 border-t border-drawer-border">
          <p class="text-lg font-semibold leading-none tracking-tight flex justify-start py-2">Online</p>
          <a
            href="https://github.com/KonstantinosLamprou"
            target="_blank"
            rel="noopener noreferrer"
            aria-label="Github Profil"
            class="flex items-center justify-center rounded-md bg-drawer-card hover:bg-drawer-card-hover px-4 py-2 text-sm font-medium transition-colors"
          >
            GitHub
          </a>
          <a
            href="https://www.youtube.com/@TinoBytee"
            target="_blank"
            rel="noopener noreferrer"
            aria-label="Youtube Profil"
            class="flex items-center justify-center rounded-md bg-drawer-card hover:bg-drawer-card-hover px-4 py-2 text-sm font-medium transition-colors"
          >
            Youtube
          </a>
          <a
            href="https://www.linkedin.com/in/konstantinos-lamprou-4600571bb/"
            target="_blank"
            rel="noopener noreferrer"
            aria-label="LinkedIn Profil"
            class="flex items-center justify-center rounded-md bg-drawer-card hover:bg-drawer-card-hover px-4 py-2 text-sm font-medium transition-colors"
          >
            LinkedIn
          </a>
        </div>

      </DrawerContent>
    </DrawerPortal>
  </DrawerRoot>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink } from 'vue-router'
import { DrawerContent, DrawerOverlay, DrawerPortal, DrawerRoot, DrawerTrigger } from 'vaul-vue'
import ToggleIcon from '../../assets/layout.svg'

const isOpen = ref(false)
</script>

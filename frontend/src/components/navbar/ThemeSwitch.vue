<template>
<div class="flex items-center justify-center">
      <button @click="toggleDarkMode" class="p-2 transition-transform duration-300 hover:scale-110">
        <Sun v-if="isDark" viewBox="0 0 24 24" class="w-7 h-7 text-body" />
        <Moon v-else viewBox="0 0 24 24" class="w-7 h-7 text-body" />
      </button>
</div>

</template>

<script setup lang="ts">
import Moon from '../../assets/moon.svg'
import Sun from '../../assets/sun.svg'
import { useTheme } from '../../composables/useTheme'

const { isDark, toggleDarkMode } = useTheme()
</script>

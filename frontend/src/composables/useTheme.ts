import { ref } from 'vue'

const isDark = ref(true)

export function useTheme() {
  const toggleDarkMode = () => {
    isDark.value = !isDark.value

    if (isDark.value) {
      document.documentElement.classList.add('dark')
      // Optional: Speicher die Wahl im LocalStorage, damit sie beim Neuladen bleibt
      localStorage.setItem('theme', 'dark')
    } else {
      document.documentElement.classList.remove('dark')
      localStorage.setItem('theme', 'light')
    }
  }

  const initTheme = () => {

    const savedTheme = localStorage.getItem('theme')

    if (savedTheme === 'light') {
      isDark.value = false
      document.documentElement.classList.remove('dark')
    } else {

      isDark.value = true
      document.documentElement.classList.add('dark')
    }
  }

  return {
    isDark,
    toggleDarkMode,
    initTheme
  }
}

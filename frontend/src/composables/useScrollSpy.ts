import { ref, onMounted, onBeforeUnmount } from 'vue'

export function useScrollspy(ids: string[], options: IntersectionObserverInit = {}) {
  // Speichert die aktuell sichtbare ID (z.B. '#preface')
  const activeId = ref<string>('')
  let observer: IntersectionObserver | null = null

  onMounted(() => {
    // 1. Den Observer konfigurieren
    observer = new IntersectionObserver((entries) => {
      entries.forEach((entry) => {
        // Wenn das Element in den definierten Sichtbereich (rootMargin) kommt
        if (entry.isIntersecting) {
          const id = `#${entry.target.id}`

          // Verifizieren, ob die ID auch in unserer Liste ist
          if (ids.includes(id)) {
            activeId.value = id
          }
        }
      })
    }, options)

    // 2. Alle HTML-Elemente auf der Seite suchen, die zu den IDs passen, und sie beobachten
    ids.forEach((id) => {
      // document.querySelector erwartet einen gültigen CSS-Selektor (inklusive dem #)
      const element = document.querySelector(id)
      if (element) {
        observer!.observe(element)
      } else {
        console.warn(`Scrollspy: Element mit der ID ${id} wurde im DOM nicht gefunden.`)
      }
    })
  })

  // 3. SEHR WICHTIG: Speicherlecks (Memory Leaks) verhindern!
  // Wenn der User auf eine andere Seite navigiert, müssen wir den Observer stoppen.
  onBeforeUnmount(() => {
    if (observer) {
      observer.disconnect()
    }
  })

  // Gibt die reaktive Variable zurück
  return activeId
}

<template>
  <component
    :is="as"
    ref="containerRef"
    class="inline-block whitespace-pre-wrap tracking-tight"
    :class="className"
  >
    <span class="inline" :style="{ color: currentTextColor }">
      {{ displayedText }}
    </span>

    <span
      v-if="showCursor"
      class="ml-1 inline-block cursor-blink"
      :class="[cursorClassName, { 'hidden': shouldHideCursor }]"
    >
      {{ cursorCharacter }}
    </span>
  </component>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted, onUnmounted } from 'vue'

// 1. Alle Props, die du von außen (z.B. aus HomeView) übergeben kannst
const props = withDefaults(defineProps<{
  className?: string
  showCursor?: boolean
  hideCursorWhileTyping?: boolean
  cursorCharacter?: string
  cursorClassName?: string
  text: string | string[]
  as?: string
  typingSpeed?: number
  initialDelay?: number
  pauseDuration?: number
  deletingSpeed?: number
  loop?: boolean
  textColors?: string[]
  variableSpeed?: { min: number; max: number }
  startOnVisible?: boolean
  reverseMode?: boolean
}>(), {
  className: '',
  showCursor: true,
  hideCursorWhileTyping: false,
  cursorCharacter: '|',
  cursorClassName: '',
  as: 'div',
  typingSpeed: 50,
  initialDelay: 0,
  pauseDuration: 2000,
  deletingSpeed: 30,
  loop: true,
  textColors: () => [],
  startOnVisible: false,
  reverseMode: false
})

// Ein Event, falls du reagieren willst, wenn ein Satz fertig getippt ist
const emit = defineEmits<{
  (e: 'sentenceComplete', sentence: string, index: number): void
}>()

// 2. Interne Zustände
const displayedText = ref('')
const currentCharIndex = ref(0)
const isDeleting = ref(false)
const currentTextIndex = ref(0)
const isVisible = ref(!props.startOnVisible)

const containerRef = ref<HTMLElement | null>(null)
let timeoutId: ReturnType<typeof setTimeout> | null = null

// 3. Computed Properties (berechnen Dinge automatisch neu)
const textArray = computed(() => Array.isArray(props.text) ? props.text : [props.text])

const currentTextColor = computed(() => {
  if (props.textColors.length === 0) return ''
  return props.textColors[currentTextIndex.value % props.textColors.length]
})

const shouldHideCursor = computed(() => {
  return props.hideCursorWhileTyping &&
    (currentCharIndex.value < textArray.value[currentTextIndex.value]!.length || isDeleting.value)
})

// 4. Die Tipp-Logik (Kernstück)
const getRandomSpeed = () => {
  if (!props.variableSpeed) return props.typingSpeed
  const { min, max } = props.variableSpeed
  return Math.random() * (max - min) + min
}

const executeTypingAnimation = () => {
  if (!isVisible.value) return

  const currentText = textArray.value[currentTextIndex.value]
  const processedText = props.reverseMode ? currentText!.split('').reverse().join('') : currentText

  if (isDeleting.value) {
    // Text löschen
    if (displayedText.value === '') {
      isDeleting.value = false
      if (currentTextIndex.value === textArray.value.length - 1 && !props.loop) return

      emit('sentenceComplete', textArray.value[currentTextIndex.value]!, currentTextIndex.value)
      currentTextIndex.value = (currentTextIndex.value + 1) % textArray.value.length
      currentCharIndex.value = 0
      timeoutId = setTimeout(() => { executeTypingAnimation() }, props.pauseDuration)
    } else {
      timeoutId = setTimeout(() => {
        displayedText.value = displayedText.value.slice(0, -1)
        executeTypingAnimation()
      }, props.deletingSpeed)
    }
  } else {
    // Text tippen
    if (currentCharIndex.value < processedText!.length) {
      timeoutId = setTimeout(() => {
        displayedText.value += processedText![currentCharIndex.value]
        currentCharIndex.value++
        executeTypingAnimation()
      }, props.variableSpeed ? getRandomSpeed() : props.typingSpeed)
    } else if (textArray.value.length > 1 || props.loop) {
      // Wenn fertig getippt, warte und fange an zu löschen (nur wenn es ein Array oder loop ist)
      // Für einfache Taglines wollen wir das Löschen oft verhindern, wenn es nur 1 Satz ist!
      if (textArray.value.length === 1 && props.loop === false) return;

      timeoutId = setTimeout(() => {
        isDeleting.value = true
        executeTypingAnimation()
      }, props.pauseDuration)
    }
  }
}

// 5. Lifecycle Hooks
let observer: IntersectionObserver | null = null

onMounted(() => {
  // Intersection Observer prüft, ob das Element im Bildschirm sichtbar ist
  if (props.startOnVisible && containerRef.value) {
    observer = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          isVisible.value = true
          observer?.disconnect()
          if (!timeoutId) executeTypingAnimation()
        }
      })
    }, { threshold: 0.1 })
    observer.observe(containerRef.value)
  }

  if (currentCharIndex.value === 0 && !isDeleting.value && displayedText.value === '') {
    timeoutId = setTimeout(executeTypingAnimation, props.initialDelay)
  } else {
    executeTypingAnimation()
  }
})

onUnmounted(() => {
  if (timeoutId) clearTimeout(timeoutId)
  if (observer) observer.disconnect()
})

// Wenn ein neuer Satz von DeepSeek kommt, starte den Tipper neu!
watch(() => props.text, () => {
  if (timeoutId) clearTimeout(timeoutId)
  displayedText.value = ''
  currentCharIndex.value = 0
  isDeleting.value = false
  currentTextIndex.value = 0
  executeTypingAnimation()
})
</script>

<style scoped>
/* Native CSS-Animation statt schwerem GSAP-Plugin */
.cursor-blink {
  animation: blink 1s step-end infinite;
}

@keyframes blink {
  0%, 100% { opacity: 1; }
  50% { opacity: 0; }
}
</style>

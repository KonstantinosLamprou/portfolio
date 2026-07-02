<template>
  <div
    class="relative cursor-pointer place-items-center overflow-hidden border"
    :class="className"
    :style="wrapperStyle"
    @mouseenter="animateIn"
    @mouseleave="animateOut"
  >
    <div ref="overlayRef" :style="overlayStyle"></div>

    <slot />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, type CSSProperties } from 'vue';

const props = withDefaults(defineProps<{
  width?: string;
  height?: string;
  background?: string;
  borderRadius?: string;
  borderColor?: string;
  glareColor?: string;
  glareOpacity?: number;
  glareAngle?: number;
  glareSize?: number;
  transitionDuration?: number;
  playOnce?: boolean;
  className?: string;
  customStyle?: CSSProperties; // 'style' ist in Vue reserviert, daher nennen wir es 'customStyle'
}>(), {
  width: '500px',
  height: '500px',
  background: '#000',
  borderRadius: '10px',
  borderColor: '#333',
  glareColor: '#ffffff',
  glareOpacity: 0.5,
  glareAngle: -45,
  glareSize: 250,
  transitionDuration: 650,
  playOnce: false,
  className: '',
  customStyle: () => ({})
});

// 2. Hex zu RGBA Konvertierung (Wird bei Änderung dynamisch neu berechnet)
const rgba = computed(() => {
  const hex = props.glareColor.replace('#', '');
  let r = 0, g = 0, b = 0;

  if (/^[\dA-Fa-f]{6}$/.test(hex)) {
    r = parseInt(hex.slice(0, 2), 16);
    g = parseInt(hex.slice(2, 4), 16);
    b = parseInt(hex.slice(4, 6), 16);
  } else if (/^[\dA-Fa-f]{3}$/.test(hex)) {
    r = parseInt(hex[0]! + hex[0]!, 16);
    g = parseInt(hex[1]! + hex[1]!, 16);
    b = parseInt(hex[2]! + hex[2]!, 16);
  } else {
    return props.glareColor;
  }
  return `rgba(${r}, ${g}, ${b}, ${props.glareOpacity})`;
});

const overlayRef = ref<HTMLElement | null>(null);

// Animations-Logik
const animateIn = () => {
  const el = overlayRef.value;
  if (!el) return;

  // Setzt die Position hart auf den Anfang (ohne Animation)
  el.style.transition = 'none';
  el.style.backgroundPosition = '-100% -100%, 0 0';

  // Ein kleiner Browser-Hack: Erzwingt ein Neuzeichnen, bevor die Animation startet
  void el.offsetWidth;

  // Startet den Verlauf von oben links nach unten rechts
  el.style.transition = `${props.transitionDuration}ms ease`;
  el.style.backgroundPosition = '100% 100%, 0 0';
};

const animateOut = () => {
  const el = overlayRef.value;
  if (!el) return;

  if (props.playOnce) {
    // Verschwindet sofort unsichtbar
    el.style.transition = 'none';
    el.style.backgroundPosition = '-100% -100%, 0 0';
  } else {
    // Huscht wieder geschmeidig zurück
    el.style.transition = `${props.transitionDuration}ms ease`;
    el.style.backgroundPosition = '-100% -100%, 0 0';
  }
};

// 5. CSS Styles dynamisch zusammenbauen
const wrapperStyle = computed<CSSProperties>(() => ({
  width: props.width,
  height: props.height,
  background: props.background,
  borderRadius: props.borderRadius,
  borderColor: props.borderColor,
  ...props.customStyle,
}));

const overlayStyle = computed<CSSProperties>(() => ({
  position: 'absolute',
  inset: '0',
  background: `linear-gradient(${props.glareAngle}deg, hsla(0,0%,0%,0) 60%, ${rgba.value} 70%, hsla(0,0%,0%,0) 100%)`,
  backgroundSize: `${props.glareSize}% ${props.glareSize}%, 100% 100%`,
  backgroundRepeat: 'no-repeat',
  backgroundPosition: '-100% -100%, 0 0',
  pointerEvents: 'none', // GANZ WICHTIG: Damit Klicks durch das Overlay auf den Button fallen!
}));
</script>

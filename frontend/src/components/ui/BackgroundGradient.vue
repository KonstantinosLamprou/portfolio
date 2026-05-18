<template>
  <div
    :data-position="position"
    :class="[
      'absolute left-1/2 z-10 w-full max-w-360 -translate-x-1/2 overflow-hidden pointer-events-none',
      position === 'bottom' ? 'bottom-0 [&>svg]:rotate-180' : 'top-0',
      $attrs.class
    ]"
  >
    <svg
      xmlns="http://www.w3.org/2000/svg"
      width="100%"
      fill="none"
      viewBox="0 0 1440 550"
      preserveAspectRatio="xMidYMid meet"
    >
      <g :filter="`url(#${orangeId})`">
        <ellipse cx="898.121" cy="7.207" fill="#FFB800" fill-opacity=".43" rx="284.881" ry="69.058" />
      </g>
      <g :filter="`url(#${redId})`">
        <ellipse cx="727.789" cy="48.819" fill="#E93F3F" fill-opacity=".43" rx="284.881" ry="131.671" />
      </g>
      <g :filter="`url(#${blueId})`">
        <ellipse cx="504.666" cy="27.364" fill="#3F64E9" fill-opacity=".43" rx="284.881" ry="89.316" />
      </g>
      <defs>
        <filter :id="orangeId" color-interpolation-filters="sRGB" filterUnits="userSpaceOnUse" width="1042.08" height="610.439" x="377.079" y="-298.012">
          <feFlood flood-opacity="0" result="BackgroundImageFix" />
          <feBlend in="SourceGraphic" in2="BackgroundImageFix" result="shape" />
          <feGaussianBlur result="blur" stdDeviation="118.081" />
        </filter>
        <filter :id="redId" color-interpolation-filters="sRGB" filterUnits="userSpaceOnUse" width="1042.08" height="735.665" x="206.747" y="-319.013">
          <feFlood flood-opacity="0" result="BackgroundImageFix" />
          <feBlend in="SourceGraphic" in2="BackgroundImageFix" result="shape" />
          <feGaussianBlur result="blur" stdDeviation="118.081" />
        </filter>
        <filter :id="blueId" color-interpolation-filters="sRGB" filterUnits="userSpaceOnUse" width="1042.08" height="650.953" x="-16.376" y="-298.113">
          <feFlood flood-opacity="0" result="BackgroundImageFix" />
          <feBlend in="SourceGraphic" in2="BackgroundImageFix" result="shape" />
          <feGaussianBlur result="blur" stdDeviation="118.081" />
        </filter>
      </defs>
    </svg>
  </div>
</template>

<script setup lang="ts">
withDefaults(defineProps<{
  position?: 'top' | 'bottom'
}>(), {
  position: 'top'
})

const uniqueId = Math.random().toString(36).substring(2, 9)
const orangeId = `orange-${uniqueId}`
const redId = `red-${uniqueId}`
const blueId = `blue-${uniqueId}`
</script>

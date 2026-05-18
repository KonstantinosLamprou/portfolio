<template>
  <div class="loader"></div>
</template>

<style scoped>
.loader {
  width: 20px;
  aspect-ratio: .75;
  --c: no-repeat linear-gradient(currentColor 0 0);
  background:
    var(--c) 0%   50%,
    var(--c) 50%  50%,
    var(--c) 100% 50%;
  background-size: 20% 60%;
  animation: l9 1s infinite;
}
@keyframes l9 {
  33% {background-position: 0% 0%  ,50% 100%,100% 0%  }
  66% {background-position: 0% 100%,50% 0%  ,100% 100%}
}
</style>

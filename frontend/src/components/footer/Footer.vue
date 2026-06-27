

<template>
        <footer>
            <div class="mt-16 flex flex-col items-center">
                <div class="mb-3 flex space-x-4">

                  <a
                    href="https://github.com/KonstantinosLamprou"
                    target="_blank"
                    rel="noopener noreferrer"
                    aria-label="Mein GitHub Profil"
                    class=" transition-all duration-300 hover:scale-110 inline-block"
                  >
                    <Github class="w-5 h-5" />
                  </a>

                  <a
                    href="https://www.linkedin.com/in/konstantinos-lamprou-4600571bb/"
                    target="_blank"
                    rel="noopener noreferrer"
                    aria-label="Mein LinkedIn Profil"
                    class="transition-all duration-300 hover:scale-110 inline-block"
                  >
                    <LinkedIn class="w-5 h-5" />
                  </a>

                  <a
                    href="https://www.youtube.com/@TinoBytee"
                    target="_blank"
                    rel="noopener noreferrer"
                    aria-label="Mein Youtube Profil"
                    class="transition-all duration-300 hover:scale-110 inline-block"
                  >
                    <Youtube class="w-5 h-5" />
                  </a>
                </div>
                <div class="mb-2 flex items-center space-x-2 md:text-sm text-center text-xs">
                    <a 
                      class="transition-all duration-300 text-link hover:text-sky hover:underline hover:scale-102" href="https://github.com/KonstantinosLamprou/portfolio"
                      target="_blank" 
                      rel="noopener noreferrer"
                      aria-label="Source Code des Portfolios auf Github"
                      >
                      Source Code hier
                    </a>
                    <div> • </div>
                    <div> ©2026 </div>
                    <div> • </div>
                    <div>gebaut mit Vue und .Net</div>
                  </div>
                  <div class=" mb-8 flex space-x-2 text-xs md:text-sm">
                    <div>entwickelt von Konstantinos Lamprou</div>
                    <div> • </div>
                    <RouterLink to="/impressum" class="transition-all duration-300 text-link hover:text-sky hover:underline hover:scale-102">Impressum</RouterLink>

                  </div>

            </div>
        </footer>
</template>

<script setup lang="ts">
import Mail from '../../assets/Mail.svg'
import Github from '../../assets/github.svg'
import Youtube from '../../assets/youtube.svg'
import LinkedIn from '../../assets/linkedin.svg'
</script>


<template>
  <div class="container my-30 flex flex-col xl:flex-row">

    <div class="mb-15 flex justify-center xl:mb-0 xl:w-1/2 xl:max-w-1/2">
      <div class="glass3d flex w-87.5 flex-col items-center rounded-4xl py-10">
        <div class="bg-picture w-56 h-56 rounded-full overflow-hidden flex items-end justify-center shadow-inner">
          <img
            src="../../assets/avatar.png"
            alt="Avatar von Konstantinos"
            class="w-52 h-auto translate-y-18 translate-x-3"
          />
        </div>

        <h3 class="pt-4 pb-2 text-2xl leading-8 font-bold tracking-tight text-white">
          Konstantinos Lamprou
        </h3>
        <div class="text-textBody">Informatikstudent</div>
        <div class="text-textBody">Praktikant, Meierhofer AG</div>
        <div class="text-textBody">Zurzeit: Testautomatisierer</div>

        //Svgs hier:
        <div class="flex space-x-4">

          <a
            href="kostaslamprounk@gmail.com"
            target="_blank"
            rel="noopener noreferrer"
            aria-label="Meine Mail"
            class="transition-all duration-300 hover:scale-110 inline-block"
          >
            <MailIcon class="w-8 h-8" />
          </a>

          <a
            href="https://github.com/KonstantinosLamprou"
            target="_blank"
            rel="noopener noreferrer"
            aria-label="Mein GitHub Profil"
            class=" transition-all duration-300 hover:scale-110 inline-block"
          >
            <GithubIcon class="w-7 h-7" />
          </a>

          <a
            href="https://www.linkedin.com/in/konstantinos-lamprou-4600571bb/"
            target="_blank"
            rel="noopener noreferrer"
            aria-label="Mein LinkedIn Profil"
            class="transition-all duration-300 hover:scale-110 inline-block"
          >
            <LinkedInIcon class="w-7 h-7" />
          </a>

          <a
            href="https://www.youtube.com/@TinoBytee"
            target="_blank"
            rel="noopener noreferrer"
            aria-label="Mein Youtube Profil"
            class="transition-all duration-300 hover:scale-110 inline-block"
          >
            <YoutubeIcon class="w-7 h-7" />
          </a>

        </div>


        <div class="flex space-x-3 pt-6">
          </div>
      </div>
    </div>

    <div class="flex items-center xl:w-1/2">
      <p class="text-1xl text-center sm:text-2xl text-body">
        Zurzeit bin ich noch Student und mit dem Programmieren, habe ich zeitgleich wie mit dem Studium gestartet.
        Seitdem arbeite ich hauptsächlich in C++, JavaScript/TypeScript und C#. Ich habe aber auch schon mit Python gearbeitet.
        Meine Interessen liegen in Systemprogrammierung, Low-level und Fullstack dev. Außerdem mache ich aber auch viel mit DevOps, Networking und Sysadmin für mein Homelab.
      </p>
    </div>

  </div>
</template>
<script setup lang="ts">
import GithubIcon from '../../assets/github.svg'
import LinkedInIcon from '../../assets/linkedin.svg'
import YoutubeIcon from '../../assets/youtube.svg'
import MailIcon from '../../assets/Mail.svg'

</script>

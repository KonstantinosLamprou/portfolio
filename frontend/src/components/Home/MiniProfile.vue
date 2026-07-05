<template>
  <div class="container my-30 flex flex-col xl:flex-row">

    <div class="mb-5 lg:mb-15 flex justify-center xl:mb-0 xl:w-1/2 xl:max-w-1/2">
      <div class="glass3d flex w-70 lg:w-87.5 flex-col items-center rounded-4xl py-10">
        <div class="bg-picture w-46 h-46 lg:w-56 lg:h-56 rounded-full overflow-hidden flex items-end justify-center shadow-inner">
          <img
            src="../../assets/avatar.png"
            alt="Avatar von Konstantinos"
            class="w-52 h-auto translate-y-18 translate-x-3"
          />
        </div>

        <h3 class="pt-4 pb-2 lg:text-2xl text-xl leading-8 font-bold tracking-tight">
          Konstantinos Lamprou
        </h3>
        <div class="">Informatikstudent</div>
        <div class="">Werkstudent, Meierhofer AG</div>
        <div class="">Zurzeit: Test Engineer</div>

        //Svgs hier:
        <div class="flex space-x-4">

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
        Ich würde sagen, dass ich leidenschaftlich gerne programmiere und ich mir auch viele persönliche Ziele gesetzt habe um besser zu werden. 
        Zurzeit arbeite und lerne ich hauptsächlich in C++, JavaScript/TypeScript und C#. Ich habe aber auch schon mit Python gearbeitet und werde auch dort noch das eine oder das andere Projekt machen.
        Meine größten Interessen liegen in der Fullstack-Entwicklung, Systemprogrammierung, Low-Level-Programmierung und DevOps. Mit dieser Website möchte ich meine Projekte und mein Wissen präsentieren und natürlich auch durch den Aufbau und das Maintainen der Website lernen! 
        Viel Spaß beim Durchstöbern meiner Website und hinterlasse gerne Feedback in meinem Gästebuch! Ciao for now, Kostas. 
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

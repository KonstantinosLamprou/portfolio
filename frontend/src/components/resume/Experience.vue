<template>
  <div class="container my-20 divide-surface divide-y">

    <div class="space-y-2 pt-6 pb-8 md:space-y-5">
      <h2 class="text-4xl leading-9 font-extrabold tracking-tight sm:text-5xl sm:leading-10 md:text-6xl md:leading-14">
        Arbeitserfahrung
      </h2>
      <p class="text-sm md:text-lg leading-4 md:leading-7opacity-80">
        Meine derzeitige Laufbahn.
      </p>
    </div>

    <ul class="divide-surface divide-y">

      <li
        v-for="data in experience"
        :key="data.title"
        class="py-12"
      >
        <article>
          <div class="space-y-2 xl:grid xl:grid-cols-4 xl:items-baseline xl:space-y-0">

            <dl>
              <dt class="sr-only">Zeitraum</dt>
              <dd class="text-base leading-6 font-medium text-gray-500">
                <time>{{ data.date }}</time>
              </dd>
            </dl>

            <div class="space-y-5 xl:col-span-3">
              <div class="space-y-6">
                <div>
                  <h3 class="flex flex-wrap items-center gap-3 text-2xl leading-8 font-medium tracking-tight">

                    <span class="flex h-8 w-8 items-center justify-center rounded-full bg-gray">
                      <WorkIcon class="h-4 w-4" aria-hidden="true" />
                    </span>

                    <span class="text-text md:text-2xl">{{ data.title }}</span>

                    <span v-if="data.latest" class="text-xs font-medium px-2.5 py-1 border rounded-md bg-surface text-matcha tracking-wider uppercase">
                      Aktuell
                    </span>
                  </h3>
                </div>

                <div class="prose max-w-none text-body opacity-80">
                  {{ data.description }}
                </div>
              </div>
            </div>

          </div>
        </article>
      </li>

    </ul>
  </div>
</template>

<script setup lang="ts">
import { experience } from '@/constants/experience';
import WorkIcon from '../../assets/work.svg'
</script>

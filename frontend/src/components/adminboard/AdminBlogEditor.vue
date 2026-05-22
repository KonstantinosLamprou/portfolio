<template>
  <div class="container mx-auto py-12">
    <h1 class="text-3xl font-bold mb-8">Neuen Blogpost erstellen</h1>

    <!-- Metadaten -->
    <!--
     TODO: 
     Dann auch noch wie die Daten aussehen sollen
     Es fehlt die Logik zum Speichern der Daten, sowie die Anbindung an die API.

    
    
    
    
    -->
    <section class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-10">
      <!-- Sollte die Section hier nicht lieber eine Form sein?  -->
      <div>
        <label class="block text-sm font-medium">Titel</label>
        <input  type="text" class="mt-1 w-full rounded-md border p-2" />
      </div>
      <div>
        <label class="block text-sm font-medium">Slug (z.B. mein-post)</label>
        <input  type="text" class="mt-1 w-full rounded-md border p-2" />
      </div>
      <div>
        <label class="block text-sm font-medium">Autor</label>
        <input  type="text" class="mt-1 w-full rounded-md border p-2" />
      </div>
      <div>
        <label class="block text-sm font-medium">Datum</label>
        <input  type="date" class="mt-1 w-full rounded-md border p-2" />
      </div>
      <div class="md:col-span-2">
        <label class="block text-sm font-medium">Vorschaubild URL</label>
        <input type="text" class="mt-1 w-full rounded-md border p-2" />
      </div>
      <div class="md:col-span-2">
        <label class="block text-sm font-medium">Kurzbeschreibung (für Karte)</label>
        <textarea rows="2" class="mt-1 w-full rounded-md border p-2"></textarea>
      </div>
    </section>

    <section>
            <div class="flex justify-between items-center mb-4">
        <h2 class="text-2xl font-semibold">Inhaltsblöcke</h2>
        <div class="flex gap-2">
          <!-- Die hier sollten Blocktypen hinzufügen, ich könnte eine Blocktyp Komponente machen, die dann den Typ und die Daten als Props bekommt -->
          <button  class="px-3 py-1 bg-accent text-main rounded">+ Titel</button>
          <button class="px-3 py-1 bg-accent text-main rounded">+ Überschrift</button>
          <button class="px-3 py-1 bg-accent text-main rounded">+ Überschrift 2</button>
          <button class="px-3 py-1 bg-accent text-main rounded">+ Überschrift 3</button>
          <button  class="px-3 py-1 bg-accent text-main rounded">+ Text</button>
          <button  class="px-3 py-1 bg-accent text-main rounded">+ Bild</button>
          <button  class="px-3 py-1 bg-accent text-main rounded">+ Code</button>
        </div>
      </div>

      <!-- <draggable v-model="post.blocks" item-key="id" handle=".handle" class="space-y-4">
        <template #item="{ element: block, index }">
          <div class="border rounded-lg p-4 bg-surface relative">
            <div class="absolute top-2 right-2 flex gap-2">
              <button @click="removeBlock(index)" class="text-red-500 text-sm">✕</button>
              <span class="handle cursor-move text-gray-400">⋮⋮</span>
            </div> -->
      <!-- <component
              :is="blockEditors[block.type]"
              :block="block"
              @update:block="updateBlock(index, $event)"
            />
          </div>
        </template>
      </draggable> -->


    </section>

    <button class="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700">
      Speichern
    </button>
  </div>
</template>

<script setup lang="ts">
//Wichtig: Hier sollten die Blocktypen für einen Blog- oder Projektpost definiert werden´


</script>

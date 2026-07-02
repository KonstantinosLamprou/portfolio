<template>
  <article class="max-w-3xl mx-auto px-4 py-10">
    <component
      v-for="block in blocks"
      :key="block.id"
      :is="getComponentMap(block.type)"
      :block="block"
    />
  </article>
</template>

<script setup lang="ts">
import HeadingBlock from '../content/HeadingBlock.vue';
import TextBlock from '../content/TextBlock.vue';
import { type Component } from 'vue';
import { type ContentBlock } from '@/types/blogTypes.ts';
import ImageBlock from '../content/ImageBlock.vue';
import CodeBlock from '../content/CodeBlock.vue';

const componentMap: Record<string, Component> = {
  heading: HeadingBlock,
  paragraph: TextBlock,
  image: ImageBlock,
  code: CodeBlock
};

defineProps<{
  blocks: ContentBlock[] 
}>();


const getComponentMap = (type: string) => {
  if (!componentMap[type]) {
    console.warn(`Unbekannter Block-Typ: ${type}`);
    return null;
  }
  return componentMap[type];
};


//Hier warscheinlich das GET für die Daten, aber da wir ja jetzt die Mock-Daten direkt über Props bekommen, brauchen wir das erstmal nicht.
</script>

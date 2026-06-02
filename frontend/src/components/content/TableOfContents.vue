<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useScrollspy } from '@/composables/useScrollSpy'
import SegmentGroup from './SegmentGroup.vue'
import SegmentGroupItem from './SegmentGroupItem.vue'

// Dein Typ sieht jetzt so aus
type TOC = { url: string; title: string; depth: number }

const props = defineProps<{ toc: TOC[] }>()
const router = useRouter()

// Wir hängen hier das '#' dran, damit der Scrollspy weiß: Suche nach id="block-1"
const tocUrls = props.toc.map((item) => `#${item.url}`)
const activeId = useScrollspy(tocUrls, { rootMargin: '0% 0% -40% 0%' })

const handleValueChange = (payload: any) => {
  // Extrahiere den Wert (z.B. '#block-1') sicher, egal ob es ein String oder ein Objekt ist
  const targetHash = typeof payload === 'string' ? payload : payload.value;

  // 1. Die Route anpassen
  router.push({ hash: targetHash });

  // 2. Das Element manuell suchen und via Browser-API sanft hinscrollen
  const element = document.querySelector(targetHash);
  if (element) {
    element.scrollIntoView({ behavior: 'smooth', block: 'start' });
  }
}
</script>

<template>
  <div class="hidden pl-4 lg:block">
    <div class="mt-10 mb-4 font-semibold">Auf dieser Seite</div>

    <SegmentGroup
      orientation="vertical"
      v-model="activeId"
      @update:model-value="handleValueChange"
      class="text-sm"
    >
      <SegmentGroupItem
        v-for="item in props.toc"
        :key="item.url"
        :value="`#${item.url}`"
        :style="{ marginLeft: `${(item.depth ) * 12}px` }"
      >
        {{ item.title }}
      </SegmentGroupItem>
    </SegmentGroup>
  </div>
</template>

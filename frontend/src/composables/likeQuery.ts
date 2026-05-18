import { ref, reactive } from 'vue'
import { mockBlogData } from '@/data/mockBlogPostData'

// Ein simpler In-Memory-Speicher für unsere Mock-Session (simuliert die Datenbank/LocalStorage)
// Key ist der Slug, Value ist ein Objekt mit den Like-Daten
const mockLikeStore = reactive<Record<string, { totalLikes: number; currentUserLikes: number }>>({})

export function useCountLike({ slug }: { slug: string }) {
  const isLoading = ref(false)
  const isError = ref(false)
  const isSuccess = ref(true)

  // Wenn der Post noch nicht im Mock-Store ist, initialisieren wir ihn aus den mockBlogData
  if (!mockLikeStore[slug]) {
    const post = mockBlogData.find(p => p.slug === slug)
    mockLikeStore[slug] = {
      totalLikes: post?.likes || 0,
      currentUserLikes: 0 // Der User startet immer bei 0 Likes in der Simulation
    }
  }

  // data gibt reaktiv den aktuellen Stand für diesen Slug zurück
  const data = ref(mockLikeStore[slug])

  return {
    data,
    isLoading,
    isError,
    isSuccess
  }
}

export function useIncrementLike() {
  // Simuliert die Mutation (wie in Vue Query)
  const mutate = ({ slug, value }: { slug: string; value: number }) => {
    if (mockLikeStore[slug]) {
      // Füge die in der Wartezeit gesammelten Likes zum Gesamtstand und zum User-Stand hinzu
      mockLikeStore[slug].totalLikes += value;
      mockLikeStore[slug].currentUserLikes += value;

      console.log(`[Mock API] Mutiere post '${slug}': +${value} Likes. (Total: ${mockLikeStore[slug].totalLikes})`)
    }
  }

  return {
    mutate
  }
}

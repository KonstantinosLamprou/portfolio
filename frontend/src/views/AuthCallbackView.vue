<script lang="ts" setup>

import { onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/useAuthStore';

const router = useRouter();
const auth = useAuthStore();

onMounted(async () => {
  await auth.checkSession();

  const returnUrl = sessionStorage.getItem('auth:returnUrl') || '/';
  sessionStorage.removeItem('auth:returnUrl');

  router.replace(returnUrl);
});
</script>
// src/stores/authStore.ts
import { defineStore } from 'pinia';
import { ref } from 'vue';
import { exchangeOAuthCode, fetchProfile } from '@/services/authService';
import type { UserProfile } from '@/types/auth';

// defineStore erwartet eine eindeutige ID (hier 'auth') und eine Setup-Funktion
export const useAuthStore = defineStore('auth', () => {
  
  // STATE 
  const user = ref<UserProfile | null>(null);
  const isAuthenticating = ref(false);

  // ACTIONS 
  const loginWithProvider = (provider: string) => {
    const returnUrl =
        window.location.pathname +
        window.location.search +
        window.location.hash;

    sessionStorage.setItem('auth:returnUrl', returnUrl);
    window.location.href = `/api/auth/${provider}/login`; 
  };

  const handleOAuthCallback = async (provider: string, code: string) => {
    isAuthenticating.value = true;
    try {
      const data = await exchangeOAuthCode(provider, code);
      user.value = data.user;
    } catch (error) {
      console.error("Login fehlgeschlagen:", error);
    } finally {
      isAuthenticating.value = false;
    }
  };

  const checkSession = async () => {
    user.value = await fetchProfile();
  };

  // RETURN 
  return { 
    user, 
    isAuthenticating, 
    loginWithProvider, 
    handleOAuthCallback, 
    checkSession 
  };
});
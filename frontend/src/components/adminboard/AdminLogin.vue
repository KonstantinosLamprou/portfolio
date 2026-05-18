<template>
  <form @submit.prevent="handleSubmit" class="max-w-sm mx-auto mt-25 p-6 bg-heading rounded-xl shadow-md space-y-4">
    <!-- E-Mail -->
    <div>
      <label for="email" class="block text-sm font-medium text-gray mb-1">E-Mail</label>
      <input
        id="email"
        v-model="email"
        type="email"
        required
        placeholder="deine@email.de"
        :class="[
          'w-full px-3 py-2 border rounded-md bg-main text-color shadow-sm focus:outline-none focus:ring-2',
          emailError
            ? 'border-red-500 focus:ring-red-500'
            : 'border-gray-300 focus:ring-color focus:border-color'
        ]"
      />
      <p v-if="emailError" class="text-red-500 text-xs mt-1">{{ emailError }}</p>
    </div>

    <!-- Passwort -->
    <div>
      <label for="password" class="block text-sm font-medium text-gray mb-1">Passwort</label>
      <input
        id="password"
        v-model="password"
        type="password"
        required
        placeholder="Passwort"
        :class="[
          'w-full px-3 py-2 border rounded-md bg-main text-color shadow-sm focus:outline-none focus:ring-2',
          passwordError
            ? 'border-red-500 focus:ring-red-500'
            : 'border-gray-300 focus:ring-color focus:border-color'
        ]"
      />
      <p v-if="passwordError" class="text-red-500 text-xs mt-1">{{ passwordError }}</p>
    </div>

    <!-- Button -->
    <button
      type="submit"
      :disabled="isSubmitting"
      class="w-full py-2 px-4 bg-main text-color rounded-md focus:outline-none focus:ring-2 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed transition"
    >
      {{ isSubmitting ? 'Anmeldung...' : 'Anmelden' }}
    </button>

    <!-- Allgemeiner Fehler -->
    <p v-if="submitError" class="text-red-500 text-sm text-center">{{ submitError }}</p>
  </form>
</template>

<script setup lang="ts">
import { ref } from 'vue'

import type { LoginCredentials } from '@/types/auth'

const emit = defineEmits<{
  (e: 'login', credentials: LoginCredentials): void
}>()

const email = ref('')
const password = ref('')
const isSubmitting = ref(false)

const emailError = ref('')
const passwordError = ref('')
const submitError = ref('')

function validate(): boolean {
  emailError.value = ''
  passwordError.value = ''
  let valid = true

  if (!email.value.trim()) {
    emailError.value = 'E-Mail ist erforderlich.'
    valid = false
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value)) {
    emailError.value = 'Bitte eine gültige E-Mail-Adresse eingeben.'
    valid = false
  }

  if (!password.value) {
    passwordError.value = 'Passwort ist erforderlich.'
    valid = false
  } else if (password.value.length < 6) {
    passwordError.value = 'Das Passwort muss mindestens 6 Zeichen lang sein.'
    valid = false
  }

  return valid
}

async function handleSubmit() {
  if (!validate()) return

  isSubmitting.value = true
  emit('login', { email: email.value, password: password.value })
  isSubmitting.value = false
}



</script>

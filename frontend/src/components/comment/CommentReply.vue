<template>
  <div class="mt-4 flex flex-col gap-3">
    <div class="border border-gray-700 rounded-xl bg-gray-900 p-2 focus-within:border-gray-500 transition">
      <div class="flex gap-2 mb-2 px-2 text-sm text-gray-400">
        <button class="hover:text-white">Write</button>
        <button class="hover:text-white">Preview</button>
      </div>
      <textarea 
        v-model="replyText"
        placeholder="Reply to comment"
        class="w-full bg-transparent text-gray-200 outline-none resize-none px-2 py-1 placeholder-gray-500"
        rows="3"
      ></textarea>
      
      <div class="flex gap-3 px-2 pt-2 border-t border-gray-800 text-gray-400">
        <button class="font-bold hover:text-white">B</button>
        <button class="line-through hover:text-white">S</button>
        <button class="italic hover:text-white">I</button>
      </div>
    </div>

    <div class="flex gap-2">
      <button 
        @click="submitReply"
        class="px-4 py-1.5 bg-gray-700 hover:bg-gray-600 text-white rounded-full text-sm font-medium transition"
        :disabled="!replyText.trim()"
      >
        Reply
      </button>
      <button 
        @click="$emit('cancel')"
        class="px-4 py-1.5 hover:bg-gray-800 text-gray-300 rounded-full text-sm font-medium transition"
      >
        Cancel
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

const emit = defineEmits(['cancel', 'submit']);
const replyText = ref('');

const submitReply = () => {
  if (replyText.value.trim()) {
    emit('submit', replyText.value);
    replyText.value = ''; // Reset nach Submit
  }
};
</script>
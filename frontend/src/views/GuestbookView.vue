<template>
    <div class="container mt-25 divide-surface divide-y">
        <GuestbookHeader />
        <div>
            <GuestbookLanding /> 

            <GuestbookEditor /> 

             <GuestbookEntries />
        </div>
    </div>
</template>


<script setup lang="ts">
import GuestbookHeader from '@/components/guestbook/GuestbookHeader.vue';
import GuestbookEntries from '@/components/guestbook/GuestbookEntries.vue';
import GuestbookEditor from '@/components/guestbook/GuestbookEditor.vue';
import GuestbookLanding from '@/components/guestbook/GuestbookLanding.vue';


</script>
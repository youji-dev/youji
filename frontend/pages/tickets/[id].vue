<template>
  <component :is="Sidebar"></component>
  <div>
    <!-- Route: /tickets/[ticketId] -->
    <!-- Page for detail view of a ticket -->
  </div>
</template>

<script lang="ts" setup>
import Sidebar from '~/components/sidebar.vue';

definePageMeta({
    middleware: "auth"
})
</script>
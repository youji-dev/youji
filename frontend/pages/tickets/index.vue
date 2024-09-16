<template>
  <component :is="Sidebar"></component>
  <div>
    <!-- Route: /tickets/ -->
    <!-- Page for the list of all tickets -->
  </div>
</template>

<script lang="ts" setup>
import Sidebar from '~/components/sidebar.vue';

definePageMeta({
    middleware: "auth"
})
</script>
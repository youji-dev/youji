<template>
    <!-- Route: / -->
    <!-- Index page. Still undecided what the content of this should be or if we even want something like this. -->
    <!-- Could contain things like statistics of all tickets or a list of tickets assigned to the authenticated user (maybe ordered by priority) -->
    <!-- If we decide to leave not include this, the index page should be /tickets/ (tickets/index.vue) -->
    <component :is="Sidebar"></component>
</template>

<script lang="ts" setup>
import Sidebar from '~/components/sidebar.vue';

definePageMeta({
    middleware: "auth"
})
</script>

<style></style>
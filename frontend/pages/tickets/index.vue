<template>
  <div class="w-full min-h-[100vh] max-h-[100%] p-6 mt-8 md:mt-0">
    <h1 class="text-xl font-light text-start w-fit h-fit py-1">{{$t("ticketOverview")}}</h1>
    <div class="w-full min-h-[50%] h-[60rem] overflow-y-scroll flex flex-col base-bg-light dark:base-bg-dark rounded-md">

    </div>
  </div>
</template>

<script lang="ts" setup>
import Sidebar from '~/components/sidebar.vue';

definePageMeta({
    // middleware: "auth",
    layout: "authenticated"
})
</script>
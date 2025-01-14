<template>
  <div class="w-full">
    <h1 class="text-lg font-light text-neutral-700 dark:text-neutral-300 text-start w-fit h-fit">
      {{ text }}
    </h1>
  </div>
</template>

<script lang="ts" setup>
const props = defineProps({
  text: {
    type: String,
    required: true
  }
});
const { text } = props;
</script>

<style></style>
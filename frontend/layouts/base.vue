<template>
  <div class="page-bg-light dark:page-bg-dark min-h-lvh">
    <div class="fixed right-5 bottom-5 z-50">
      <ThemePicker />
      <LanguagePicker />
    </div>
    <slot />
  </div>
</template>

<script lang="ts" setup>
  import LanguagePicker from '~/components/languagePicker.vue';
  import ThemePicker from '~/components/themePicker.vue';
</script>

<style>
  @tailwind base;
  @tailwind components;
  @tailwind utilities;
  html,
  body {
    margin: 0;
    padding: 0;
  }
  @layer components {
    .page-bg-dark {
      background-color: #0a0a0a;
    }

    .page-bg-light {
      background-color: #f2f3f5;
    }

    .base-bg-dark {
      background-color: #141414;
    }

    .base-bg-light {
      background-color: #ffffff;
    }

    .text-dark {
      color: #e5eaf3;
    }

    .text-light {
      color: #303133;
    }

    .logo-dark {
      background-color: #e5eaf3;
    }

    .logo-light {
      background-color: #303133;
    }
  }

  ::-webkit-scrollbar {
    width: 5px;
    z-index: 50;
  }

  /* Track */
  ::-webkit-scrollbar-track {
    background: transparent;
    z-index: 50;
  }

  /* Handle */
  ::-webkit-scrollbar-thumb {
    background: #555;
    border-radius: 2rem;
    z-index: 50;
  }

  /* Handle on hover */
  ::-webkit-scrollbar-thumb:hover {
    background: #777;
  }
</style>

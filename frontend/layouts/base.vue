<template>
  <div class="page-bg-light dark:page-bg-dark">
    <!-- This file will be used as default layout on every page. It is typically used for backgrounds, navbars, ... -->
    <div class="fixed right-5 bottom-5 z-50">
      <Theme/>
      <Language/>
    </div>
    <slot />
  </div>
</template>

<script lang="ts" setup>
import Language from '~/components/language.vue';
import Theme from '~/components/theme.vue';
</script>

<style>

@tailwind base;
@tailwind components;
@tailwind utilities;

@layer components {
  .page-bg-dark {
    background-color: #0A0A0A;
  }

  .page-bg-light {
    background-color: #F2F3F5;
  }

  .base-bg-dark {
    background-color: #141414;
  }

  .base-bg-light {
    background-color: #FFFFFF;
  }

  .text-dark {
    color: #E5EAF3
  }

  .text-light {
    color: #303133;
  }

  .logo-dark {
    background-color: #E5EAF3
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
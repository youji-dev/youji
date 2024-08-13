<template>
  <div class="page-bg-light dark:page-bg-dark">
    <!-- This file will be used as default layout on every page. It is typically used for backgrounds, navbars, ... -->
    <slot />
    <div class="absolute right-5 bottom-5">
      <component :is="Theme"></component>
      <component :is="Language"></component>
    </div>
  </div>
</template>

<script lang="ts" setup>
const colorMode = useColorMode();
import Language from '~/components/language.vue';
import Theme from '~/components/theme.vue';
const { public: { COLORS } } = useRuntimeConfig();
onNuxtReady( () => {
  console.log(colorMode.value)
  
  // if (!document.documentElement.classList.contains(colorMode.value)) {
  //   if (colorMode.value === "system") {
  //     colorMode.value = "light";
  //     if (!document.documentElement.classList.contains("light")) {
  //       document.documentElement.classList.add("light")
  //     }
  //     return;
  //   }
  //   document.documentElement.classList.toggle("light");
  //   document.documentElement.classList.toggle("dark");
  // } 
  // console.log(document.documentElement.classList);
})
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

}
</style>
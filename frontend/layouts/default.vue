<template>
  <div class="page-bg-light dark:page-bg-dark w-full min-h-[100vh] max-h-full flex overflow-x-hidden overflow-y-scroll">
    <div class="fixed right-5 bottom-5 z-50">
      <Theme />
      <Language />
    </div>
    <Sidebar class="flex-none" />
    <SidebarMobile class="flex-none" />
    <slot />
  </div>
</template>

<script lang="ts" setup>
import Language from "~/components/language.vue";
import SidebarMobile from "~/components/sidebar-mobile.vue";
import Sidebar from "~/components/sidebar.vue";
import Theme from "~/components/theme.vue";
</script>

<style>
@tailwind base;
@tailwind components;
@tailwind utilities;

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
  background: rgba(0, 0, 0, 0.3);
  border-radius: 2rem;
  z-index: 50;
}

/* Handle on hover */
::-webkit-scrollbar-thumb:hover {
  background: #777;
}
</style>

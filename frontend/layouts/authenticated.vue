<template>
    <div class="page-bg-light dark:page-bg-dark">
        <component :is="Sidebar"></component>
        <component :is="SidebarMobile"></component>
      <!-- This file will be used as default layout on every page. It is typically used for backgrounds, navbars, ... -->
      <slot />
      <div class="absolute right-5 bottom-5">
        <component :is="Theme"></component>
        <component :is="Language"></component>
      </div>
    </div>
  </template>
  
  <script lang="ts" setup>
  import Language from '~/components/language.vue';
import SidebarMobile from '~/components/sidebar-mobile.vue';
import Sidebar from '~/components/sidebar.vue';
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
  </style>
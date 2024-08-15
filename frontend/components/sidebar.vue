<template>
  <div class="absolute px-6 py-3 left-0 top-0 h-full w-fit base-bg-light dark:base-bg-dark">
    <div class="flex items-center justify-center p-3">
      <svg width="35" height="35" viewBox="0 0 97 97" fill="none" xmlns="http://www.w3.org/2000/svg">
        <path
          d="M1 26.2181L46.5678 1.52121C47.853 0.82463 49.3994 0.826366 50.6831 1.52583L96 26.2181M30.2308 62.9684L66.7692 63.1154M30.2308 81.3846H66.7692M15.6154 33.8846V92.3462C15.6154 94.7624 16.8735 96 19.2692 96H77.7308C80.1265 96 81.3846 94.7624 81.3846 92.3462V33.8594C81.3846 31.4432 80.1265 30.2308 77.7308 30.2308H19.2692C16.8735 30.2308 15.6154 31.4683 15.6154 33.8846ZM30.2308 33.8846V42.9115C30.2308 45.3278 31.4889 46.5654 33.8846 46.5654H63.1154C65.5111 46.5654 66.7692 45.3278 66.7692 42.9115V33.8846C66.7692 31.4683 65.5111 30.2308 63.1154 30.2308H33.8846C31.4889 30.2308 30.2308 31.4683 30.2308 33.8846Z"
          :stroke="getFillColor()" stroke-width="1.5" stroke-linecap="round" />
      </svg>

      <h5 class="mb-2">Youji</h5>
    </div>
    <el-menu default-active="1" class="el-menu-vertical-demo" @open="handleOpen" @close="handleClose">
      <el-menu-item index="1">
        <el-icon>
          <Plus />
        </el-icon>
        <span>{{ $t("createTicket") }}</span>
      </el-menu-item>
      <el-menu-item index="2">
        <el-icon>
          <Files />
        </el-icon>
        <span>{{ $t("ticketOverview") }}</span>
      </el-menu-item>
    </el-menu>
  </div>
</template>

<script lang="ts" setup>
import { Files, Plus, Tickets } from '@element-plus/icons-vue';
const colorMode = useColorMode();
const handleOpen = (key: string, keyPath: string[]) => {
  console.log(key, keyPath)
}
const handleClose = (key: string, keyPath: string[]) => {
  console.log(key, keyPath)
}
 
function getFillColor() {
  if (colorMode.value === "light") {
    return "#000000";
  } else {
    return "#FFFFFF";
  }
}
</script>

<style>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 200px;
  min-height: 400px;
}
</style>
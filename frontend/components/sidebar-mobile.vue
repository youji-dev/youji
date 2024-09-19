<template>
  <div
    class="absolute px-6 py-3 left-0 top-0 h-full w-full bg-transparent block md:hidden z-10"
    :class="{'max-h-fit': !drawer}"
  >
    <div class="flex items-center justify-between w-full">
      <div class="flex items-center justify-center w-fit">
        <svg
          width="25"
          height="25"
          viewBox="0 0 97 97"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            class="stroke-neutral-700 dark:stroke-neutral-200"
            d="M1 26.2181L46.5678 1.52121C47.853 0.82463 49.3994 0.826366 50.6831 1.52583L96 26.2181M30.2308 62.9684L66.7692 63.1154M30.2308 81.3846H66.7692M15.6154 33.8846V92.3462C15.6154 94.7624 16.8735 96 19.2692 96H77.7308C80.1265 96 81.3846 94.7624 81.3846 92.3462V33.8594C81.3846 31.4432 80.1265 30.2308 77.7308 30.2308H19.2692C16.8735 30.2308 15.6154 31.4683 15.6154 33.8846ZM30.2308 33.8846V42.9115C30.2308 45.3278 31.4889 46.5654 33.8846 46.5654H63.1154C65.5111 46.5654 66.7692 45.3278 66.7692 42.9115V33.8846C66.7692 31.4683 65.5111 30.2308 63.1154 30.2308H33.8846C31.4889 30.2308 30.2308 31.4683 30.2308 33.8846Z"
            stroke-width="5"
            stroke-linecap="round"
          />
        </svg>

        <h1 class="p-3 font-semibold text-center text-neutral-700 dark:text-neutral-200">
          youji
        </h1>
      </div>

      <Icon
        name="hugeicons:menu-01"
        @click="drawer = true"
        class="text-neutral-700 dark:text-neutral-300 text-3xl py-6 cursor-pointer"
      />
    </div>

    <el-drawer
      v-model="drawer"
      :title="$t('navigation')"
      :direction="'ltr'"
      style="min-width: 60%"
    >
    <el-menu
    default-active="2"
    class="el-menu-vertical-demo pt-5"
    :collapse="false"
    style="
      border-right: 0px;
      display: flex;
      overflow-y: hidden;
      justify-content: space-between;
      flex-direction: column;
      flex-flow: column;
      min-height: 0;
      height: 95%;
    "
  >
    <div>
      <el-menu-item index="2">
        <el-icon>
          <Files />
        </el-icon>
        <el-badge :value="9" type="primary" style="" :offset="[10, 15]">
          <span class="w-fit h-fit">{{ $t("ticketOverview") }}</span>
        </el-badge>
      </el-menu-item>

      <el-menu-item index="1" style="width: 100%; height: fit-content">
        <el-icon>
          <Plus />
        </el-icon>
        <span>{{ $t("createTicket") }}</span>
      </el-menu-item>
    </div>
    <div class="">
      <el-divider></el-divider>
      <el-menu-item style="width: 100%; height: fit-content" class="">
        <el-icon class="-rotate-90" color="#EF4444">
          <Upload />
        </el-icon>
        <span>{{ $t("logout") }}</span>
      </el-menu-item>
      <el-menu-item style="width: 100%; height: fit-content" class="" index="3">
        <el-icon class="-rotate-90">
          <Setting />
        </el-icon>
        <span>{{ $t("settings") }}</span>
      </el-menu-item>
    </div>
  </el-menu>
    </el-drawer>
  </div>
</template>

<script lang="ts" setup>
import { Files, Plus, Setting, Upload } from "@element-plus/icons-vue";
const drawer = ref(false);
</script>

<style>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  min-width: 100%;
  min-height: 400px;
}
</style>

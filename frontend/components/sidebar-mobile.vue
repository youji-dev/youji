<template>
  <div
    class="absolute px-6 py-3 left-0 top-0 h-full w-full bg-transparent block md:hidden"
    :class="{ 'max-h-fit': !drawer }"
  >
    <div class="flex items-center justify-between w-full">
      <Logo />
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
      <template #header="{}" style="margin-bottom: 0"> </template>

      <el-menu default-active="2" class="el-menu-vertical-demo">
        <div>
          <el-menu-item index="2">
            <el-icon>
              <Files />
            </el-icon>
            <el-badge :value="9" type="primary" :offset="[10, 15]">
              <span class="w-fit h-fit">{{ $t("ticketOverview") }}</span>
            </el-badge>
          </el-menu-item>

          <el-menu-item index="1" class="menu-item">
            <el-icon>
              <Plus />
            </el-icon>
            <span>{{ $t("createTicket") }}</span>
          </el-menu-item>
        </div>
        <div>
          <el-divider></el-divider>
          <el-menu-item class="menu-item">
            <el-icon class="-rotate-90" color="#EF4444">
              <Upload />
            </el-icon>
            <span>{{ $t("logout") }}</span>
          </el-menu-item>
          <el-menu-item class="menu-item" index="3">
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
import Logo from "./logo.vue";
const drawer = ref(false);
</script>

<style>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  min-width: 100%;
  min-height: 400px;
  border-right: 0px;
  display: flex;
  overflow-y: hidden;
  justify-content: space-between;
  flex-direction: column;
  flex-flow: column;
  min-height: 0;
  height: 95%;
}

.menu-item {
  width: 100%;
  height: fit-content;
}
</style>

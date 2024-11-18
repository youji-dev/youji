<template>
  <div
    id="navbar"
    class="max-h-[100vh] px-6 py-3 md:w-[40%] lg:w-[30%] xl:w-[20%] 2xl:w-[15%] w-fit base-bg-light dark:base-bg-dark hidden md:block lg:block overflow-x-hidden overflow-y-scroll z-10 nav-height">
    <Logo />
    <el-menu default-active="2" class="el-menu-vertical-demo pt-5">
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
  </div>
</template>

<script lang="ts" setup>
import { Files, Plus, Setting, Upload } from "@element-plus/icons-vue";
</script>

<style>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 100%;
  border-right: 0px;
  display: flex;
  overflow-y: hidden;
  overflow-x: hidden;
  justify-content: space-between;
  flex-direction: column;
  flex-flow: column;
  min-height: 0;
  height: calc(100vh - 72px);
}

.menu-item {
  width: 100%;
  height: fit-content;
}
</style>

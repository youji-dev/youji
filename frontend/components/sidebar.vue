<template>
  <div
    class="max-h-[100vh] px-6 py-3 max-w-fit min-w-fit base-bg-light dark:base-bg-dark hidden lg:block overflow-x-hidden overflow-y-scroll z-10 nav-height"
    id="navbar"
  >
    <div class="flex items-center justify-between">
      <Logo />
      <div class="flex items-center justify-end">
        <Theme />
        <Language />
      </div>
    </div>
    <el-menu
      :default-active="getPageIndex()"
      class="el-menu-vertical-demo pt-5"
    >
      <div>
        <el-menu-item
          index="1"
          @click="router.push(localeRoute('/tickets')?.fullPath as string)"
        >
          <el-icon>
            <Files />
          </el-icon>
          <el-badge
            v-bind:hidden="!openTicketsCount"
            :value="openTicketsCount ?? 0"
            type="primary"
            :offset="[-125, 15]"
          >
            <span class="w-fit h-fit">{{ $t("ticketOverview") }}</span>
          </el-badge>
        </el-menu-item>

        <el-menu-item
          index="2"
          class="menu-item"
          @click="router.push(localeRoute('/tickets/new')?.fullPath as string)"
        >
          <el-icon>
            <Plus />
          </el-icon>
          <span>{{ $t("createTicket") }}</span>
        </el-menu-item>
      </div>
      <div>
        <el-divider></el-divider>
        <el-menu-item
          class="menu-item"
          @click="router.push(localeRoute('/logout')?.fullPath as string)"
        >
          <el-icon class="-rotate-90" color="#EF4444">
            <Upload />
          </el-icon>
          <span>{{ $t("logout") }}</span>
        </el-menu-item>
        <el-menu-item
          class="menu-item"
          index="3"
          @click="router.push(localeRoute('/settings')?.fullPath as string)"
        >
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

const localeRoute = useLocaleRoute();
const route = useRoute();
const routeObject = reactive({ route });
const { locale } = useI18n();
const router = useRouter();

const props = defineProps<{
  openTicketsCount: number | null;
}>();

function getPageIndex() {
  if (
    routeObject.route.fullPath ==
    localeRoute("/tickets", locale.value)?.fullPath
  ) {
    return "1";
  } else if (
    routeObject.route.fullPath ==
    localeRoute("/tickets/new", locale.value)?.fullPath
  ) {
    return "2";
  } else if (
    routeObject.route.fullPath ==
    localeRoute("/settings", locale.value)?.fullPath
  ) {
    return "3";
  } else {
    return "0";
  }
}
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

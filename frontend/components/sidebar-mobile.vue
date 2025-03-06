<template>
  <div
    class="absolute px-6 py-3 left-0 top-0 h-fit w-full bg-transparent block lg:hidden z-10"
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
      :title="$t('ticketSystem')"
      :with-header="false"
      :direction="'ltr'"
      style="min-width: fit-content; max-width: fit-content; height: 100vh"
    >
      <template #header="{}" style="margin-bottom: 0"> </template>
      <template #body style="overflow-y: hidden"></template>
      <el-menu
        :default-active="getPageIndex()"
        class="el-menu-vertical-demo h-full"
      >
        <div>
          <div class="flex items-center justify-between">
            <Logo />
          </div>
          <el-menu-item
            index="1"
            @click="router.push(localeRoute('/tickets')?.fullPath as string); drawer = !drawer;"
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
            @click="
              router.push(localeRoute('/tickets/new')?.fullPath as string); drawer = !drawer;
            "
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
            @click="router.push(localeRoute('/logout')?.fullPath as string); drawer = !drawer;"
          >
            <el-icon class="-rotate-90" color="#EF4444">
              <Upload />
            </el-icon>
            <span>{{ $t("logout") }}</span>
          </el-menu-item>
          <el-menu-item
            class="menu-item"
            index="3"
            @click="router.push(localeRoute('/settings')?.fullPath as string); drawer = !drawer;"
          >
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
const router = useRouter();
const localeRoute = useLocaleRoute();
const route = useRoute();
const routeObject = reactive({ route });
const { locale } = useI18n();

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
  min-width: 100%;
  min-height: 400px;
  border-right: 0px;
  display: flex;
  overflow-y: hidden;
  justify-content: space-between;
  flex-direction: column;
  flex-flow: column;
  min-height: 0;
  height: 100%;
}

.menu-item {
  width: 100%;
  height: fit-content;
}
</style>

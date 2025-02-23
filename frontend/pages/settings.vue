<template>
  <div
    class="w-full h-[calc(100vh - 74px)] lg:h-[calc(100vh - 10px)] p-6 mt-9 lg:mt-0"
  >
    <div
      class="w-full h-full overflow-y-scroll flex flex-col justify-center items-center base-bg-light dark:base-bg-dark rounded-md"
      id="table_container"
    >
      <PageHeader :text="$t('settings')" class="p-3" />
      <div class="w-full h-full p-3">
        <el-tabs
          type="card"
          class="demo-tabs"
          v-model="activeName"
          @tab-click="handleClick"
        >
          <el-tab-pane :label="$t('user')" name="user">
            <User/>
          </el-tab-pane>
          <el-tab-pane :label="$t('global')" name="global">
            <Global/>
          </el-tab-pane>
        </el-tabs>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import PageHeader from "~/components/pageHeader.vue";
import { ref } from "vue";
import type { TabsPaneContext } from "element-plus";
import Global from "~/components/settings/global.vue";
import User from "~/components/settings/user.vue";
const activeName = ref("user");
const handleClick = (tab: TabsPaneContext, event: Event) => {
  console.log(tab, event);
};
</script>
<style>
.demo-tabs > .el-tabs__content {
  padding: 32px;
  color: #6b778c;
  font-size: 32px;
  font-weight: 600;
}
</style>

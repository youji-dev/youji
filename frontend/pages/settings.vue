<template>
  <div
    class="w-full h-[calc(100vh - 74px)] p-6 mt-9 lg:mt-0"
    id="globalSettings">
    <div
      class="w-full h-full overflow-y-scroll flex flex-col justify-center items-center base-bg-light dark:base-bg-dark rounded-md"
      id="table_container">
      <div class="w-full h-full p-3">
        <el-tabs
          type="card"
          class="demo-tabs"
          v-model="activeName">
          <el-tab-pane
            :label="$t('user')"
            name="user">
            <User />
          </el-tab-pane>
          <el-tab-pane
            v-if="isUserAdmin"
            :label="$t('global')"
            name="global">
            <Global />
          </el-tab-pane>
        </el-tabs>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
  import { ref } from 'vue';
  import Global from '~/components/settings/global.vue';
  import User from '~/components/settings/user.vue';
  const activeName = ref('user');
  const { isUserAdmin } = storeToRefs(useAuthStore());
  const { initialLoading } = storeToRefs(useSettingsStore());
  const { fetchBuildings, fetchPriorities, fetchStates, fetchUsers, fetchMyUser } = useSettingsStore();

  onNuxtReady(async () => {
    await fetchUsers();
    await fetchBuildings();
    await fetchPriorities();
    await fetchStates();
    await fetchMyUser();
    initialLoading.value = false;
  });
</script>
<style>
  .demo-tabs > .el-tabs__content {
    padding: 32px;
    color: #6b778c;
    font-size: 32px;
    font-weight: 600;
  }
</style>

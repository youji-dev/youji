<template>
  <el-popover
    ref="popover"
    placement="top"
    :width="100"
    trigger="click">
    <div
      v-for="availLocale in availableLocales"
      :key="availLocale.code"
      class="flex items-center justify-between px-6 py-1 cursor-pointer hover:text-blue-400"
      @click="switchLocale(availLocale.code)">
      <div class="flex items-center w-1/3">
        <Icon :name="availLocale.icon" />
      </div>
      <div class="flex items-center w-2/3">
        <h1 class="txt-sm font-light text-center">{{ availLocale.name }}</h1>
      </div>
    </div>
    <template #reference>
      <el-button
        type="primary"
        :size="'default'"
        plain
        circle>
        <Icon
          name="material-symbols:language"
          :style="{
            backgroundColor: colorMode.value === 'light' ? TEXT_LIGHT : TEXT_DARK,
          }" />
      </el-button>
    </template>
  </el-popover>
</template>

<script lang="ts" setup>
  const { locales } = useI18n();
  const colorMode = useColorMode();
  const {
    public: { TEXT_LIGHT, TEXT_DARK },
  } = useRuntimeConfig();

  const availableLocales = computed(() => {
    return locales.value;
  });
  const { myUser } = storeToRefs(useSettingsStore());
  const { updateMyUser, fetchUsers, fetchMyUser } = useSettingsStore();
  const popover = ref();
  const switchLocale = async (localeKey: string) => {
    if (popover.value) {
      popover.value.hide();
    }

    if (myUser.value === null) return;
    await updateMyUser({
      userId: myUser.value.userId.value,
      newPreferredEmailLcid: localeKey,
    });
    await fetchUsers();
    fetchMyUser();
  };
</script>

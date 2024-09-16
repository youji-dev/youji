<template class="z-10">
  <el-popover placement="top" :width="100" trigger="click">
    <div v-for="availLocale in availableLocales" @click="switchLocale(availLocale.code)"
      class="flex items-center justify-between px-6 py-1 cursor-pointer hover:text-blue-400">
      <div class="flex items-center w-1/3">
        <Icon :name="availLocale.icon" />
      </div>
      <div class="flex items-center w-2/3">
        <h1 class="txt-sm font-light text-center ">{{ availLocale.name }}</h1>
      </div>
    </div>
    <template #reference>
      <el-button circle>
        <Icon name="material-symbols:language"
          :style="{ 'backgroundColor': colorMode.value === 'light' ? TEXT_LIGHT : TEXT_DARK }" />
      </el-button>
    </template>
  </el-popover>
</template>

<script lang="ts" setup>
const { locale, locales } = useI18n();
const router = useRouter();
const i18n = useI18n();
const localePath = useLocalePath();
const switchLocalePath = useSwitchLocalePath();
const colorMode = useColorMode();
const { public: { TEXT_LIGHT, TEXT_DARK } } = useRuntimeConfig();
const availableLocales = computed(() => {
  return locales.value;
});

const switchLocale = (localeKey: string) => {
  console.log(localeKey)
  router.push(switchLocalePath(localeKey));
  ElMessage({
    type:"success",
    message: i18n.t("localeSwitched")
  })
};
</script>

<style></style>
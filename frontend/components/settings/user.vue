<template>
  <div class="grid grid-cols-1 lg:grid-cols-2">
    <div class="flex flex-col">
      <div class="flex justify-between lg:justify-between items-center py-2">
        <h1 class="text-lg">{{ $t("theme") }}</h1>
        <div class="pl-3 flex items-center">
          <h1 class="text-sm px-3">
            {{
              colorMode.preference === "dark"
                ? $t("dark")
                : colorMode.preference === "light"
                ? $t("light")
                : $t("system")
            }}
          </h1>
          <Theme />
        </div>
      </div>
      <div class="flex justify-between lg:justify-between items-center py-2">
        <h1 class="text-lg">{{ $t("language") }}</h1>
        <div class="pl-3 flex items-center">
          <h1 class="text-sm px-3">
            {{ locale }}
          </h1>
          <Language />
        </div>
      </div>

      <el-divider />
      <div class="flex justify-between lg:justify-between items-center py-2">
        <h1 :title="$t('receiveEmailNotifications')" class="text-lg overflow-hidden text-nowrap overflow-ellipsis w-[75%]">{{ $t("receiveEmailNotifications") }}</h1>
        <el-switch></el-switch>
      </div>
      <div class="flex justify-between lg:justify-between items-center py-2">
        <h1 class="text-lg">{{ $t("languageEmail") }}</h1>
        <div class="pl-3 flex items-center">
          <h1 class="text-sm px-3">
            {{ locale }}
          </h1>
          <LanguageEmail />
        </div>
      </div>
    </div>
    <div class="hidden lg:block"></div>
  </div>
</template>

<script lang="ts" setup>
import Language from "../language.vue";
import LanguageEmail from "../languageEmail.vue";
import Theme from "../theme.vue";
const colorMode = useColorMode();
const i18n = useI18n();
const locale = i18n.localeProperties.value.name;
</script>

<style></style>

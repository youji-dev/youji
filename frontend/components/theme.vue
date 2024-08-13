<template>
  <el-popover
    placement="top-start"
    :title="$t('colorMode')"
    :width="200"
    trigger="hover"
    :content="$t(colorMode.preference)"
  >
    <template #reference>
      <el-button
        @click="toggleTheme"
        v-show="colorMode.preference === 'dark'"
        circle
        :icon="Sunny"
      ></el-button>
    </template>
  </el-popover>
  <el-popover
    placement="top-start"
    :title="$t('colorMode')"
    :width="200"
    trigger="hover"
    :content="$t(colorMode.preference)"
  >
    <template #reference>

      <el-button
        @click="toggleTheme"
        v-show="colorMode.preference === 'light'"
        circle
        :icon="Moon"
      ></el-button>
    </template>
  </el-popover>
  <el-popover
    placement="top-start"
    :title="$t('colorMode')"
    :width="200"
    trigger="hover"
    :content="$t(colorMode.preference)"
  >
    <template #reference>
      <el-button
        @click="toggleTheme"
        v-show="colorMode.preference === 'system'"
        circle
        :icon="Monitor"
      ></el-button>
    </template>
  </el-popover>
</template>

<script lang="ts" setup>
import { Monitor, Moon, Sunny } from "@element-plus/icons-vue";

const colorMode = useColorMode();
const toggleTheme = () => {
  document.documentElement.classList.add(
    colorMode.value === "dark" ? "light" : "dark"
  );
  document.documentElement.classList.remove(colorMode.value);
  if (colorMode.preference === "light") {
    colorMode.preference = "dark";
  } else if (colorMode.preference === "dark") {
    colorMode.preference = "system";
  } else if (colorMode.preference === "system") {
    colorMode.preference = "light";
  }
};
</script>

<style></style>

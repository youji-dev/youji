<template class="z-10">
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
        circle
        :icon="determineIcon()"
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
  if (colorMode.preference === "system") {
    const prev = colorMode.value;
    colorMode.preference = "dark" // Emit color mode changed event?
    colorMode.preference= "system";
  }
};

function determineIcon() {
  if (colorMode.preference === "light") {
    return Sunny;
  } else if (colorMode.preference === "dark") {
    return Moon;
  } else if (colorMode.preference === "system") {
    return Monitor;
  } else {
    return Sunny;
  }
}
</script>

<style></style>

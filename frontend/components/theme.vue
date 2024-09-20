<template>
      <el-button
        @click="toggleTheme"
        circle
        type="primary"
        :icon="determineIcon()"
      ></el-button>
</template>

<script lang="ts" setup>
import { Monitor, Moon, Sunny } from "@element-plus/icons-vue";

const colorMode = useColorMode();
const toggleTheme = () => {
  document.documentElement.classList.remove(colorMode.value);
  if (colorMode.preference === "light") {
    colorMode.preference = "dark";
    document.documentElement.classList.add("dark");
  } else if (colorMode.preference === "dark") {
    colorMode.preference = "system";
    document.documentElement.classList.add(colorMode.value);
  } else if (colorMode.preference === "system") {
    colorMode.preference = "light";
    document.documentElement.classList.add("light");
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

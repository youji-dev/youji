<template>
  <el-button @click="toggleTheme" :size="'small'" circle plain type="primary" :icon="icon"></el-button>
</template>

<script lang="ts" setup>
import { Monitor, Moon, Sunny } from "@element-plus/icons-vue";
const icon = ref();
const colorMode = useColorMode();
onMounted(() => {
  determineIcon();
})

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
  determineIcon();
};

function determineIcon() {
  if (colorMode.preference === "light") {
    icon.value = Sunny;
  } else if (colorMode.preference === "dark") {
    icon.value = Moon;
  } else if (colorMode.preference === "system") {
    icon.value = Monitor;
  } else {
    icon.value =Sunny;
  }
}
</script>

<template>
  <el-button @click="toggleTheme" :size="'small'" circle plain type="primary" :icon="icons[icon]"></el-button>
</template>

<script lang="ts" setup>
import { Monitor, Moon, Sunny } from "@element-plus/icons-vue";
// Instead of storing the icon as ref and changing it directly in the determineIcon function, I implemented it this way to avoid storing a component as a reactive object.
// According to the error this outputs in the console, storing a Component as a reactive object can cause performance overhead.
const icon = ref() as Ref<number>;
let icons = [Monitor, Moon, Sunny];
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
    icon.value = 2;
  } else if (colorMode.preference === "dark") {
    icon.value = 1;
  } else if (colorMode.preference === "system") {
    icon.value = 0;
  } else {
    icon.value = 2;
  }
}
</script>

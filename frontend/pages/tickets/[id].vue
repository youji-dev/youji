<template>
  <!-- Route: /tickets/[ticketId] -->
  <!-- Page for detail view of a ticket -->
  <!-- header -->
  <div class=" h-[100vh] p-6 mt-8 lg:mt-0" style="height: calc(100vh - 72px)" :style="{width: width}">
    <div class="flex m-2">
      <!-- backbutton -->
      <el-button class="text-sm flex-none" type="text" link @click="router.back()" :icon="ArrowLeft">{{ $t("back")
        }}</el-button>
      <!-- seperator -->
      <el-divider class="flex-none self-center" direction="vertical" />
      <!-- Title -->
      <el-text class="font-semibold flex-auto truncate"
        size="large">TicdasdsadsaddsadsadsadsadsadsadsadasdasdsaketName</el-text>
      <!-- Edit Button -->
      <el-button class="text-sm flex-none justify-self-end" type="primary" :icon="EditPen" @click="toggleEditMode">{{
        $t("edit")
      }}</el-button>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ArrowLeft, EditPen } from "@element-plus/icons-vue";
const router = useRouter();
</script>

<script lang="ts">
const editMode = ref(false);
const width = ref("100vw")
onNuxtReady(() => {
  determineViewWidth();
  window.addEventListener("resize", determineViewWidth);
});
function toggleEditMode() {
  editMode.value = !editMode.value;
}
function determineViewWidth() {
  if (typeof document === "undefined") return;
  const navbar = document.getElementById("navbar");
  if (typeof navbar === "undefined") return;
  if (typeof navbar?.offsetWidth === "undefined") return;
  width.value = window.innerWidth - navbar?.offsetWidth + "px";
  const table = document.querySelector("el-table__inner-wrapper");
  if (!!!table) return;

  return;
}
</script>

<style></style>
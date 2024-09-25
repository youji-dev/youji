<template>
  <!-- Route: /tickets/[ticketId] -->
  <!-- Page for detail view of a ticket -->
  <!-- header -->
  <div class="flex-1">
    <div class="flex m-2">
      <!-- backbutton -->
      <el-button class="text-sm flex-none" type="text" link @click="router.back()" :icon="ArrowLeft">{{ $t("back")
        }}</el-button>
      <!-- seperator -->
      <el-divider class="flex-none self-center" direction="vertical" />
      <!-- Title -->
      <el-text class="font-semibold flex-auto text-black min-w-0 text-ellipsis overflow-hidden"
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
function toggleEditMode() {
  editMode.value = !editMode.value;
}
</script>

<style></style>
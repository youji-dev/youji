<template>
  <!-- Route: /tickets/[ticketId] -->
  <!-- Page for detail view of a ticket -->
  <!-- header -->
  <div class="flex">
    <!-- backbutton -->
    <el-button type="text" link @click="router.back()" :icon="ArrowLeft" class="text-sm">{{ $t("back") }}</el-button>
    <!-- seperator -->
    <el-divider direction="vertical" />
    <!-- Title -->
    <el-text size="large" :truncated=true class="font-semibold">TicketName</el-text>
    <!-- Edit Button -->
    <el-button type="primary" :icon="EditPen">{{ $t("edit") }}</el-button>
  </div>
</template>

<script lang="ts" setup>
import { ArrowLeft, EditPen } from "@element-plus/icons-vue";
const router = useRouter();
</script>
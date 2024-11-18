<template>
  <div class="flex">
    <el-button class="text-sm" link @click="router.back()" :icon="ArrowLeft">{{ $t("back") }}</el-button>
    <el-divider class="self-center" direction="vertical" />
    <el-text class="font-semibold truncate" size="large">{{ ticket.title }}</el-text>
  </div>
</template>

<script lang="ts" setup>
import type ticket from "~/types/api/response/ticketResponse";
import { ArrowLeft } from "@element-plus/icons-vue";

defineProps<{
  ticket: ticket;
}>();

var router = useRouter();
</script>

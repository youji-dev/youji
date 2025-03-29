<template>
  <div class="flex">
    <el-button
      class="text-sm"
      link
      :icon="ArrowLeft"
      @click="router.back()"
      >{{ $t('back') }}</el-button
    >
    <el-divider
      class="self-center"
      direction="vertical" />
    <el-text
      class="font-semibold truncate"
      size="large"
      >{{ ticket.title }}</el-text
    >
  </div>
</template>

<script lang="ts" setup>
  import type ticketType from '~/types/api/response/ticketResponse';
  import { ArrowLeft } from '@element-plus/icons-vue';

  defineProps<{
    ticket: ticketType;
  }>();

  const router = useRouter();
</script>

<template>
  <el-dialog
    v-model="isVisible"
    width="500"
    :title="title"
    @closed="() => emit('closed')">
    <p
      v-if="itemName"
      style="font-weight: bold">
      {{ itemName }}
    </p>
    <span>{{ description }}</span>
    <template #footer>
      <div class="dialog-footer">
        <el-button
          type="default"
          @click="isVisible = false"
          >{{ $t('close') }}</el-button
        >
        <el-button
          type="danger"
          :loading="loading"
          @click="emit('confirm')">
          {{ $t('delete') }}
        </el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup>
  const isVisible = defineModel<boolean>('visible', { required: true });

  defineProps<{
    title: string;
    description: string;
    itemName?: string;
    loading?: boolean;
  }>();

  const emit = defineEmits(['confirm', 'closed']);
</script>

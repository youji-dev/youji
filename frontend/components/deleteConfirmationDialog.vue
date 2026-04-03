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
      <div class="shift-hint">
        <el-divider
          border-style="dashed"
          class="mb-2 mt-3" />
        <el-text
          size="small"
          type="info"
          class="block center text-left">
          {{ $t('shiftClickToSkip') }}
        </el-text>
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

  const isShiftHeld = ref(false);

  /**
   * Tracks Keydown events to determine if Shift is held, allowing for a quick delete action.
   * @param e KeyboardEvent object
   */
  function onKeyDown(e: KeyboardEvent) {
    if (e.key === 'Shift') isShiftHeld.value = true;
  }
  /**
   * Tracks Keyup events to reset the Shift key state.
   * @param e KeyboardEvent object
   */
  function onKeyUp(e: KeyboardEvent) {
    if (e.key === 'Shift') isShiftHeld.value = false;
  }

  onMounted(() => {
    window.addEventListener('keydown', onKeyDown);
    window.addEventListener('keyup', onKeyUp);
  });
  onUnmounted(() => {
    window.removeEventListener('keydown', onKeyDown);
    window.removeEventListener('keyup', onKeyUp);
  });

  watch(isVisible, val => {
    if (val && isShiftHeld.value) {
      isVisible.value = false;
      emit('confirm');
    }
  });
</script>

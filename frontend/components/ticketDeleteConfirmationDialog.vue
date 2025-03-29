<template>
  <el-dialog
    v-model="isVisible"
    width="500"
    :before-close="beforeClose"
    :title="i18n.t('deleteTicketTitle')"
    @closed="beforeClose">
    <p style="font-weight: bold">{{ ticket?.title }}</p>
    <span>{{ $t('deleteTicketDescription') }}</span>
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
          @click="deleteTicket()">
          {{ $t('delete') }}
        </el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup>
  import type ticket from '~/types/api/response/ticketResponse';

  const ticketModel = defineModel<ticket | null>('ticket', { required: true });
  const isVisible = defineModel<boolean>('visible', { required: true });
  const beforeClose = defineModel<void>('beforeClose', { required: true });

  const emit = defineEmits(['deleted']);

  const loading = ref(false);

  const { $api } = useNuxtApp();
  const i18n = useI18n();

  /**
   * Deletes the ticket that this dialog is associated with
   */
  async function deleteTicket() {
    loading.value = true;
    try {
      if (!ticketModel.value?.id) {
        throw new Error(i18n.t('error'));
      }
      const deleteResult = await $api.ticket.delete(ticketModel.value.id);

      if (deleteResult.error.value) {
        if (deleteResult.error.value.statusCode === 403) {
          throw new Error(i18n.t('forbidden'));
        }
        if (deleteResult.error.value.statusCode === 500) {
          throw new Error('serverError');
        }
        if (deleteResult.error.value.message) {
          throw new Error(deleteResult.error.value.message);
        }
        if (deleteResult.error.value.data) {
          throw new Error(deleteResult.error.value.data);
        } else {
          throw new Error(i18n.t('error'));
        }
      }

      if (deleteResult.data.value) {
        ElMessage({
          message: i18n.t('deleted'),
          type: 'success',
          duration: 5000,
        });
        emit('deleted');
        isVisible.value = false;
      }
    } catch (error) {
      ElNotification({
        title: i18n.t('error'),
        message: (error as Error).message,
        type: 'error',
        duration: 5000,
      });
    } finally {
      loading.value = false;
    }
  }
</script>

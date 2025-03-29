<template>
  <el-card
    v-loading="loading"
    class="drop-shadow-xl base-bg-light dark:base-bg-dark lg:min-h-[13.9rem]">
    <el-input
      v-model="newComment"
      type="textarea"
      resize="vertical"
      :rows="3"
      :placeholder="$t('newComment')" />
    <el-button
      class="mt-2 float-end"
      type="primary"
      size="small"
      @click="sendComment()"
      >{{ $t('sendComment') }}</el-button
    >

    <el-divider class="mt-10 mb-3" />

    <el-timeline>
      <el-timeline-item
        v-for="comment in sortedComments"
        :key="comment.id"
        class="drop-shadow-xl"
        :timestamp="new Date(comment.creationDate).toLocaleString()"
        placement="top">
        <TicketComment
          :comment="comment"
          :ticket="ticketModel" />
      </el-timeline-item>
    </el-timeline>
  </el-card>
</template>

<script lang="ts" setup>
  import type ticketComment from '~/types/api/response/ticketCommentResponse';
  import type ticket from '~/types/api/response/ticketResponse';
  import TicketComment from './ticketComment.vue';

  const { $api } = useNuxtApp();
  const i18n = useI18n();
  const route = useRoute();

  const ticketModel = defineModel<ticket>('ticket', { required: true });
  const sortedComments = computed(() => {
    return [...(ticketModel.value.comments ?? [])].sort(sortCommentsByDate);
  });
  const newComment = ref('');
  const loading = ref(false);

  /**
   * Creates a comment under the ticket
   */
  async function sendComment() {
    try {
      if (newComment.value === '') {
        ElNotification({
          title: i18n.t('error'),
          message: i18n.t('commentEmpty'),
          type: 'error',
          duration: 5000,
        });
        return;
      }

      loading.value = true;
      const commentPostResult = await $api.ticket.addComment(route.params.id as string, newComment.value);

      if (commentPostResult.error.value) {
        loading.value = false;
        if (commentPostResult.error.value.statusCode === 404) {
          throw new Error(i18n.t('resourceNotFound'));
        }
        if (commentPostResult.error.value.statusCode === 500) {
          throw new Error('serverError');
        }
        if (commentPostResult.error.value.message) {
          throw new Error(commentPostResult.error.value.message);
        }
        if (commentPostResult.error.value.data) {
          throw new Error(commentPostResult.error.value.data);
        } else {
          throw new Error(i18n.t('error'));
        }
      }

      ticketModel.value.comments = (await $api.ticket.getComments(route.params.id as string)).data.value ?? [];
      newComment.value = '';
      ElNotification({
        title: i18n.t('success'),
        message: i18n.t('commentPostSuccess'),
        type: 'success',
        duration: 5000,
      });
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

  /**
   * Sorting function for comments
   * @param a First comment
   * @param b Second comment
   * @returns The comparison result
   */
  function sortCommentsByDate(a: ticketComment, b: ticketComment) {
    return new Date(b.creationDate).getTime() - new Date(a.creationDate).getTime();
  }
</script>

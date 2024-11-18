<template>
  <el-card class="block">
    <div class="flex justify-between">
      <el-text size="large" tag="b" type="primary">{{ commentModel.author }}</el-text>
      <el-button :loading="loading" size="small" :icon="Delete" @click="deleteComment()" />
    </div>
    <el-text size="default">{{ commentModel.content }}</el-text>
  </el-card>
</template>

<script lang="ts" setup>
import { Delete } from "@element-plus/icons-vue";
import type ticketComment from "~/types/api/response/ticketCommentResponse";
import type ticket from "~/types/api/response/ticketResponse";

const { $api } = useNuxtApp();
const route = useRoute();
const i18n = useI18n();
const loading = ref(false);
const commentModel = defineModel<ticketComment>("comment", { required: true });
const ticketModel = defineModel<ticket>("ticket", { required: true });

async function deleteComment() {
  try {
    loading.value = true;
    let commentDeleteResult = await $api.comment.delete(commentModel.value.id);

    if (commentDeleteResult.error.value) {
      loading.value = false;
      if (commentDeleteResult.error.value.statusCode === 404) {
        throw new Error(i18n.t("resourceNotFound"));
      }
      if (commentDeleteResult.error.value.statusCode === 403) {
        throw new Error(i18n.t("forbidden"));
      }
      if (commentDeleteResult.error.value.statusCode === 500) {
        throw new Error("serverError");
      }
      if (commentDeleteResult.error.value.message) {
        throw new Error(commentDeleteResult.error.value.message);
      }
      if (commentDeleteResult.error.value.data) {
        throw new Error(commentDeleteResult.error.value.data);
      } else {
        throw new Error(i18n.t("error"));
      }
    }

    ticketModel.value.comments = (await $api.ticket.getComments(route.params.id as string)).data.value ?? [];
    ElNotification({
      title: i18n.t("success"),
      message: i18n.t("commentDeleteSuccess"),
      type: "success",
      duration: 5000,
    });
  } catch (error) {
    ElNotification({
      title: i18n.t("error"),
      message: (error as Error).message,
      type: "error",
      duration: 5000,
    });
  } finally {
    loading.value = false;
  }
}
</script>

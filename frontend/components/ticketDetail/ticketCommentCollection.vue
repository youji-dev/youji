<template>
  <el-card v-loading="loading" class="drop-shadow-xl base-bg-light dark:bg-black">
    <el-input v-model="newComment" type="textarea" resize="vertical" :rows="3" :placeholder="$t('newComment')" />
    <el-button class="mt-2 float-end" type="primary" size="small" @click="sendComment()">{{ $t("sendComment") }}</el-button>

    <el-divider class="mt-10 mb-3" />

    <el-timeline>
      <el-timeline-item v-for="comment in ticket.value.comments" class="drop-shadow-xl" :timestamp="new Date(comment.creationDate).toLocaleString()" :key="comment.id" placement="top">
        <el-card class="block">
          <div class="flex justify-between">
            <el-text size="large" tag="b" type="primary">{{ comment.author }}</el-text>
            <el-button size="small" :icon="Delete" @click="deleteComment(comment)" />
          </div>
          <el-text size="default">{{ comment.content }}</el-text>
        </el-card>
      </el-timeline-item>
    </el-timeline>
  </el-card>
</template>

<script lang="ts" setup>
import { Delete } from "@element-plus/icons-vue";
import type ticketComment from "~/types/api/response/ticketCommentResponse";
import type ticket from "~/types/api/response/ticketResponse";

defineProps<{
  ticket: Ref<ticket>;
}>();

const { $api } = useNuxtApp();
const i18n = useI18n();
const route = useRoute();

let newComment = ref("");
let loading = ref(false);

async function sendComment() {
  try {
    if (newComment.value === "") {
      ElNotification({
        title: i18n.t("error"),
        message: i18n.t("commentEmpty"),
        type: "error",
        duration: 5000,
      });
      return;
    }

    loading.value = true;
    let commentPostResult = await $api.ticket.addComment(route.params.id as string, { content: newComment.value, author: "someone" });

    if (commentPostResult.error.value) {
      loading.value = false;
      if (commentPostResult.error.value.statusCode === 404) {
        throw new Error(i18n.t("resourceNotFound"));
      }
      if (commentPostResult.error.value.statusCode === 500) {
        throw new Error("serverError");
      }
      if (commentPostResult.error.value.message) {
        throw new Error(commentPostResult.error.value.message);
      }
      if (commentPostResult.error.value.data) {
        throw new Error(commentPostResult.error.value.data);
      } else {
        throw new Error(i18n.t("error"));
      }
    }

    ticket.value.comments = (await $api.ticket.getComments(route.params.id as string)).data.value?.sort(sortCommentsByDate) ?? [];
    newComment.value = "";
    ElNotification({
      title: i18n.t("success"),
      message: i18n.t("commentPostSuccess"),
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

async function deleteComment(comment: ticketComment) {
  try {
    loading.value = true;
    let commentDeleteResult = await $api.comment.delete(comment.id);

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

    ticket.value.comments = (await $api.ticket.getComments(route.params.id as string)).data.value?.sort(sortCommentsByDate) ?? [];
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

function sortCommentsByDate(a: ticketComment, b: ticketComment) {
  return new Date(b.creationDate).getTime() - new Date(a.creationDate).getTime();
}
</script>

<style></style>

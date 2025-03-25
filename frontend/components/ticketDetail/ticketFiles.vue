<template>
  <el-card
    :v-loading="loading"
    class="drop-shadow-xl base-bg-light dark:base-bg-dark lg:min-h-[13.9rem]">
    <el-text class="text-xl">{{ $t('files') }}</el-text>
    <el-upload
      v-model:file-list="ticket.attachments"
      list-type="picture-card"
      :action="getUploadUrl()"
      :headers="getUploadRequestHeaders()"
      name="attachmentFile"
      :on-error="onUploadError"
      :on-success="onUploadSuccess">
      <template #file="{ file }">
        <UnLazyImage
          v-if="file.blurHash"
          class="object-cover aspect-square w-full"
          :src="$api.attachment.generateAttachmentURL(file.id)"
          :blurhash="file.blurHash"
          :lazy-load="true" />

        <div
          v-else
          class="justify-center align-center text-center w-full h-full flex flex-col">
          <FileIcons
            class="self-center"
            :width="30"
            :height="30"
            :name="file.name" />
          <p>{{ file.name }}</p>
        </div>

        <div>
          <span class="el-upload-list__item-actions">
            <span
              v-if="file.blurHash"
              class="el-upload-list__item-preview"
              @click="openPreview(file)">
              <el-icon><zoom-in /></el-icon>
            </span>
            <span
              class="el-upload-list__item-delete"
              @click="downloadFile(file)">
              <el-icon>
                <Download />
              </el-icon>
            </span>
            <span
              class="el-upload-list__item-delete"
              @click="deleteFile(file)">
              <el-icon>
                <Delete />
              </el-icon>
            </span>
          </span>
        </div>
      </template>
      <el-icon>
        <Upload />
      </el-icon>
    </el-upload>
  </el-card>
</template>

<script lang="ts" setup>
  import type ticket from '~/types/api/response/ticketResponse';
  import { Upload, ZoomIn, Download, Delete } from '@element-plus/icons-vue';
  import FileIcons from 'file-icons-vue';
  import type ticketAttachment from '~/types/api/response/ticketAttachmentResponse';
  import type { UploadFile } from 'element-plus';

  const props = defineProps<{
    ticket: ticket;
  }>();

  const { $api } = useNuxtApp();
  const i18n = useI18n();

  const { setImagePreviewDisplay, setImagePreviewSrc } = useImagePreviewDisplayStore();

  const {
    public: { BACKEND_URL, ACCESS_TOKEN_NAME },
  } = useRuntimeConfig();

  function getUploadRequestHeaders(): Record<string, any> {
    const token = useCookie(ACCESS_TOKEN_NAME, {
      secure: true,
      sameSite: 'strict',
    }).value;

    return {
      Authorization: `Bearer ${token}`,
    };
  }

  async function onUploadSuccess(response: any, uploadFile: UploadFile) {
    props.ticket.attachments.pop();
    props.ticket.attachments = [...props.ticket.attachments, response];
  }

  function onUploadError(error: Error, file: UploadFile) {
    ElNotification({
      title: `${i18n.t('attachmentUploadError')}: ${file.name}`,
      message: error.message,
      type: 'error',
    });
  }

  function getUploadUrl(): string {
    return `${BACKEND_URL}/api/Ticket/${props.ticket.id}/attachment`;
  }

  let loading = ref(false);

  function openPreview(file: ticketAttachment) {
    setImagePreviewSrc($api.attachment.generateAttachmentURL(file.id));
    setImagePreviewDisplay(true);
  }

  function downloadFile(file: ticketAttachment) {
    const link = document.createElement('a');
    link.href = $api.attachment.generateAttachmentURL(file.id);
    link.setAttribute('download', file.name);
    link.style.display = 'none';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  async function deleteFile(file: ticketAttachment) {
    loading.value = true;
    try {
      var attachmentDeleteResult = await $api.attachment.delete(file.id);

      if (attachmentDeleteResult.error.value) {
        if (attachmentDeleteResult.error.value.statusCode === 403) {
          throw new Error(i18n.t('forbidden'));
        }
        if (attachmentDeleteResult.error.value.statusCode === 500) {
          throw new Error('serverError');
        }
        if (attachmentDeleteResult.error.value.message) {
          throw new Error(attachmentDeleteResult.error.value.message);
        }
        if (attachmentDeleteResult.error.value.data) {
          throw new Error(attachmentDeleteResult.error.value.data);
        } else {
          throw new Error(i18n.t('error'));
        }
      }

      props.ticket.attachments = props.ticket.attachments.filter(attachment => attachment.id !== file.id);
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

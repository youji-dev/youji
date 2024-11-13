<template>
  <el-card class="drop-shadow-xl base-bg-light dark:base-bg-dark lg:min-h-[13.9rem]">
    <el-text class="text-xl">{{ $t("files") }}</el-text>
    <el-upload v-model:file-list="ticket.attachments" list-type="picture-card">
      <template #file="{ file }">
        <div>
          <img class="object-cover aspect-square w-full" :src="file.binary" />
          <span class="el-upload-list__item-actions">
            <span class="el-upload-list__item-preview" @click="">
              <el-icon><zoom-in /></el-icon>
            </span>
            <span class="el-upload-list__item-delete" @click="">
              <el-icon>
                <Download />
              </el-icon>
            </span>
            <span class="el-upload-list__item-delete" @click="">
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
import type ticket from "~/types/api/response/ticketResponse";
import { Upload, ZoomIn, Download, Delete } from "@element-plus/icons-vue";

defineProps<{
  ticket: ticket;
}>();
</script>

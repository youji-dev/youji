<template>
  <div class="mt-20 md:mt-5 max-h-[92vh] overflow-y-clip" :style="{ width: width }">
    <div v-loading="loading" v-if="!is404 && ticketModel" class="px-5 pb-3">
      <TicketHeader :ticket="ticketModel" class="lg:col-span-full"></TicketHeader>
    </div>
    <div class="overflow-y-auto px-5 max-h-[75vh] md:max-h-[82vh]" :style="{ width: width }">
      <div v-loading="loading" v-if="!is404 && ticketModel" class="grid grid-cols-1 gap-3 auto-rows-min lg:grid-cols-[7fr_4fr]" :element-loading-text="loadingText">
        <div class="lg:col-start-1 lg:col-end-3 lg:row-start-2 lg:row-end-3">
          <el-text>{{ $t("title") }}</el-text>
          <el-input v-model="ticketModel.title" :placeholder="$t('enter')" class="drop-shadow-md dark:base-bg-dark" />
        </div>

        <DropdownGroup :ticket="ticketModel" class="lg:col-start-2 lg:col-end-3 lg:row-start-3 lg:row-end-4 self-start" />

        <div class="lg:col-start-1 lg:col-end-2 lg:row-start-3 lg:row-end-4">
          <el-text>{{ $t("description") }}</el-text>
          <el-input v-model="ticketModel.description" type="textarea" class="drop-shadow-md max-h-full dark:base-bg-dark" :rows="15" resize="vertical" :placeholder="$t('enter')" />
        </div>

        <div v-if="!isNew" class="flex justify-around self-start lg:block lg:text-right lg:col-start-2 lg:col-end-3 lg:row-start-5 lg:row-end-6">
          <el-text class="w-1/2 truncate text-center">{{ $t("createdBy") }}: {{ ticketModel.author }}</el-text>
          <br />
          <el-text class="w-1/2 text-center">{{ $t("createdOn") }}: {{ new Date(ticketModel.creationDate).toLocaleString() }}</el-text>
        </div>

        <TicketFiles v-if="!isNew" class="lg:col-start-2 lg:col-end-3 lg:row-start-4 lg:row-end-5" :ticket="ticketModel" />

        <TicketCommentCollection v-if="!isNew" class="self-start lg:col-start-1 lg:col-end-2 lg:row-start-4 lg:row-end-6 mb-3" v-model:ticket="ticketModel" />
      </div>
      <div v-if="is404" class="h-screen flex justify-center items-center">
        <el-result class="" icon="error" :title="$t('resourceNotFound')">
          <template #extra>
            <el-button @click="router.back()" type="primary">{{ $t("back") }}</el-button>
          </template>
        </el-result>
      </div>
      <div v-else-if="ticketModel == null">
        <TicketDetailTicketLoadingSkeleton />
      </div>
    </div>
    <div class="flex justify-between px-5 py-3 lg:col-span-full lg:row-start-6 lg:row-end-7">
      <el-tooltip :disabled="!isNew" :content="$t('pdfExportNotOnUnsaved')" placement="top-start">
        <el-button :disabled="isNew" class="text-sm drop-shadow-md" type="default" :icon="Printer" @click="exportToPDF()">{{ $t("pdfExport") }}</el-button>
      </el-tooltip>

      <div class="flex">
        <el-button class="text-sm justify-self-end drop-shadow-md" type="primary" @click="isNew ? createTicket() : saveTicketChanges()">{{ $t("save") }}</el-button>

        <el-button class="text-sm justify-self-end drop-shadow-md" type="default" @click="router.back()">{{ $t("close") }}</el-button>
      </div>
    </div>
  </div>
  <el-dialog v-model="imagePreviewDisplay">
    <img w-full :src="imagePreviewSrc" alt="Preview Image" class="w-full" />
  </el-dialog>
</template>

<script lang="ts" setup async>
import { Printer } from "@element-plus/icons-vue";
import type building from "~/types/api/response/buildingResponse";
import type priority from "~/types/api/response/priorityResponse";
import type state from "~/types/api/response/stateResponse";
import type ticket from "~/types/api/response/ticketResponse";
import type ticketAttachment from "~/types/api/response/ticketAttachmentResponse";
import type ticketComment from "~/types/api/response/ticketCommentResponse";

import { fromTicketResponse as createTicketFromResponse } from "~/types/api/request/createTicket";
import { fromTicketResponse as editTicketFromResponse } from "~/types/api/request/editTicket";
import TicketNotFoundError from "~/types/error/ticketNotFound";

import DropdownGroup from "~/components/ticketDetail/ticketDropdown.vue";
import TicketCommentCollection from "~/components/ticketDetail/ticketCommentCollection.vue";
import TicketFiles from "~/components/ticketDetail/ticketFiles.vue";
import TicketHeader from "~/components/ticketDetail/ticketHeader.vue";
const { $api } = useNuxtApp();
const i18n = useI18n();
const localePath = useLocaleRoute();

const { imagePreviewDisplay, imagePreviewSrc } = storeToRefs(useImagePreviewDisplayStore());

const router = useRouter();
const route = useRoute();

const width = ref("100vw");
let isNew = ref((route.params.id as string).toLocaleLowerCase() == "new");
let is404 = ref(false);
let loading = ref(true);
let loadingText = ref(i18n.t("loadingData"));
let availableStates: Ref<state[]> = ref([] as state[]);
let availablePriorities: Ref<priority[]> = ref([] as priority[]);
let availableBuildings: Ref<building[]> = ref([] as building[]);
let ticketModel: Ref<ticket | null> = ref(null);

onNuxtReady(async () => {
  await Promise.all([$api.state.getAll(), $api.priority.getAll(), $api.building.getAll(), fetchOrInitializeTicket(route.params.id as string)])
    .then(([states, priorities, buildings, ticketData]) => {
      availableStates.value = states.data.value ?? [];
      availablePriorities.value = priorities.data.value ?? [];
      availableBuildings.value = buildings.data.value ?? [];
      ticketModel.value = ticketData;
      is404.value = false;
    })
    .catch(error => {
      if (error instanceof TicketNotFoundError) {
        is404.value = true;
      } else {
        ElNotification({
          title: i18n.t("error"),
          message: (error as Error).message,
          type: "error",
          duration: 5000,
        });
      }
    })
    .finally(() => (loading.value = false));

  determineViewWidth();
  window.addEventListener("resize", determineViewWidth);
});

async function fetchOrInitializeTicket(id: string): Promise<ticket> {
  if (id === "new") {
    const newTicket = {
      title: "",
      description: "",
      author: "",
      attachments: [] as ticketAttachment[],
      comments: [] as ticketComment[],
    } as ticket;

    return newTicket;
  }

  const ticketResult = await $api.ticket.get(id);

  if (ticketResult.error.value) {
    console.log(ticketResult.error);
    if (ticketResult.error.value.statusCode === 404) {
      throw new TicketNotFoundError("Ticket not found");
    }
    if (ticketResult.error.value.statusCode === 500) {
      throw new Error("serverError");
    }
    if (ticketResult.error.value.message) {
      throw new Error(ticketResult.error.value.message);
    }
    if (ticketResult.error.value.data) {
      throw new Error(ticketResult.error.value.data);
    } else {
      throw new Error(i18n.t("error"));
    }
  }
  const ticketData = ticketResult.data.value;

  if (ticketData === null) {
    return await fetchOrInitializeTicket("new");
  }
  return ticketData;
}

async function saveTicketChanges() {
  try {
    loadingText.value = i18n.t("savingTicket");
    loading.value = true;
    const ticketResult = await $api.ticket.edit(editTicketFromResponse(ticketModel.value!));

    if (ticketResult.error.value) {
      if (ticketResult.error.value.statusCode === 404) {
        throw new Error(i18n.t("resourceNotFound"));
      }
      if (ticketResult.error.value.statusCode === 403) {
        throw new Error(i18n.t("forbidden"));
      }
      if (ticketResult.error.value.statusCode === 500) {
        throw new Error("serverError");
      }
      if (ticketResult.error.value.message) {
        throw new Error(ticketResult.error.value.message);
      }
      if (ticketResult.error.value.data) {
        throw new Error(ticketResult.error.value.data);
      } else {
        throw new Error(i18n.t("error"));
      }
    }

    if (ticketResult.data.value) {
      ticketModel.value = ticketResult.data.value;
    }
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

async function createTicket() {
  try {
    loadingText.value = i18n.t("creatingTicket");
    loading.value = true;
    const ticketResult = await $api.ticket.create(createTicketFromResponse(ticketModel.value!));

    if (ticketResult.error.value) {
      if (ticketResult.error.value.statusCode === 403) {
        throw new Error(i18n.t("forbidden"));
      }
      if (ticketResult.error.value.statusCode === 500) {
        throw new Error("serverError");
      }
      if (ticketResult.error.value.message) {
        throw new Error(ticketResult.error.value.message);
      }
      if (ticketResult.error.value.data) {
        throw new Error(ticketResult.error.value.data);
      } else {
        throw new Error(i18n.t("error"));
      }
    }

    if (ticketResult.data.value) {
      router.push(localePath("/tickets/" + ticketResult.data.value.id)?.fullPath as string);
    }
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

async function exportToPDF() {
  loadingText.value = i18n.t("creatingPDFExport");
  loading.value = true;
  await $api.ticket.exportToPDF(route.params.id as string, i18n.locale.value);
  loading.value = false;
}

function determineViewWidth() {
  if (typeof document === "undefined") return;
  const navbar = document.getElementById("navbar");
  if (typeof navbar === "undefined") return;
  if (typeof navbar?.offsetWidth === "undefined") return;
  width.value = window.innerWidth - navbar?.offsetWidth + "px";
  const table = document.querySelector("el-table__inner-wrapper");
  if (!!!table) return;

  return;
}
</script>

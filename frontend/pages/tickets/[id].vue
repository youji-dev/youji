<template>
  <!-- Route: /tickets/[ticketId] -->
  <!-- Page for detail view of a ticket -->
  <div class="py-20 mt-17 mx-5 lg:py-5 lg:mt-0 lg-mx-3 grid grid-cols-1 gap-3 auto-rows-min lg:grid-cols-10" :style="{ width: width }" v-loading="loading" :element-loading-text="loadingText">
    <TicketHeader :ticket="ticket" class="lg:col-span-full lg:row-start-1 lg:row-end-2"></TicketHeader>

    <!-- Title -->
    <div class="lg:col-start-1 lg:col-end-7 lg:row-start-2 lg:row-end-3">
      <el-text>{{ $t("title") }}</el-text>
      <el-input v-model="ticket.title" :placeholder="$t('enter')" class="drop-shadow-xl" />
    </div>

    <DropdownGroup :ticket="ticket" class="lg:col-start-7 lg:col-end-11 lg:row-start-2 lg:row-end-4" />

    <!-- Description -->
    <div class="lg:col-start-1 lg:col-end-7 lg:row-start-3 lg:row-end-4">
      <el-text>{{ $t("description") }}</el-text>
      <el-input v-model="ticket.description" type="textarea" class="drop-shadow-xl max-h-full" :rows="15" resize="vertical" :placeholder="$t('enter')" />
    </div>

    <!-- meta data -->
    <div v-if="!newTicket" class="flex justify-around self-start lg:block lg:text-right lg:col-start-7 lg:col-end-11 lg:row-start-5 lg:row-end-6">
      <el-text class="w-1/2 truncate text-center">{{ $t("createdBy") }}: {{ ticket.author }}</el-text>
      <br />
      <el-text class="w-1/2 text-center">{{ $t("createdOn") }}: {{ new Date(ticket.creationDate).toLocaleString() }}</el-text>
    </div>

    <TicketFiles v-if="!newTicket" class="lg:col-start-7 lg:col-end-11 lg:row-start-4 lg:row-end-5" :ticket="ticket" />

    <TicketCommentCollection v-if="!newTicket" class="self-start lg:col-start-1 lg:col-end-7 lg:row-start-4 lg:row-end-6" :ticket="ticket" />

    <!-- buttons -->
    <div class="flex justify-between lg:col-span-full lg:row-start-6 lg:row-end-7">
      <el-tooltip :disabled="!newTicket" :content="$t('pdfExportNotOnUnsaved')" placement="top-start">
        <el-button :disabled="newTicket" class="text-sm drop-shadow-xl" type="default" :icon="Printer" @click="exportToPDF()">{{ $t("pdfExport") }}</el-button>
      </el-tooltip>

      <div class="flex">
        <el-button class="text-sm justify-self-end drop-shadow-xl" type="primary" @click="newTicket ? createTicket() : updateTicket()">{{ $t("save") }}</el-button>

        <el-button class="text-sm justify-self-end drop-shadow-xl" type="default" @click="router.back()">{{ $t("close") }}</el-button>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup async>
const { $api } = useNuxtApp();
const i18n = useI18n();
const localePath = useLocaleRoute();

const router = useRouter();
const route = useRoute();

const width = ref("100vw");
let newComment = ref("");
let newTicket = ref((route.params.id as string).toLocaleLowerCase() == "new");
let loading = ref(true);
let loadingText = ref(i18n.t("loadingData"));
let availableStates: Ref<state[]> = ref([] as state[]);
let availablePriorities: Ref<priority[]> = ref([] as priority[]);
let availableBuildings: Ref<building[]> = ref([] as building[]);
let ticket: Ref<ticket> = ref({} as ticket);

onNuxtReady(async () => {
  try {
    const [states, priorities, buildings, ticketData] = await Promise.all([$api.state.getAll(), $api.priority.getAll(), $api.building.getAll(), fetchOrCreateTicket(route.params.id as string)]);

    availableStates.value = states.data.value ?? [];
    availablePriorities.value = priorities.data.value ?? [];
    availableBuildings.value = buildings.data.value ?? [];
    ticket.value = ticketData;

    loading.value = false;
  } catch (error) {
    loading.value = false;
    ElNotification({
      title: i18n.t("error"),
      message: (error as Error).message,
      type: "error",
      duration: 5000,
    });
  }
});

async function fetchOrCreateTicket(id: string): Promise<ticket> {
  if (id === "new") {
    const newTicket = {
      title: "",
      description: "",
      author: "",
    } as ticket;

    return newTicket;
  }

  const ticketResult = await $api.ticket.get(id);

  if (ticketResult.error.value) {
    if (ticketResult.error.value.statusCode === 404) {
      throw new Error(i18n.t("resourceNotFound"));
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
    return await fetchOrCreateTicket("new");
  }

  ticketData.comments = ticketData.comments.sort(sortCommentsByDate);
  return ticketData;
}

async function updateTicket() {
  try {
    loading.value = true;
    const ticketResult = await $api.ticket.edit(fromTicketResponse(ticket.value));

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
      ticket.value = ticketResult.data.value;
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
    loading.value = true;
    const ticketResult = await $api.ticket.create({
      title: ticket.value.title,
      description: ticket.value.description ?? null,
      author: ticket.value.author,
      stateId: ticket.value.state.id,
      priorityValue: ticket.value.priority.value,
      buildingId: ticket.value.building?.id ?? null,
      object: ticket.value.object ?? null,
      room: ticket.value.room ?? null,
    });

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
  loading.value = true;
  await $api.ticket.exportToPDF(route.params.id as string, i18n.locale.value);
  loading.value = false;
}
</script>

<script lang="ts">
import { Upload, ZoomIn, Download, Delete, Printer } from "@element-plus/icons-vue";
import { contrastColor } from "contrast-color";
import type priority from "~/types/api/response/priorityResponse";
import type building from "~/types/api/response/buildingResponse";
import type ticket from "~/types/api/response/ticketResponse";
import type state from "~/types/api/response/stateResponse";
import type ticketComment from "~/types/api/response/ticketCommentResponse";
import { fromTicketResponse } from "~/types/api/request/editTicket";
import TicketHeader from "~/components/ticketDetail/ticketHeader.vue";
import DropdownGroup from "~/components/ticketDetail/ticketDropdown.vue";
import TicketFiles from "~/components/ticketDetail/ticketFiles.vue";

const width = ref("100vw");
onNuxtReady(() => {
  determineViewWidth();
  window.addEventListener("resize", determineViewWidth);
});

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

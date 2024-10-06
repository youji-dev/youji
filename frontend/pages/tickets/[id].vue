<template>
  <!-- Route: /tickets/[ticketId] -->
  <!-- Page for detail view of a ticket -->
  <div class="py-20 mt-17 mx-5 lg:py-5 lg:mt-0 lg-mx-3 grid grid-cols-1 gap-3 auto-rows-min lg:grid-cols-10" :style="{ width: width }">
    <!-- header -->
    <div class="flex justify-between lg:col-span-full lg:row-start-1 lg:row-end-2">
      <div>
        <!-- backbutton -->
        <el-button class="text-sm" link @click="router.back()" :icon="ArrowLeft">{{ $t("back") }}</el-button>
        <!-- separator -->
        <el-divider class="self-center" direction="vertical" />
        <!-- Title -->
        <el-text class="font-semibold truncate" size="large">{{ ticket.title }}</el-text>
      </div>
      <div>
        <!-- Edit Button -->
        <el-button class="text-sm drop-shadow-xl" type="primary" :icon="EditPen">{{ $t("edit") }}</el-button>
      </div>
    </div>
    <!-- Dropdown Group -->
    <div class="grid lg:grid-cols-2 lg:grid-rows-4 gap-1 self-center | lg:col-start-7 lg:col-end-11 lg:row-start-2 lg:row-end-3">
      <!-- State dropdown -->
      <div>
        <el-text>{{ $t("state") }}</el-text>
        <el-select v-model="ticket.state" value-key="id" class="drop-shadow-xl" :placeholder="$t('select')">
          <el-option v-for="state in availableStates" :key="state.id" :label="state.name" :value="state">
            <el-tag :color="state.color">
              <el-text :style="{ color: contrastColor({ bgColor: state.color }) }"> {{ state.name }}</el-text>
            </el-tag>
          </el-option>
          <template #label>
            <div class="flex items-center">
              <el-tag :color="ticket.state.color" size="small" class="mr-2 aspect-square" />
              <span class="truncate">{{ ticket.state.name }}</span>
            </div>
          </template>
        </el-select>
      </div>
      <!-- Priority dropdown -->
      <div>
        <el-text>{{ $t("priority") }}</el-text>
        <el-select v-model="ticket.priority" value-key="value" class="drop-shadow-xl" :placeholder="$t('select')">
          <el-option v-for="priority in availablePriorities" :key="priority.value" :label="priority.name" :value="priority" />
        </el-select>
      </div>
      <!-- Building dropdown -->
      <div class="lg:col-span-full">
        <el-text>{{ $t("building") }}</el-text>
        <el-select v-model="ticket.building" class="drop-shadow-xl" value-key="id" :clearable="true" :placeholder="$t('select')">
          <el-option v-for="building in availableBuildings" :key="building.id" :label="building.name" :value="building" />
        </el-select>
      </div>
      <!-- Room textfield -->
      <div class="lg:col-span-full">
        <el-text>{{ $t("room") }}</el-text>
        <el-input v-model="ticket.room" class="drop-shadow-xl" :placeholder="$t('enter')" />
      </div>
      <!-- Object -->
      <div class="lg:col-span-full">
        <el-text>{{ $t("object") }}</el-text>
        <el-input v-model="ticket.object" class="drop-shadow-xl" :placeholder="$t('enter')" />
      </div>
    </div>

    <!-- Description -->
    <div class="lg:col-start-1 lg:col-end-7 lg:row-start-2 lg:row-end-3">
      <el-text>{{ $t("description") }}</el-text>
      <el-input v-model="ticket.description" type="textarea" class="drop-shadow-xl max-h-full" :rows="15" resize="vertical" :placeholder="$t('enter')" />
    </div>

    <!-- meta data -->
    <div class="flex justify-around self-start lg:block lg:text-right lg:col-start-7 lg:col-end-11 lg:row-start-4 lg:row-end-5">
      <el-text class="w-1/2 truncate text-center">{{ $t("createdBy") }}: {{ ticket.author }}</el-text>
      <br />
      <el-text class="w-1/2 text-center">{{ $t("createdOn") }}: {{ new Date(ticket.creationDate).toLocaleString() }}</el-text>
    </div>

    <!-- files -->
    <el-card class="drop-shadow-xl base-bg-light dark:bg-black lg:col-start-7 lg:col-end-11 lg:row-start-3 lg:row-end-4">
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

    <!-- comments -->
    <el-card class="drop-shadow-xl base-bg-light dark:bg-black self-start lg:col-start-1 lg:col-end-7 lg:row-start-3 lg:row-end-5">
      <el-input v-model="newComment" type="textarea" resize="vertical" :rows="3" :placeholder="$t('newComment')" />
      <el-button class="mt-2 float-end" type="primary" size="small">{{ $t("sendComment") }}</el-button>

      <el-divider class="mt-10 mb-3" />

      <el-timeline>
        <el-timeline-item v-for="comment in ticket.comments" class="drop-shadow-xl" :timestamp="new Date(comment.creationDate).toLocaleString()" :key="comment.id" placement="top">
          <el-card class="block">
            <el-text size="large" tag="b" type="primary">{{ comment.author }}</el-text>
            <br />
            <el-text size="default">{{ comment.content }}</el-text>
          </el-card>
        </el-timeline-item>
      </el-timeline>
    </el-card>

    <!-- buttons -->
    <div class="flex justify-between lg:col-span-full lg:row-start-5 lg:row-end-6">
      <el-button class="text-sm drop-shadow-xl" type="default" :icon="Printer">{{ $t("pdfExport") }}</el-button>

      <div class="flex">
        <el-button class="text-sm justify-self-end drop-shadow-xl" type="primary">{{ $t("save") }}</el-button>

        <el-button class="text-sm justify-self-end drop-shadow-xl" type="default">{{ $t("close") }}</el-button>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup async>
import type { UploadProps, UploadUserFile } from "element-plus";
const { $api } = useNuxtApp();
const i18n = useI18n();

const router = useRouter();
const route = useRoute();

const width = ref("100vw");
let newComment = ref("");
let newTicket = ref((route.params.id as string).toLocaleLowerCase() == "new");
let loading = ref(true);

let availableStates: Ref<state[]> = ref([] as state[]);
let availablePriorities: Ref<priority[]> = ref([] as priority[]);
let availableBuildings: Ref<building[]> = ref([] as building[]);
let ticket: Ref<ticket> = ref({} as ticket);

onNuxtReady(async () => {
  const states = (await $api.state.getAll()).data.value ?? [];
  const priorities = (await $api.priority.getAll()).data.value ?? [];
  const buildings = (await $api.building.getAll()).data.value ?? [];

  availableStates.value = states;
  availablePriorities.value = priorities;
  availableBuildings.value = buildings;
  ticket.value = await fetchOrCreateTicket(route.params.id as string);
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
    ElNotification({
      title: i18n.t("error"),
      message: ticketResult.error.value?.message ?? JSON.stringify(ticketResult.error.value),
      type: "error",
      duration: 5000,
    });

    throw new Error(ticketResult.error.value?.message);
  }

  const ticketData = ticketResult.data.value;

  if (ticketData === null) {
    return await fetchOrCreateTicket("new");
  }

  return ticketData;
}
</script>

<script lang="ts">
import { ArrowLeft, EditPen, Upload, ZoomIn, Download, Delete, Printer } from "@element-plus/icons-vue";
import { contrastColor } from "contrast-color";
import type priority from "~/types/api/response/priorityResponse";
import type building from "~/types/api/response/buildingResponse";
import type ticket from "~/types/api/response/ticketResponse";
import type state from "~/types/api/response/stateResponse";

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

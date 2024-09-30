<template>
  <!-- Route: /tickets/[ticketId] -->
  <!-- Page for detail view of a ticket -->
  <div class="h-[100vh] p-6 mt-8 lg:mt-0" style="height: calc(100vh - 72px)" :style="{ width: width }">
    <!-- header -->
    <div class="flex my-2">
      <!-- backbutton -->
      <el-button class="text-sm flex-none" link @click="router.back()" :icon="ArrowLeft">{{ $t("back") }}</el-button>
      <!-- separator -->
      <el-divider class="flex-none self-center" direction="vertical" />
      <!-- Title -->
      <el-text class="font-semibold flex-auto truncate" size="large">TicketName</el-text>
      <!-- Edit Button -->
      <el-button class="text-sm flex-none justify-self-end" type="primary" :icon="EditPen" @click="toggleEditMode">{{
        $t("edit") }}</el-button>
    </div>
    <!-- Dropdown Group -->
    <div class="grid gap-3 grid-cols-2 grid-rows-2">
      <!-- State dropdown -->
      <div>
        <el-text>{{ $t("state") }}</el-text>
        <el-select v-model="form.state" value-key="id" :placeholder="stateOptions[0].name">
          <el-option v-for="state in stateOptions" :key="state.id" :label="state.name" :value="state">
            <div class="flex items-center">
              <el-tag :color="state.color" size="small" class="mr-2 aspect-square" />
              <span :style="{ color: state.color }">{{ state.name }}</span>
            </div>
          </el-option>
        </el-select>
      </div>
      <!-- Priority dropdown -->
      <div>
        <el-text>{{ $t("priority") }}</el-text>
        <el-select v-model="form.priority" value-key="value" :placeholder="priorityOptions[0].name">
          <el-option v-for="priority in priorityOptions" :key="priority.value" :label="priority.name"
            :value="priority" />
        </el-select>
      </div>
      <!-- Room textfield -->
      <div>
        <el-text>{{ $t("room") }}</el-text>
        <el-input v-model="form.room" placeholder="Raum 200" />
      </div>
      <!-- Building dropdown -->
      <div>
        <el-text>{{ $t("building") }}</el-text>
        <el-select v-model="form.building" value-key="id" :placeholder="buildingOptions[0].name">
          <el-option v-for="building in buildingOptions" :key="building.id" :label="building.name" :value="building" />
        </el-select>
      </div>
    </div>

    <!-- Description -->
    <div class="mt-6">
      <el-text>{{ $t("description") }}</el-text>
      <el-input v-model="form.description" type="textarea" resize="none" :rows="10" />
    </div>

    <!-- meta data -->
    <div class="flex mt-2 justify-around	">
      <el-text class="w-1/2 truncate text-center">{{ $t("createdBy") }}: {{ form.author }}</el-text>
      <el-text class="w-1/2 text-center	">{{ $t("createdOn") }}: {{ form.createdAt }}</el-text>
    </div>

    <!-- files -->
    <div class="mt-2">
      <!-- <el-upload
    v-model:file-list="fileList"
    action="https://run.mocky.io/v3/9d059bf9-4660-45f2-925d-ce80ad6c4d15"
    list-type="picture-card"
    :on-preview="handlePictureCardPreview"
    :on-remove="handleRemove"
  >
    <el-icon><Plus /></el-icon>
  </el-upload> -->
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ArrowLeft, EditPen } from "@element-plus/icons-vue";
import type state from "~/types/state";
import type priority from "~/types/priority";
import type building from "~/types/building";
const router = useRouter();

let form = ref({
  title: "TicketName" as string,
  state: null as state | null,
  priority: null as priority | null,
  room: "" as string,
  building: null as building | null,
  createdAt: "21.05.2024" as string | null,
  author: "dmeyer" as string,
  description: "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet." as string,
});
</script>

<script lang="ts">
const editMode = ref(false);
const width = ref("100vw");
onNuxtReady(() => {
  determineViewWidth();
  window.addEventListener("resize", determineViewWidth);
});
function toggleEditMode() {
  editMode.value = !editMode.value;
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

const stateOptions: state[] = [
  {
    id: "1",
    name: "new",
    color: "#ff293b",
  },
  {
    id: "2",
    name: "open",
    color: "#f8ff29",
  },
  {
    id: "3",
    name: "closed",
    color: "#74ff29",
  },
  {
    id: "4",
    name: "Emilio ist doof",
    color: "#6734eb",
  },
];

const priorityOptions: priority[] = [
  {
    value: 1,
    name: "low",
  },
  {
    value: 2,
    name: "medium",
  },
  {
    value: 3,
    name: "high",
  },
];

const buildingOptions: building[] = [
  {
    id: "1",
    name: "Hauptgebäude",
  },
  {
    id: "2",
    name: "Nebengebäude",
  },
  {
    id: "3",
    name: "Werkhalle",
  },
  {
    id: "4",
    name: "Turnhalle",
  },
];
</script>
<template>
  <!-- Route: /tickets/[ticketId] -->
  <!-- Page for detail view of a ticket -->
  <!-- header -->
  <div class=" h-[100vh] p-6 mt-8 lg:mt-0" style="height: calc(100vh - 72px)" :style="{ width: width }">
    <div class="flex m-2">
      <!-- backbutton -->
      <el-button class="text-sm flex-none" link @click="router.back()" :icon="ArrowLeft">{{ $t("back")
        }}</el-button>
      <!-- seperator -->
      <el-divider class="flex-none self-center" direction="vertical" />
      <!-- Title -->
      <el-text class="font-semibold flex-auto truncate" size="large">TicketName</el-text>
      <!-- Edit Button -->
      <el-button class="text-sm flex-none justify-self-end" type="primary" :icon="EditPen" @click="toggleEditMode">{{
        $t("edit")
        }}</el-button>
    </div>
    <!-- Dropdown Group -->
    <div class="grid-cols-2 grid-rows-2">
      <!-- State dropdown -->
      <div>
        <el-text>{{ $t("state") }}</el-text>
        <el-select :v-model="form.state" :placeholder="stateOptions[0].name">
          <el-option v-for="state in stateOptions" :key="state.id" :label="state.name" :value="!state" />
        </el-select>
      </div>
      <!-- Priority dropdown -->
      <div>
        <el-text>{{ $t("priority") }}</el-text>
        <el-select :v-model="form.priority" :placeholder="priorityOptions[0].name">
          <el-option v-for="priority in priorityOptions" :key="priority.value" :label="priority.name"
            :value="priority.value" />
        </el-select>
      </div>
      <!-- Room textfield -->
      <div>
        <el-text>{{ $t("room") }}</el-text>
        <el-input :v-model="form.room" placeholder="Raum 200" />
      </div>
      <!-- Building dropdown -->
      <div>
        <el-text>{{ $t("building") }}</el-text>
        <el-select :v-model="form.building" :placeholder="buildingOptions[0].name">
          <el-option v-for="building in buildingOptions" :key="building.id" :label="building.name" :value="!building" />
        </el-select>
      </div>
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
  room: null as string | null,
  building: null as building | null,
  description: null as string | null,
});
</script>

<script lang="ts">
const editMode = ref(false);
const width = ref("100vw")
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
    color: "#EF4444",
  },
  {
    id: "2",
    name: "open",
    color: "#EF4444",
  },
  {
    id: "3",
    name: "closed",
    color: "#EF4444",
  }
]

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
  }
]

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
  }
]
</script>

<style></style>
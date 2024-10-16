<template>
  <div class="grid lg:grid-cols-2 lg:grid-rows-4 gap-1 self-center">
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
</template>

<script lang="ts" setup>
import { contrastColor } from "contrast-color";
import type ticket from "~/types/api/response/ticketResponse";
import type state from "~/types/api/response/stateResponse";
import type priority from "~/types/api/response/priorityResponse";
import type building from "~/types/api/response/buildingResponse";
defineProps<{
  ticket: ticket;
}>();

const { $api } = useNuxtApp();
const i18n = useI18n();

let availableStates: Ref<state[]> = ref([]);
let availablePriorities: Ref<priority[]> = ref([]);
let availableBuildings: Ref<building[]> = ref([]);

onNuxtReady(async () => {
  try {
    const [states, priorities, buildings] = await Promise.all([$api.state.getAll(), $api.priority.getAll(), $api.building.getAll()]);

    availableStates.value = states.data.value ?? [];
    availablePriorities.value = priorities.data.value ?? [];
    availableBuildings.value = buildings.data.value ?? [];
  } catch (error) {
    ElNotification({
      title: i18n.t("error"),
      message: (error as Error).message,
      type: "error",
      duration: 5000,
    });
  }
});
</script>

<style></style>

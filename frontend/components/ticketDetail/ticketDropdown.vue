<template>
  <div class="grid lg:grid-cols-2 lg:grid-rows-4 gap-x-1">
    <el-form-item
      :label="$t('state')"
      prop="state">
      <el-tooltip
        :content="$t('stateChangeNotAllowed')"
        placement="top"
        :disabled="canUserChangeState">
        <el-select
          v-model="ticket.state"
          value-key="id"
          class="drop-shadow-xl dark:base-bg-dark"
          :placeholder="$t('select')">
          <el-option
            v-for="availableState in availableStates"
            :key="availableState.id"
            :label="availableState.name"
            :value="availableState"
            :disabled="!canUserChangeState">
            <div class="flex items-center">
              <el-tag
                :color="availableState.color"
                size="small"
                class="mr-2 aspect-square" />
              <span class="truncate">{{ availableState.name }}</span>
            </div>
          </el-option>
          <template #label>
            <div class="flex items-center">
              <el-tag
                :color="ticket.state.color"
                size="small"
                class="mr-2 aspect-square" />
              <span class="truncate">{{ ticket.state.name }}</span>
            </div>
          </template>
        </el-select>
      </el-tooltip>
    </el-form-item>
    <el-form-item
      :label="$t('priority')"
      prop="priority">
      <el-select
        v-model="ticket.priority"
        value-key="id"
        class="drop-shadow-xl dark:base-bg-dark"
        :placeholder="$t('select')">
        <el-option
          v-for="availablePriority in availablePriorities"
          :key="availablePriority.id"
          :label="availablePriority.name"
          :value="availablePriority">
          <div class="flex items-center">
            <el-tag
              :color="availablePriority.color"
              size="small"
              class="mr-2 aspect-square" />
            <span class="truncate">{{ availablePriority.name }}</span>
          </div>
        </el-option>
        <template #label>
          <div class="flex items-center">
            <el-tag
              :color="ticket.priority.color"
              size="small"
              class="mr-2 aspect-square" />
            <span class="truncate">{{ ticket.priority.name }}</span>
          </div>
        </template>
      </el-select>
    </el-form-item>
    <el-form-item
      :label="$t('building')"
      prop="building"
      class="lg:col-span-full">
      <el-select
        v-model="ticket.building"
        class="drop-shadow-xl dark:base-bg-dark"
        value-key="id"
        :clearable="true"
        :placeholder="$t('select')">
        <el-option
          v-for="availableBuilding in availableBuildings"
          :key="availableBuilding.id"
          :label="availableBuilding.name"
          :value="availableBuilding" />
      </el-select>
    </el-form-item>
    <el-form-item
      :label="$t('room')"
      prop="room"
      class="lg:col-span-full">
      <el-input
        v-model="ticket.room"
        class="drop-shadow-xl dark:base-bg-dark"
        :placeholder="$t('enter')" />
    </el-form-item>
    <el-form-item
      :label="$t('object')"
      prop="object"
      class="lg:col-span-full">
      <el-input
        v-model="ticket.object"
        class="drop-shadow-xl dark:base-bg-dark"
        :placeholder="$t('enter')" />
    </el-form-item>
  </div>
</template>

<script lang="ts" setup>
  import type ticketType from '~/types/api/response/ticketResponse';
  import type state from '~/types/api/response/stateResponse';
  import type priority from '~/types/api/response/priorityResponse';
  import type building from '~/types/api/response/buildingResponse';

  const _props = defineProps<{
    ticket: ticketType;
  }>();

  const { isUserAdmin, isUserFacilityManager } = storeToRefs(useAuthStore());
  const { $api } = useNuxtApp();
  const i18n = useI18n();

  const availableStates: Ref<state[]> = ref([]);
  const availablePriorities: Ref<priority[]> = ref([]);
  const availableBuildings: Ref<building[]> = ref([]);
  const doesDefaultStateExist: Ref<boolean> = ref(false);
  const canUserChangeState: Ref<boolean> = computed(() => {
    return isUserAdmin.value || isUserFacilityManager.value || !doesDefaultStateExist.value;
  });

  onNuxtReady(async () => {
    try {
      const statesPromise = $api.state.getAll();
      const prioritiesPromise = $api.priority.getAll();
      const buildingsPromise = $api.building.getAll();

      const [states, prioritiesResponse, buildings] = await Promise.all([
        statesPromise,
        prioritiesPromise,
        buildingsPromise,
      ]);

      const sortedPriorities = prioritiesResponse.data.value?.sort((a, b) => b.value - a.value);

      availableStates.value = states.data.value ?? [];
      availablePriorities.value = sortedPriorities ?? [];
      availableBuildings.value = buildings.data.value ?? [];
      doesDefaultStateExist.value = availableStates.value.some(state => state.isDefault);
    } catch (error) {
      ElNotification({
        title: i18n.t('error'),
        message: (error as Error).message,
        type: 'error',
        duration: 5000,
      });
    }
  });
</script>

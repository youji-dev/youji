<template>
  <div
    v-if="!initialLoading"
    class="flex flex-col xl:grid xl:grid-cols-2 xl:gap-4">
    <div id="priorities">
      <h1 class="text-sm">{{ $t('managePriorities') }}</h1>
      <el-table
        v-loading="prioritiesLoading"
        :data="priorities"
        border
        class="my-3 lg:my-1"
        max-height="200"
        style="width: 100%"
        :default-sort="{ prop: 'value[value]', order: 'descending' }"
        @cell-dblclick="(row, column) => handleCellClick(row, column, priorities)">
        <el-table-column
          prop="value[value]"
          :label="$t('priority')">
          <template #default="{ row }">
            <div class="flex items-center justify-around">
              <span
                v-if="!row.new"
                class="caret-wrapper"
                ><i
                  class="sort-caret ascending"
                  @click="increasePriorityValue(row.id.value)" /><i
                  class="sort-caret descending"
                  @click="decreasePriorityValue(row.id.value)"
              /></span>
              <h1 class="text-sm px-3">{{ row.value.value }}</h1>
            </div>
          </template>
        </el-table-column>
        <TableEditableColumn
          prop="name"
          :label="$t('name')"
          :save-callback="updatePriorities" />
        <el-table-column :label="$t('color')">
          <template #default="{ row }">
            <div class="w-full flex justify-center items-center">
              <el-color-picker
                v-model="row.color.value"
                @change="!row.new && updatePriorities(row, 'U')" />
            </div>
          </template>
        </el-table-column>
        <el-table-column :label="''">
          <template #default="{ row }">
            <el-popconfirm
              v-if="!row.new"
              :title="$t('confirmDelete')"
              :cancel-button-text="$t('no')"
              :confirm-button-text="$t('yes')"
              @confirm="updatePriorities(row, 'D')">
              <template #reference>
                <div class="flex items-center justify-center w-full">
                  <el-button
                    type="danger"
                    size="small"
                    link
                    >{{ $t('delete') }}</el-button
                  >
                </div>
              </template>
            </el-popconfirm>
            <el-popconfirm
              v-else
              :title="$t('confirmSave')"
              :cancel-button-text="$t('no')"
              :confirm-button-text="$t('yes')"
              @confirm="updatePriorities(row, 'C')">
              <template #reference>
                <div class="flex items-center justify-center w-full">
                  <el-button
                    type="primary"
                    size="small"
                    link
                    >{{ $t('save') }}</el-button
                  >
                </div>
              </template>
            </el-popconfirm>
          </template>
        </el-table-column>
      </el-table>
      <div class="w-full">
        <div class="w-full flex items-center justify-center py-2">
          <el-button
            type="primary"
            link
            @click="addEmptyPriority()">
            {{ $t('addPriority') }}
            <span class="px-3">
              <ElIconPlus class="w-5" />
            </span>
          </el-button>
        </div>
      </div>
    </div>
    <div id="buildings">
      <h1 class="text-sm">{{ $t('manageBuildings') }}</h1>
      <el-table
        v-loading="buildingsLoading"
        :data="buildings"
        border
        class="my-3 lg:my-1"
        style="width: 100%"
        max-height="200"
        :default-sort="{ prop: 'name[value]', order: 'descending' }"
        @cell-dblclick="(row, column) => handleCellClick(row, column, buildings)">
        <TableEditableColumn
          prop="name"
          :label="$t('building')"
          :save-callback="updateBuildings" />
        <el-table-column
          :label="''"
          width="70">
          <template #default="{ row }">
            <el-popconfirm
              v-if="!row.new"
              :title="$t('confirmDelete')"
              :cancel-button-text="$t('no')"
              :confirm-button-text="$t('yes')"
              @confirm="updateBuildings(row, 'D')">
              <template #reference>
                <div class="flex items-center justify-center w-full">
                  <el-button
                    type="danger"
                    size="small"
                    link
                    >{{ $t('delete') }}</el-button
                  >
                </div>
              </template>
            </el-popconfirm>
            <el-popconfirm
              v-else
              :title="$t('confirmSave')"
              :cancel-button-text="$t('no')"
              :confirm-button-text="$t('yes')"
              @confirm="updateBuildings(row, 'C')">
              <template #reference>
                <div class="flex items-center justify-center w-full">
                  <el-button
                    type="primary"
                    size="small"
                    link
                    >{{ $t('save') }}</el-button
                  >
                </div>
              </template>
            </el-popconfirm>
          </template>
        </el-table-column>
      </el-table>
      <div class="w-full">
        <div class="w-full flex items-center justify-center py-2">
          <el-button
            type="primary"
            link
            @click="addEmptyBuilding()">
            {{ $t('addBuilding') }}
            <span class="px-3">
              <ElIconPlus class="w-5" />
            </span>
          </el-button>
        </div>
      </div>
    </div>
    <div class="">
      <h1 class="text-sm">{{ $t('manageUsers') }}</h1>
      <el-table
        v-loading="usersLoading"
        :data="users"
        border
        class="my-3 lg:my-1"
        max-height="300"
        style="width: 100%"
        :default-sort="{ prop: 'userId[value]', order: 'descending' }">
        <el-table-column
          :label="$t('userId')"
          prop="userId[value]" />
        <el-table-column
          header-align="center"
          :label="$t('admin')">
          <template #default="{ row }">
            <div class="w-full flex justify-center items-center">
              <el-radio-group
                v-model="row.type.value"
                @change="updateUsers(row)">
                <el-radio value="4" />
              </el-radio-group>
              <!-- <el-checkbox v-model="row.isAdmin"></el-checkbox> -->
            </div>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('fm')"
          header-align="center">
          <template #default="{ row }">
            <div class="w-full flex justify-center items-center">
              <el-radio-group
                v-model="row.type.value"
                @change="updateUsers(row)">
                <el-radio value="2" />
              </el-radio-group>
              <!-- <el-checkbox v-model="row.isFm"></el-checkbox> -->
            </div>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('teacher')"
          header-align="center">
          <template #default="{ row }">
            <div class="w-full flex justify-center items-center">
              <el-radio-group
                v-model="row.type.value"
                @change="updateUsers(row)">
                <el-radio value="1" />
              </el-radio-group>
              <!-- <el-checkbox v-model="row.isTeacher"></el-checkbox> -->
            </div>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('noRole')"
          header-align="center">
          <template #default="{ row }">
            <div class="w-full flex justify-center items-center">
              <el-radio-group
                v-model="row.type.value"
                @change="updateUsers(row)">
                <el-radio value="0" />
              </el-radio-group>
              <!-- <el-checkbox v-model="row.isTeacher"></el-checkbox> -->
            </div>
          </template>
        </el-table-column>
      </el-table>
    </div>
    <div
      id="states"
      class="mt-6 xl:mt-0">
      <h1 class="text-sm">{{ $t('manageStatus') }}</h1>
      <el-table
        v-loading="statesLoading"
        :data="states"
        border
        class="my-3 lg:my-1"
        style="width: 100%"
        max-height="300"
        :default-sort="{ prop: 'name[value]', order: 'descending' }"
        @cell-dblclick="(row, column) => handleCellClick(row, column, states)">
        <TableEditableColumn
          prop="name"
          :label="$t('status')"
          :save-callback="updateStates" />
        <el-table-column :label="$t('color')">
          <template #default="{ row }">
            <div class="w-full flex justify-center items-center">
              <el-color-picker
                v-model="row.color.value"
                @change="!row.new && updateStates(row, 'U')" />
            </div>
          </template>
        </el-table-column>
        <el-table-column :label="$t('default')">
          <template #default="{ row }">
            <el-switch
              v-model="row.isDefault.value"
              active-color="#13ce66"
              @change="!row.new && updateStates(row, 'U', true)" />
          </template>
        </el-table-column>

        <el-table-column :label="$t('autoPurge')">
          <template #default="{ row }">
            <el-switch
              v-model="row.hasAutoPurge.value"
              active-color="#13ce66"
              @change="!row.new && updateStates(row, 'U')" />
          </template>
        </el-table-column>
        <TableEditableColumn
          prop="autoPurgeDays"
          input-type="number"
          :label="$t('autoPurgeDays')"
          :save-callback="updateStates" />
        <el-table-column
          :label="''"
          width="70">
          <template #default="{ row }">
            <el-popconfirm
              v-if="!row.new"
              :title="$t('confirmDelete')"
              :cancel-button-text="$t('no')"
              :confirm-button-text="$t('yes')"
              @confirm="updateStates(row, 'D')">
              <template #reference>
                <div class="flex items-center justify-center w-full">
                  <el-button
                    type="danger"
                    size="small"
                    link
                    >{{ $t('delete') }}</el-button
                  >
                </div>
              </template>
            </el-popconfirm>
            <el-popconfirm
              v-else
              :title="$t('confirmSave')"
              :cancel-button-text="$t('no')"
              :confirm-button-text="$t('yes')"
              @confirm="updateStates(row, 'C')">
              <template #reference>
                <div class="flex items-center justify-center w-full">
                  <el-button
                    type="primary"
                    size="small"
                    link
                    >{{ $t('save') }}</el-button
                  >
                </div>
              </template>
            </el-popconfirm>
          </template>
        </el-table-column>
      </el-table>
      <div class="w-full">
        <div class="w-full flex items-center justify-center py-2">
          <el-button
            type="primary"
            link
            @click="addEmptyState()">
            {{ $t('addState') }}
            <span class="px-3">
              <ElIconPlus class="w-5" />
            </span>
          </el-button>
        </div>
      </div>
    </div>
  </div>
  <div
    v-else
    class="w-full h-full">
    <ElIconLoading class="animate-spin w-5" />
  </div>
  <el-dialog
    v-model="isAdminPermissionRemovalWarningVisible"
    width="500"
    :title="i18n.t('ownPermissionChangeWarningTitle')">
    <span>{{ $t('ownPermissionChangeWarningDescription') }}</span>
    <template #footer>
      <div class="dialog-footer">
        <el-button
          type="default"
          @click="isAdminPermissionRemovalWarningVisible = false"
          >{{ $t('close') }}</el-button
        >
      </div>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup>
  import TableEditableColumn, { handleCellClick } from '../tableEditableColumn.vue';
  const i18n = useI18n();
  const {
    priorities,
    buildings,
    states,
    users,
    prioritiesLoading,
    buildingsLoading,
    statesLoading,
    usersLoading,
    initialLoading,
  } = storeToRefs(useSettingsStore());
  const { updateBuildings, updatePriorities, fetchPriorities, updateStates, updateUsers } = useSettingsStore();
  const isAdminPermissionRemovalWarningVisible = ref(false);
  onNuxtReady(async () => {
    document.getElementById('globalSettings')?.addEventListener('defaultStateNotUpdatable', () => {
      ElMessage({
        type: 'warning',
        message: i18n.t('defaultStateNotUpdatable'),
      });
    });
    document.getElementById('globalSettings')?.addEventListener('objectIsReferenced', () => {
      ElMessage({
        type: 'error',
        message: i18n.t('objectIsReferenced'),
      });
    });
    document.getElementById('globalSettings')?.addEventListener('onlyOneDefaultState', () => {
      ElMessage({
        type: 'warning',
        message: i18n.t('onlyOneDefaultState'),
      });
    });
    document.getElementById('globalSettings')?.addEventListener('updateFailed', () => {
      ElMessage({
        type: 'warning',
        message: i18n.t('updateFailed'),
      });
    });
    document.getElementById('globalSettings')?.addEventListener('saved', () => {
      ElMessage({
        type: 'success',
        message: i18n.t('saved'),
      });
    });
    document.getElementById('globalSettings')?.addEventListener('ownRoleChanged', () => {
      isAdminPermissionRemovalWarningVisible.value = true;
    });
  });

  /**
   * Add an empty priority to the list of priorities
   */
  function addEmptyPriority() {
    priorities.value.push({
      color: { editing: false, value: '' },
      // We initialize the id with a date in order to identify multiple new objects in the frontend before it's saved. Will be overwritten later.
      id: { editing: false, value: new Date().toString() },
      name: { editing: false, value: '' },
      value: { editing: false, value: findNewPriorityNumber() },
      new: true,
    });
    // setTimeout(() => {
    //   scrollToBottom("priorities");
    // }, 250);
  }

  /**
   * Add an empty building to the list of buildings
   */
  function addEmptyBuilding() {
    buildings.value.push({
      id: { editing: false, value: new Date().toString() },
      name: { editing: false, value: '' },
      new: true,
    });
    setTimeout(() => {
      scrollToBottom('buildings');
    }, 250);
  }

  /**
   * Add an empty state to the list of states
   */
  function addEmptyState() {
    states.value.push({
      autoPurgeDays: { editing: false, value: null },
      color: { editing: false, value: '' },
      hasAutoPurge: { editing: false, value: false },
      id: { editing: false, value: '' },
      isDefault: { editing: false, value: false },
      name: { editing: false, value: '' },
      new: true,
    });
    setTimeout(() => {
      scrollToBottom('states');
    }, 250);
  }

  const scrollToBottom = (initialElementId?: string, element?: HTMLElement | null | undefined) => {
    if (!element) {
      element = document.getElementById(initialElementId as string);
    }
    if (!element) return;
    element.scrollTo({ top: element.scrollHeight, behavior: 'smooth' });

    // Scroll all its child elements recursively
    Array.from(element.children).forEach(child => {
      if (child instanceof HTMLElement) {
        scrollToBottom(initialElementId, child);
      }
    });
  };

  /**
   * Changes the order of the priorities by decreasing the value of the priority with the given id therefore increasing the value of the priority below
   * @param priorityId The id of the priority to decrease the value of
   */
  async function decreasePriorityValue(priorityId: string) {
    const { thisPriorityIndex, lowerPriorityIndex } = findPriorityAndAround(priorityId);
    if (lowerPriorityIndex === null) return;
    if (thisPriorityIndex !== null) {
      const updatedThisPriority = priorities.value[thisPriorityIndex];
      const updatedLowerPriority = priorities.value[lowerPriorityIndex];
      updatedThisPriority.value.value--;
      updatedLowerPriority.value.value++;
      await updatePriorities(updatedThisPriority, 'U');
      await updatePriorities(updatedLowerPriority, 'U');
      await fetchPriorities();
    }
  }

  /**
   * Changes the order of the priorities by increasing the value of the priority with the given id therefore decreasing the value of the priority above
   * @param priorityId The id of the priority to increase the value of
   */
  async function increasePriorityValue(priorityId: string) {
    const { thisPriorityIndex, higherPriorityIndex } = findPriorityAndAround(priorityId);
    if (higherPriorityIndex === null) return;
    if (thisPriorityIndex !== null) {
      const updatedThisPriority = priorities.value[thisPriorityIndex];
      const updatedHigherPriority = priorities.value[higherPriorityIndex];
      updatedThisPriority.value.value++;
      updatedHigherPriority.value.value--;
      await updatePriorities(updatedThisPriority, 'U');
      await updatePriorities(updatedHigherPriority, 'U');
      await fetchPriorities();
    }
  }

  /**
   *  Finds the priority with the given id and the priorities above and below it
   * @param priorityId The id of the priority to find
   * @returns The index of the priority with the given id and the indexes of the priorities above and below it
   */
  function findPriorityAndAround(priorityId: string): {
    thisPriorityIndex: number | null;
    lowerPriorityIndex: number | null;
    higherPriorityIndex: number | null;
  } {
    let higherPriority = null;
    let lowerPriority = null;
    let thisPriority = null;
    priorities.value.forEach((p, index) => {
      if (p.id.value === priorityId) {
        thisPriority = index;
        lowerPriority = typeof priorities.value[index - 1] !== 'undefined' ? index - 1 : null;
        higherPriority = typeof priorities.value[index + 1] !== 'undefined' ? index + 1 : null;
        return;
      }
    });
    return {
      thisPriorityIndex: thisPriority,
      higherPriorityIndex: higherPriority,
      lowerPriorityIndex: lowerPriority,
    };
  }

  /**
   * Returns the highest priority value + 1. Used to find the next priority value when adding a new priority^
   * @returns The highest priority value + 1
   */
  function findNewPriorityNumber() {
    let highest = 0;
    priorities.value.forEach(p => {
      if (p.value.value > highest) highest = p.value.value;
    });
    return highest + 1;
  }
</script>

<style></style>

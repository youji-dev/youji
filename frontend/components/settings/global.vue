<template>
  <div
    id="globalsettings"
    class="flex flex-col xl:grid xl:grid-cols-2 xl:gap-4"
    v-if="!loading"
  >
    <div class="">
      <h1 class="text-sm">{{ $t("managePriorities") }}</h1>
      <el-table
        :data="priorities"
        border
        class="my-3 lg:my-1"
        max-height="200"
        style="width: 100%"
        :default-sort="{ prop: 'name[value]', order: 'descending' }"
        v-loading="prioritiesLoading"
        @cell-dblclick="
          (row, column) => handleCellClick(row, column, priorities)
        "
      >
        <TableEditableColumn
          prop="value"
          :label="$t('priority')"
          :save-callback="updatePriorities"
        />
        <TableEditableColumn
          prop="name"
          :label="$t('name')"
          :save-callback="updatePriorities"
        />
        <el-table-column :label="$t('color')">
          <template #default="{ row }">
            <div class="w-full flex justify-center items-center">
              <el-color-picker @change="!row.new && updatePriorities(row, 'U')" v-model="row.color.value" />
            </div>
          </template>
        </el-table-column>
        <el-table-column :label="''">
          <template #default="{ row }">
            <el-popconfirm
              :title="$t('confirmDelete')"
              :cancel-button-text="$t('no')"
              :confirm-button-text="$t('yes')"
              @confirm="updatePriorities(row, 'D')"
              v-if="!row.new"
            >
              <template #reference>
                <div class="flex items-center justify-center w-full">
                  <el-button type="danger" size="small" link>{{
                    $t("delete")
                  }}</el-button>
                </div>
              </template>
            </el-popconfirm>
            <el-popconfirm
              :title="$t('confirmSave')"
              :cancel-button-text="$t('no')"
              :confirm-button-text="$t('yes')"
              @confirm="updatePriorities(row, 'C')"
              v-else
            >
              <template #reference>
                <div class="flex items-center justify-center w-full">
                  <el-button type="primary" size="small" link>{{
                    $t("save")
                  }}</el-button>
                </div>
              </template>
            </el-popconfirm>
          </template>
        </el-table-column>
      </el-table>
      <div class="w-full ">
            <div class="w-full flex items-center justify-center py-2 ">
              <el-button
                type="primary"
                link
                @click="
                  priorities.push({
                    color: { editing: false, value: '' },
                    // We initialize the id with a date in order to identify multiple new objects in the frontend before it's saved. Will be overwritten later.
                    id: { editing: false, value: new Date().toString() },
                    name: { editing: false, value: '' },
                    value: { editing: false, value: 0 },
                    new: true,
                  }); 
                "
              >
                {{ $t("addPriority") }}
                <span class="px-3"><ElIconPlus class="w-5" /></span>
              </el-button>
            </div>
          </div>
    </div>
    <div class="">
      <h1 class="text-sm">{{ $t("manageBuildings") }}</h1>
      <el-table
        :data="buildings"
        border
        class="my-3 lg:my-1"
        style="width: 100%"
        max-height="200"
        v-loading="buildingsLoading"
        :default-sort="{ prop: 'name[value]', order: 'descending' }"
        @cell-dblclick="
          (row, column) => handleCellClick(row, column, buildings)
        "
      >
        <TableEditableColumn
          prop="name"
          :label="$t('building')"
          :save-callback="updateBuildings"
        />
        <el-table-column :label="''" width="70">
          <template #default="{ row }">
            <el-popconfirm
              :title="$t('confirmDelete')"
              :cancel-button-text="$t('no')"
              :confirm-button-text="$t('yes')"
              @confirm="updateBuildings(row, 'D')"
              v-if="!row.new"
            >
              <template #reference>
                <div class="flex items-center justify-center w-full">
                  <el-button type="danger" size="small" link>{{
                    $t("delete")
                  }}</el-button>
                </div>
              </template>
            </el-popconfirm>
            <el-popconfirm
              :title="$t('confirmSave')"
              :cancel-button-text="$t('no')"
              :confirm-button-text="$t('yes')"
              @confirm="updateBuildings(row, 'C')"
              v-else
            >
              <template #reference>
                <div class="flex items-center justify-center w-full">
                  <el-button type="primary" size="small" link>{{
                    $t("save")
                  }}</el-button>
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
            @click="
              buildings.push({
                id: { editing: false, value: new Date().toString() },
                name: { editing: false, value: '' },
                new: true,
              })
            "
          >
            {{ $t("addBuilding") }}
            <span class="px-3"><ElIconPlus class="w-5" /></span>
          </el-button>
        </div>
      </div>
    </div>
    <div class="">
      <h1 class="text-sm">{{ $t("manageUsers") }}</h1>
      <!-- <label for="select-fms" class="text-xs m-0 py-3">{{
        $t("selectFms")
      }}</label>
      <el-select id="select-fms" multiple></el-select>
      <label for="select-admins" class="text-xs m-0 py-3">{{
        $t("selectAdmins")
      }}</label>
      <el-select id="select-admins" multiple></el-select> -->
      <el-table
        :data="users"
        border
        class="my-3 lg:my-1"
        height="300"
        style="width: 100%;"
        v-loading="usersLoading"
        :default-sort="{prop: 'userId[value]', order: 'descending'}">
        <el-table-column :label="$t('userId')" prop="userId[value]"/>
        <el-table-column :label="$t('admin')">
          <template #default="{row}">
            <el-checkbox v-model="row.isAdmin"></el-checkbox>
          </template>
        </el-table-column>
        <el-table-column :label="$t('fm')">
          <template #default="{row}">
            <el-checkbox v-model="row.isFm"></el-checkbox>
          </template>
        </el-table-column>
      </el-table>
    </div>
    <div class="mt-6 xl:mt-0">
      <h1 class="text-sm">{{ $t("manageStatus") }}</h1>
      <el-table
        :data="states"
        border
        class="my-3 lg:my-1"
        style="width: 100%"
        max-height="300"
        v-loading="statesLoading"
        @cell-dblclick="(row, column) => handleCellClick(row, column, states)"
        :default-sort="{ prop: 'name[value]', order: 'descending' }"
      >
        <TableEditableColumn
          prop="name"
          :label="$t('status')"
          :save-callback="updateStates"
        />
        <el-table-column :label="$t('color')">
          <template #default="{ row }">
            <div class="w-full flex justify-center items-center">
              <el-color-picker @change="!row.new && updateStates(row, 'U')" v-model="row.color.value" />
            </div>
          </template>
        </el-table-column>
        <el-table-column :label="$t('default')">
          <template #default="{ row }">
            <el-switch
              v-model="row.isDefault.value"
              active-color="#13ce66"
              @change="!row.new && updateStates(row, 'U', true)"
            />
          </template>
        </el-table-column>

        <el-table-column :label="$t('autoPurge')">
          <template #default="{ row }">
            <el-switch
              v-model="row.hasAutoPurge.value"
              active-color="#13ce66"
              @change="!row.new && updateStates(row, 'U')"
            />
          </template>
        </el-table-column>
        <TableEditableColumn
          prop="autoPurgeDays"
          input-type="number"
          :label="$t('autoPurgeDays')"
          :save-callback="updateStates"
        />
        <el-table-column :label="''" width="70">
          <template #default="{ row }">
            <el-popconfirm
              :title="$t('confirmDelete')"
              :cancel-button-text="$t('no')"
              :confirm-button-text="$t('yes')"
              @confirm="updateStates(row, 'D')"
              v-if="!row.new"
            >
              <template #reference>
                <div class="flex items-center justify-center w-full">
                  <el-button type="danger" size="small" link>{{
                    $t("delete")
                  }}</el-button>
                </div>
              </template>
            </el-popconfirm>
            <el-popconfirm
              :title="$t('confirmSave')"
              :cancel-button-text="$t('no')"
              :confirm-button-text="$t('yes')"
              @confirm="updateStates(row, 'C')"
              v-else
            >
              <template #reference>
                <div class="flex items-center justify-center w-full">
                  <el-button type="primary" size="small" link>{{
                    $t("save")
                  }}</el-button>
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
            @click="
              states.push({
                autoPurgeDays: { editing: false, value: null },
                color: { editing: false, value: '' },
                hasAutoPurge: { editing: false, value: false },
                id: { editing: false, value: '' },
                isDefault: { editing: false, value: false },
                name: { editing: false, value: '' },
                new: true,
              })
            "
          >
            {{ $t("addState") }}
            <span class="px-3"><ElIconPlus class="w-5" /></span>
          </el-button>
        </div>
      </div>
    </div>
  </div>
  <div v-else class="w-full h-full">
    <ElIconLoading class="animate-spin w-5" />
  </div>
</template>

<script lang="ts" setup>
import TableEditableColumn, {
  handleCellClick,
} from "../tableEditableColumn.vue";
const i18n = useI18n();
const {
  priorities,
  buildings,
  states,
  users,
  prioritiesLoading,
  buildingsLoading,
  preferredEmailLanguage,
  statesLoading,
  usersLoading,
} = storeToRefs(useSettingsStore());
const {
  fetchBuildings,
  fetchPreferredEmailLanguage,
  fetchPriorities,
  fetchStates,
  fetchUsers,
  updateBuildings,
  updatePriorities,
  updateStates,
  updateUsers,
} = useSettingsStore();
const loading = ref(true);
onNuxtReady(async () => {
  await fetchBuildings();
  await fetchPriorities();
  await fetchStates();
  await fetchUsers();
  loading.value = false;
  document
    .getElementById("globalsettings")
    ?.addEventListener("defaultStateNotUpdatable", () => {
      ElMessage({
        type: "warning",
        message: i18n.t("defaultStateNotUpdatable"),
      });
    });
  document
    .getElementById("globalsettings")
    ?.addEventListener("objectIsRefereced", () => {
      ElMessage({
        type: "error",
        message: i18n.t("objectIsRefereced"),
      });
    });
});
</script>

<style></style>

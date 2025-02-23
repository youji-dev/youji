<template>
  <div
    class="flex flex-col lg:grid lg:grid-cols-2 lg:gap-2 xl:grid-cols-3 xl:gap-4"
  >
    <div class="">
      <h1 class="text-sm">{{ $t("managePriorities") }}</h1>
      <el-table
        :data="tableData"
        border
        class="my-3 lg:my-1"
        style="width: 100%"
      >
        <el-table-column prop="priority" :label="$t('priority')" />
        <el-table-column prop="name" :label="$t('name')" />
        <el-table-column prop="color" :label="$t('color')" />
        <el-table-column :label="''">
          <template #default>
            <el-button type="danger" size="small" plain>{{
              $t("delete")
            }}</el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>
    <div class="">
      <h1 class="text-sm">{{ $t("manageStatus") }}</h1>
      <el-table
        :data="tableData"
        border
        class="my-3 lg:my-1"
        style="width: 100%"
      >
        <el-table-column prop="status" :label="$t('status')" />
        <el-table-column prop="color" :label="$t('color')" />
        <el-table-column :label="''" width="70">
          <template #default>
            <el-button type="danger" size="small" plain>{{
              $t("delete")
            }}</el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>
    <div class="flex flex-col">
      <h1 class="text-sm">{{ $t("manageUsers") }}</h1>
      <label for="select-fms" class="text-xs m-0 py-3">{{
        $t("selectFms")
      }}</label>
      <el-select id="select-fms" multiple></el-select>
      <label for="select-admins" class="text-xs m-0 py-3">{{
        $t("selectAdmins")
      }}</label>
      <el-select id="select-admins" multiple></el-select>
    </div>
    <div class="col-span-2">
      <h1 class="text-sm">{{ $t("manageBuildings") }}</h1>
      <el-table
        :data="tableData"
        border
        class="my-3 lg:my-1"
        style="width: 100%"
      >
        <el-table-column prop="priority" :label="$t('priority')" />
        <el-table-column prop="name" :label="$t('name')" />
        <el-table-column prop="color" :label="$t('color')" />
        <el-table-column :label="''" width="70">
          <template #default>
            <el-button type="danger" size="small" plain>{{
              $t("delete")
            }}</el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>
  </div>
</template>

<script lang="ts" setup></script>

<style></style>

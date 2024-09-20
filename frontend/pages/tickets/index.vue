<template>
  <div class="w-full h-[100vh] p-6 mt-8 md:mt-0" style="height: calc(100vh - 72px)">
    <div class="flex flex-row items-center justify-between w-full py-2">
      <h1
        class="text-lg font-light text-neutral-700 dark:text-neutral-300 text-start w-fit h-fit"
      >
        {{ $t("ticketOverview") }}
      </h1>
      <div class="w-1/2 md:w-1/6">
        <el-input
          v-model="search"
          style="width: 100%"
          :placeholder="$t('searchVerb')"
          :prefix-icon="Search"
        />
      </div>
    </div>
    <div
      class="w-full h-[100%] overflow-y-scroll flex flex-col justify-center items-center base-bg-light dark:base-bg-dark rounded-md"
      id="table_container"
    >
      <el-table-v2
        :columns="columns"
        :data="data"
        :width="tableDimensions.width"
        :height="tableDimensions.height"
      />
    </div>
  </div>
</template>

<script lang="tsx" setup>
import { Search } from "@element-plus/icons-vue";
import { ref } from "vue";
const search = ref("");
const i18n = useI18n();
import { ElButton, ElTag, ElTooltip, TableV2FixedDir } from "element-plus";
import type { Column } from "element-plus";
const localeRoute = useLocaleRoute();
const tableDimensions = ref({
  width: 0,
  height: 0,
});
let id = 0;
const dataGenerator = () => ({
  id: `random-id-${++id}`,
  name: "Tom",
  title: {title: "Test Ticket", id: id},
  status: {text: "Offen", color: "primary"},
  building: "Hauptgebäude",
  room: "222",
  priority: "3",
  create_date: "20.09.2024"
});
const columns: Ref<Column<any>[]> = ref([]);
const data = ref([] as Array<{}>);
onMounted(() => {
  tableDimensions.value = getTableDimensions();
  columns.value = [
  {
    key: "title",
    title: i18n.t("title"),
    dataKey: "title",
    width: (tableDimensions.value.width / 16) * 2,
    fixed: TableV2FixedDir.LEFT,
    cellRenderer: ({ cellData: title }) => (
      <ElTooltip content={title}>
        {
          <a href={localeRoute("/tickets/"+ title.id)?.fullPath } class="flex items-center">
            {title.title}
          </a>
        }
      </ElTooltip>
    ),
  },
  {
    key: "status",
    title: i18n.t("status"),
    dataKey: "status",
    width: (tableDimensions.value.width / 16) * 2,
    align: "center",
    cellRenderer: ({ cellData: status }) => <el-tag type="primary" plain>{status.text}</el-tag>,
  },
  {
    key: "name",
    title: i18n.t("user"),
    dataKey: "name",
    width: (tableDimensions.value.width / 16) * 2,
    align: "center",
    cellRenderer: ({ cellData: name }) => <span>{name}</span>,
  },
  {
    key: "building",
    title: i18n.t("building"),
    dataKey: "building",
    width: (tableDimensions.value.width / 16) * 2,
    align: "center",
    cellRenderer: ({ cellData: building }) => <span>{building}</span>,
  },
  {
    key: "room",
    title: i18n.t("room"),
    dataKey: "room",
    width: (tableDimensions.value.width / 16) * 2,
    align: "center",
    cellRenderer: ({ cellData: room }) => <span>{room}</span>,
  },
  {
    key: "priority",
    title: i18n.t("priority"),
    dataKey: "priority",
    width: (tableDimensions.value.width / 16) * 2,
    align: "center",
    cellRenderer: ({ cellData: priority }) => <span>{priority + " - " + i18n.t("priority_" + priority)}</span>,
  },
  {
    key: "create_date",
    title: i18n.t("createDate"),
    dataKey: "create_date",
    width: (tableDimensions.value.width / 16) * 2,
    align: "center",
    cellRenderer: ({ cellData: create_date }) => <span>{create_date}</span>,
  },
  {
    key: "operations",
    title: "Operations",
    cellRenderer: () => (
      <>
        <ElButton size="small" type="primary" round>Edit</ElButton>
      </>
    ),
    width: 150,
    align: "center",
  },
];
data.value = Array.from({ length: 200 }).map(dataGenerator);
});

function getTableDimensions() {
  const element = document.getElementById("table_container");
  if (element) {
    return {
      width: element.offsetWidth,
      height: element.offsetHeight,
    };
  } else {
    return {
      width: 1000,
      height: 1000,
    };
  }
}
</script>

<style></style>

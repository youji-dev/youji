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
      <div v-if="loading" class="w-full h-full p-10">
        <el-skeleton :rows="23" animated />
      </div>
      <el-table
        v-if="!loading"
        :data="filterTableData"
        :height="tableDimensions['height']"
        :width="tableDimensions['width']"
        style="width: 100%"
        :default-sort="{ prop: 'create_date', order: 'descending' }"
      >
        <el-table-column prop="id" :label="$t('id')" width="180" sortable />
        <el-table-column prop="name" :label="$t('username')" width="180" sortable />
        <el-table-column prop="title.title" :label="$t('title')" sortable />
        <el-table-column
          prop="status.text"
          :label="$t('status')"
          :filters="parsedStatusOptions"
          :filter-method="filterTag"
          filter-placement="bottom-end"
          sortable
        >
          <template #default="scope">
            <el-tag :type="scope.row.status.color">
              {{ $t(scope.row.status.text) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="building" :label="$t('building')" sortable />
        <el-table-column prop="room" :label="$t('room')" sortable />
        <el-table-column prop="priority" :label="$t('priority')" sortable />
        <el-table-column prop="create_date" :label="$t('createDate')" sortable />
      </el-table>
    </div>
    <el-pagination layout="prev, pager, next" :total="1000" />
  </div>
</template>

<script lang="tsx" setup>
import { Search } from "@element-plus/icons-vue";
import { ref } from "vue";
const search = ref("");
const i18n = useI18n();
import { ElTag } from "element-plus";
const { statusOptions } = storeToRefs(useTicketsStore());
const parsedStatusOptions = ref([]) as Ref<Array<any>>;
const { fetchStatusOptions } = useTicketsStore();
const loading = ref(true);
interface Ticket {
  id: number;
  name: string;
  title: {title: string, id: number};
  status: { text: string; color: string };
  building: string;
  room: string;
  priority: string;
  create_date: string;
}

const tableDimensions = ref({
  width: 0,
  height: 0,
});
let id = 0;
const dataGenerator = () => ({
  id: ++id,
  name: "Tom",
  title: { title: "Test Ticket", id: id },
  status: {
    text: id % 2 === 0 ? "Neu" : "Abgeschlossen",
    color: id % 2 === 0 ? "primary" : "success",
  },
  building: "Hauptgebäude",
  room: "222",
  priority: "3",
  create_date: (id % 2 === 0) ? "20.09.2024" : "21.09.2024",
});

const data = ref([] as Array<Ticket>);
onMounted(async () => {
  data.value = Array.from({ length: 200 }).map(dataGenerator);
  getTableDimensions();
  await fetchStatusOptions();
  parseStatusOptions();
  loading.value = false;
});
const filterTag = (value: string, row: Ticket) => {
  return row.status.text === value;
};
const filterTableData = computed(() =>
  data.value.filter(
    (data) =>
      !search.value ||
      data.name.toLowerCase().includes(search.value.toLowerCase()) ||
      data.building.toLowerCase().includes(search.value.toLowerCase()) ||
      data.room.toLowerCase().includes(search.value.toLowerCase()) ||
      data.id.toString().includes(search.value.toLowerCase()) ||
      data.priority.toLowerCase().includes(search.value.toLowerCase()) ||
      data.title.title.toLowerCase().includes(search.value.toLowerCase()) ||
      data.status.text.toLowerCase().includes(search.value.toLowerCase()) 
  )
);
function getTableDimensions() {
  const element = document.getElementById("table_container");
  if (element) {
    tableDimensions.value.width = element.offsetWidth;
    tableDimensions.value.height = element.offsetHeight;
    return;
  } else {
    tableDimensions.value.width = 1000;
    tableDimensions.value.height = 1000;
  }
}

function parseStatusOptions() {
  statusOptions.value.forEach((element) => {
    parsedStatusOptions.value.push({
      text: element.text,
      value: element.text,
    });
  });
}
</script>

<style></style>

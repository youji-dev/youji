<template>
  <div class="w-full h-[100vh] p-6 mt-8 lg:mt-0" style="height: calc(100vh - 72px)" :style="{ width: width }"
    :onresize="determineViewWidth()">
    <div class="flex flex-row items-center justify-between w-full py-2">
      <h1 class="text-lg font-light text-neutral-700 dark:text-neutral-300 text-start w-fit h-fit">
        {{ $t("ticketOverview") }}
      </h1>
      <div class="w-1/2 md:lg-1/6">
        <el-input v-model="search" style="width: 100%" :placeholder="$t('searchVerb')" :prefix-icon="Search" />
      </div>
    </div>
    <div
      class="w-full h-[100%] overflow-y-scroll flex flex-col justify-center items-center base-bg-light dark:base-bg-dark rounded-md"
      id="table_container">
      <div v-if="loading" class="w-full h-full p-10">
        <el-skeleton :rows="23" animated />
      </div>
      <el-table v-if="!loading" :data="filterTableData" :height="tableDimensions['height']"
        style="width: 100%; height: 100%" class="overflow-x-scroll"
        :default-sort="{ prop: 'create_date', order: 'descending' }">
          <el-table-column prop="id" :label="$t('id')" width="100" sortable />
        <el-table-column class="hidden lg:block" prop="name" :label="$t('username')" width="150" sortable />
        <el-table-column prop="title.title" :label="$t('title')" width="250" sortable />
        <el-table-column prop="status.text" :label="$t('status')" :filters="parsedStatusOptions"
          :filter-method="filterTag" filter-placement="bottom-end" width="300" sortable>
          <template #default="scope">
            <div class="flex justtify-around items-center">
              <el-select class="w-2/3 mx-1" v-model="scope.row.status" value-key="text"
                @change="updateTicketStatus(scope.row.id, scope.row.status)">
                <el-option v-for="option in statusOptions" :key="option.text" :label="option.text"
                  :value="option"></el-option>
              </el-select>
              <el-tag class="w-1/3 mx-1 hidden lg:flex" :type="scope.row.status.color" disable-transitions>
                {{ scope.row.status.text }}
              </el-tag>
            </div>
          </template>
        </el-table-column>
        <el-table-column prop="building" class="hidden lg:block" :label="$t('building')" width="150" sortable />
        <el-table-column prop="room" class="hidden lg:block" :label="$t('room')" width="100" sortable />
        <el-table-column prop="priority" class="hidden lg:block" :label="$t('priority')" width="100" sortable />
        <el-table-column class="hidden lg:block" prop="create_date" :label="$t('createDate')" width="200" sortable />
        <el-table-column fixed="right" label="Operations" min-width="120">
          <template #default="scope">
            <el-button link type="primary" size="small"
              @click="router.push(localeRoute(`/tickets/${scope.row.id}`)?.fullPath as string)">
              {{ $t("detail") }}
            </el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination class="mr-auto" v-if="!loading" layout="prev, pager, next" :total="1000" />
    </div>
  </div>
</template>

<script lang="tsx" setup>
import { Search } from "@element-plus/icons-vue";
import { ref } from "vue";
const search = ref("");
import { ElTag } from "element-plus";
import type { Status } from "~/stores/tickets";
const { statusOptions } = storeToRefs(useTicketsStore());
const parsedStatusOptions = ref([]) as Ref<Array<any>>;
const { fetchStatusOptions } = useTicketsStore();
const loading = ref(true);
const localeRoute = useLocaleRoute();
const router = useRouter();
const i18n = useI18n();
interface Ticket {
  id: number;
  name: string;
  title: { title: string; id: number };
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
  create_date: id % 2 === 0 ? "20.09.2024" : "21.09.2024",
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

async function updateTicketStatus(ticket_id: number, new_status: Status) {
  // Update actual Ticket Status in backend
  ElMessage({
    message: i18n.t("updated"),
    type: "success",
  });
  return;
}
</script>

<style></style>

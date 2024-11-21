<template>
  <div
    class="w-full h-full py-6 lg:py-0 px-6 mt-6 lg:mt-0"
    style="height: calc(100vh - 72px)"
    :style="{ width: width }"
    :onresize="determineViewWidth()"
  >
    <div class="flex flex-row items-center justify-end w-full py-2">
      <div class="w-1/2 md:w-1/3 lg:w-1/5">
        <el-input
          v-model="search"
          class="w-full"
          :placeholder="$t('searchVerb')"
          :prefix-icon="Search"
          @change="fetchTicketsFromStart()"
        />
      </div>
    </div>
    <div
      class="w-full h-full overflow-y-scroll flex flex-col justify-center items-center base-bg-light dark:base-bg-dark rounded-md"
      id="table_container"
    >
      <div v-if="loading" class="w-full h-full p-10">
        <el-skeleton :rows="18" animated />
      </div>
      <div v-if="pageLoading" class="w-full h-full p-10 flex justify-center items-center">
        <ElIconLoading class="animate-spin w-5"></ElIconLoading>
      </div>
      <!-- TODO : Somehow change the default element-plus sorting to comply with pagination. When any of the possbile sorting arrows are clicked, the data has to be fetched again completely.
       The page should stay the same. -->
      <el-table
        v-if="!loading && !pageLoading"
        :data="parsedTickets"
        :height="tableDimensions['height']"
        style="width: 100%; height: min-content"
        class="overflow-x-scroll"
        :default-sort="{ prop: 'create_date', order: 'descending' }"
      >
        <el-table-column
          class="hidden lg:block"
          prop="author"
          :label="$t('username')"
          width="150"
          sortable
        />
        <el-table-column prop="title" :label="$t('title')" width="250" sortable />
        <el-table-column
          prop="state.name"
          :label="$t('status')"
          :filters="[]"
          :filter-method="filterTag"
          filter-placement="bottom-end"
          width="200"
          sortable
        > 
          <template #default="scope">
            <div class="flex justify-start items-center">
              <ColoredSelect
                :color="scope.row.state.color"
                :change-callback="updateTicketStatus"
                :change-call-back-params="[scope.row.id]"
                :add-current-value-to-callback="true"
                :current="scope.row.state"
                :options="statusOptions"
                :keyText="'id'"
                :labelText="'name'"
              ></ColoredSelect>
            </div>
          </template>
        </el-table-column>
        <el-table-column
          prop="building.name"
          class="hidden lg:block"
          :label="$t('building')"
          width="150"
          sortable
        />
        <el-table-column
          prop="room"
          class="hidden lg:block"
          :label="$t('room')"
          width="100"
          sortable
        />
        <el-table-column
          prop="priority.name"
          class="hidden lg:block"
          :label="$t('priority')"
          width="120"
          sortable
        />
        <el-table-column
          class="hidden lg:block"
          prop="creationDate"
          :label="$t('createDate')"
          width="200"
          sortable
        />
        <el-table-column fixed="right"  min-width="120">
          <template #default="scope">
            <el-button
              link
              type="primary"
              size="small"
              @click="router.push(localeRoute(`/tickets/${scope.row.id}`)?.fullPath as string)"
            >
              {{ $t("detail") }}
              <ElIconEdit class="w-5 mx-2"></ElIconEdit>
            </el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination
        class="mr-auto"
        v-if="!loading"
        layout="prev, pager, next"
        :total="totalCount"
        :page-size="25"
        @current-change="fetchNewPage"
      />
    </div>
  </div>
</template>

<script lang="tsx" setup>
import { Search } from "@element-plus/icons-vue";
import { ref } from "vue";
const search = ref("");
import ColoredSelect from "~/components/coloredSelect.vue";
import type state from "~/types/api/response/stateResponse";
const { statusOptions, tickets, totalCount } = storeToRefs(useTicketsStore());
const { fetchStatusOptions, fetchTickets } = useTicketsStore();
const loading = ref(true);
const pageLoading = ref(false);
const localeRoute = useLocaleRoute();
const router = useRouter();
const i18n = useI18n();
const width = ref("100vw");
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

onMounted(async () => {
  getTableDimensions();
});

onNuxtReady(async () => {
  await fetchStatusOptions();
  await fetchTicketsFromStart(false);
  determineViewWidth();
  window.addEventListener("resize", determineViewWidth);
});

async function fetchTicketsFromStart(fromSearch : boolean) {
  pageLoading.value = fromSearch;
  loading.value = !fromSearch;
  await fetchTickets(search.value, 0, 25);
  pageLoading.value = false;
  loading.value = false;
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

const filterTag = (value: string, row: Ticket) => {
  return row.status.text === value;
};

const parsedTickets = computed(() => {
  return tickets.value.map((ticket) => {
    return {
      id: ticket.id,
      title: ticket.title,
      description: ticket.description,
      author: ticket.author,
      creationDate: new Date(ticket.creationDate).toLocaleString(),
      priority: ticket.priority,
      state: ticket.state,
      building: ticket.building,
      room: ticket.room,
      object: ticket.object,
      comments: ticket.comments,
      attachments: ticket.attachments
    }
  }
)});

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

function updateTicketStatus(ticket_id: number, new_status: state) {
  ElMessage({
    message: i18n.t("updated"),
    type: "success",
  });
  return;
}

async function fetchNewPage(page: number) {
  pageLoading.value = true;
  await fetchTickets(search.value, page * 25, 25);
  pageLoading.value = false;
}


</script>

<style></style>

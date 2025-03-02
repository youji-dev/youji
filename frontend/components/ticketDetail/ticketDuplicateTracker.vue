<template>
  <div>
    <el-card
      class="block drop-shadow-xl base-bg-light dark:base-bg-dark"
      v-resize-observer="onResize"
    >
      <!-- No Results found Page -->
      <el-empty
        style="height: 230px"
        v-if="ticketSearchResult.length === 0"
        :description="$t('noDuplicatesFound')"
      />
      <!-- Results found Page -->
      <el-table-v2
        v-else
        id="ticketDuplicateTracker"
        :columns="columns"
        :data="ticketSearchResult"
        :width="tableDimensions.width"
        :height="230"
        :sort-by="sortState"
        @column-sort="onSort"
      />
    </el-card>
  </div>
</template>

<script lang="tsx" setup>
import { vResizeObserver } from "@vueuse/components";
import ColoredSelect from "~/components/coloredSelect.vue";
import type ticket from "~/types/api/response/ticketResponse";
import type state from "~/types/api/response/stateResponse";
import type priority from "~/types/api/response/priorityResponse";
import { TableV2SortOrder, type Column, type SortBy } from "element-plus";
import { ColoredSelectOption } from "~/types/frontend/ColoredSelectOption";
import { FixedDir } from "element-plus/es/components/table-v2/src/constants.mjs";

const i18n = useI18n();
const { $api } = useNuxtApp();
const { statusOptions, priorityOptions } = storeToRefs(useTicketsStore());
const { fetchStatusOptions, fetchPriorityOptions } = useTicketsStore();
const localeRoute = useLocaleRoute();

const props = defineProps<{
  ticket: ticket;
}>();

let ticketFilter: Record<string, any[]> = {};
let skip = 0;
let take = 0;
let limit = 0;
const refreshTimeout: Ref<NodeJS.Timeout | null> = ref(null);
const ticketSearchResult = ref<ticket[]>([]);

const sortState = ref<SortBy>({
  key: "creationDate",
  order: TableV2SortOrder.DESC,
});
const tableDimensions = ref({
  width: 0,
  height: 0,
});
const columns: Ref<Column<any>[]> = ref([
  {
    key: "title",
    title: i18n.t("title"),
    dataKey: "title",
    width: 250,
  },
  {
    key: "state",
    title: i18n.t("state"),
    dataKey: "state",
    width: 200,
    cellRenderer: ({ cellData: state }) => (
      <ColoredSelect
        options={statusOptions.value.map((state: state) => ({
          option: state,
          color: state.color,
        }))}
        current={new ColoredSelectOption(state, state.color)}
        keyText="id"
        labelText="name"
        changeCallback={() => {}}
        readOnly={true}
      />
    ),
  },
  {
    key: "building",
    title: i18n.t("building"),
    dataKey: "building",
    width: 150,
    cellRenderer: ({ cellData: building }) => building.name,
  },
  {
    key: "room",
    title: i18n.t("room"),
    dataKey: "room",
    width: 100,
  },
  {
    key: "priority",
    title: i18n.t("priority"),
    dataKey: "priority",
    width: 150,
    cellRenderer: ({ cellData: priority }) => (
      <ColoredSelect
        options={priorityOptions.value.map((priority: priority) => ({
          option: priority,
          color: priority.color,
        }))}
        current={new ColoredSelectOption(priority, priority.color)}
        keyText="id"
        labelText="name"
        changeCallback={() => {}}
        readOnly={true}
      />
    ),
  },
  {
    key: "creationDate",
    title: i18n.t("createDate"),
    dataKey: "creationDate",
    width: 122,
    sortable: true,
    cellRenderer: ({ cellData: creationDate }) => (
      <p> {new Date(creationDate).toLocaleDateString()} </p>
    ),
  },
  {
    key: "buttons",
    dataKey: "id",
    width: 70,
    fixed: FixedDir.RIGHT,
    cellRenderer: ({ cellData: id }) => (
      <el-tooltip
        content={i18n.t("openInNewTab")}
        placement="top-start"
        show-after={500}
      >
        <el-button
          link
          type="primary"
          size="small"
          onClick={() => {
            window.open(
              localeRoute(`/tickets/${id}`)?.fullPath as string,
              "_blank"
            );
          }}
        >
          <ElIconEdit class="w-5 mx-2" />
        </el-button>
      </el-tooltip>
    ),
  },
]);

onNuxtReady(async () => {
  await fetchStatusOptions();
  await fetchPriorityOptions();
});

watch(
  () => props.ticket,
  async () => {
    if (refreshTimeout.value) clearTimeout(refreshTimeout.value);

    refreshTimeout.value = setTimeout(async () => {
      ticketFilter = {};
      addTitleFilters();
      addDescriptionFilters();
      addObjectFilters();
      addRoomFilters();
      await searchForTickets();
    }, 1500);
  },
  { deep: true }
);

function addTitleFilters(): void {
  if (props.ticket.title.length > 0) {
    ticketFilter.Title = tokenize(props.ticket.title);
  }
}

function addDescriptionFilters(): void {
  if (props.ticket.description) {
    ticketFilter.District = tokenize(props.ticket.description);
  }
}

function addRoomFilters(): void {
  if (props.ticket.room) {
    ticketFilter.Room = tokenize(props.ticket.room);
  }
}

function addObjectFilters(): void {
  if (props.ticket.object) {
    ticketFilter.Object = tokenize(props.ticket.object);
  }
}

function tokenize(text: string): string[] {
  const stopWords = new Set([
    "der",
    "die",
    "das",
    "und",
    "ist",
    "im",
    "in",
    "am",
    "ein",
    "eine",
    "zu",
    "auf",
    "mit",
    "für",
    "von",
    "aus",
    "an",
    "oder",
    "wie",
    "weil",
    "wenn",
    "dann",
    "the",
    "and",
    "is",
    "in",
    "at",
    "a",
    "an",
    "to",
    "on",
    "with",
    "for",
    "of",
    "from",
    "by",
    "or",
    "as",
    "because",
    "if",
    "then",
  ]);
  const tokens = text.toLowerCase().match(/\b[\wäöüß]+\b/g) || [];
  tokens.filter((word) => !stopWords.has(word));
  return tokens.filter((token) => token.length > 4);
}

async function searchForTickets(): Promise<void> {
  for (const key in ticketFilter) {
    if (ticketFilter[key].length === 0) {
      delete ticketFilter[key];
    }
  }

  if (Object.keys(ticketFilter).length === 0) {
    ticketSearchResult.value = [];
    return;
  }

  var response = await $api.ticket.search(
    ticketFilter,
    "creationDate",
    sortState.value.order == TableV2SortOrder.DESC,
    0,
    15,
    true
  );

  if (response.data.value == null) {
    ticketSearchResult.value = [];
    return;
  }

  ticketSearchResult.value = response.data.value.results;
}

function onResize(entries: ResizeObserverEntry[]): void {
  const entry = entries[0];
  const { width, height } = entry.contentRect;
  tableDimensions.value = { width: width - 45, height };
  setCellsHidden();
}

function setCellsHidden() {
  if (tableDimensions.value.width < 1000) {
    columns.value[2].hidden = true;
    columns.value[3].hidden = true;
    columns.value[4].hidden = true;
  } else {
    columns.value[2].hidden = false;
    columns.value[3].hidden = false;
    columns.value[4].hidden = false;
  }
}

const onSort = (sortBy: SortBy) => {
  sortState.value = sortBy;
  searchForTickets();
};
</script>

<style lang="scss">
#ticketDuplicateTracker {
  --el-table-header-bg-color: var(--el-bg-color);
}
</style>

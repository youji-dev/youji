<template>
  <div>
    <el-card
      style="height: 275px"
      class="block drop-shadow-xl base-bg-light dark:base-bg-dark"
      v-resize-observer="onResize"
    >
      <el-empty
        v-if="ticketSearchResult.length === 0"
        style="height: 250px"
        :description="$t('noDuplicatesFound')"
      />
      <el-table
        v-else
        :data="ticketSearchResult"
        class="h-full w-full overflow-x-scroll"
        :fit="true"
        :height="tableDimensions.height"
        @row-dblclick="(row: any, column: any, event: Event) => {
        openInNewTab(localeRoute(`/tickets/${row.id}`)?.fullPath as string) }"
      >
        <el-table-column
          class="hidden lg:block"
          prop="title"
          filter-class-name="Title"
          :label="$t('title')"
          :width="tableDimensions.width / 6"
          show-overflow-tooltip
        />
        <el-table-column
          class="hidden lg:block"
          prop="state.name"
          :label="$t('status')"
          :width="tableDimensions.width / 6"
        >
          <template #default="scope">
            <div class="flex justify-start items-center">
              <ColoredSelect
                :options="
                  statusOptions.map((opt) => {
                    return new ColoredSelectOption(opt, opt.color);
                  })
                "
                :current="
                  new ColoredSelectOption(
                    scope.row.state,
                    scope.row.state.color
                  )
                "
                keyText="id"
                labelText="name"
                :changeCallback="() => {}"
                :readOnly="true"
              />
            </div>
          </template>
        </el-table-column>
        <el-table-column
          class="hidden lg:block"
          prop="building.name"
          :label="$t('building')"
          :width="tableDimensions.width / 6"
        />
        <el-table-column
          class="hidden lg:block"
          prop="room"
          :label="$t('room')"
          :width="tableDimensions.width / 6"
        />
        <el-table-column
          class="hidden lg:block"
          prop="priority.name"
          :label="$t('priority')"
          :width="tableDimensions.width / 6"
        >
          <template #default="scope">
            <div class="flex justify-start items-center">
              <ColoredSelect
                :options="
                  priorityOptions.map((opt) => {
                    return new ColoredSelectOption(opt, opt.color);
                  })
                "
                :current="
                  new ColoredSelectOption(
                    scope.row.priority,
                    scope.row.priority.color
                  )
                "
                keyText="id"
                labelText="name"
                :changeCallback="() => {}"
                :readOnly="true"
              />
            </div>
          </template>
        </el-table-column>
        <el-table-column
          class="hidden lg:block"
          prop="creationDate"
          :label="$t('createDate')"
          :width="tableDimensions.width / 6"
        >
          <template #default="scope">
            <p>{{ new Date(scope.row.creationDate).toLocaleString() }}</p>
          </template>
        </el-table-column>
        <el-table-column width="70" fixed="right">
          <template #default="scope">
            <el-button
              link
              type="primary"
              size="small"
              @click="openInNewTab(localeRoute(`/tickets/${scope.row.id}`)?.fullPath as string)"
            >
              <ElIconEdit class="w-5 mx-2"></ElIconEdit>
            </el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<script lang="tsx" setup>
import { vResizeObserver } from "@vueuse/components";
import ColoredSelect from "~/components/coloredSelect.vue";
import type ticket from "~/types/api/response/ticketResponse";
import { ColoredSelectOption } from "~/types/frontend/ColoredSelectOption";

const { $api } = useNuxtApp();
const { statusOptions, priorityOptions } = storeToRefs(useTicketsStore());
const { fetchStatusOptions, fetchPriorityOptions } = useTicketsStore();
const localeRoute = useLocaleRoute();

const props = defineProps<{
  ticket: ticket;
}>();

let ticketFilter: Record<string, any[]> = {};
const refreshTimeout: Ref<NodeJS.Timeout | null> = ref(null);
const ticketSearchResult = ref<ticket[]>([]);

const tableDimensions = ref({
  width: 0,
  height: 0,
});

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
    ticketFilter.Description = tokenize(props.ticket.description);
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
    "CreationDate",
    true,
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
  tableDimensions.value = { width: width - 100, height };
}

function openInNewTab(url: string) {
  const win = window.open(url, "_blank");
  if (win) {
    win.focus();
  }
}
</script>

<style lang="scss">
#ticketDuplicateTracker {
  --el-table-header-bg-color: var(--el-bg-color);
}
</style>

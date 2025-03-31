<template>
  <div
    class="w-full h-full py-6 lg:py-0 px-6 mt-6 lg:mt-0"
    style="height: calc(100vh - 72px)"
    :style="{ width: width }"
    :onresize="determineViewWidth()">
    <div class="flex flex-row items-center justify-end w-full py-2">
      <div class="w-full sm:w-1/2 lg:w-1/3 xl:w-1/4 flex justify-center items-center">
        <el-input
          v-model="search"
          class="w-full mr-1"
          :placeholder="$t('searchVerb')"
          :prefix-icon="Search"
          @change="fetchTicketsFromStart(true)" />
        <el-button
          class="ml-1"
          type="primary"
          :icon="ElIconSearch"
          :loading="searchLoading"
          round
          @click="fetchTicketsFromStart(true)" />
      </div>
    </div>
    <div
      class="w-full h-full overflow-y-scroll flex flex-col justify-center items-center base-bg-light dark:base-bg-dark rounded-md">
      <div
        v-if="loading"
        class="w-full h-full p-10">
        <el-skeleton
          :rows="18"
          animated />
      </div>
      <div
        v-if="pageLoading"
        class="w-full h-full p-10 flex justify-center items-center">
        <ElIconLoading class="animate-spin w-5" />
      </div>
      <!-- TODO : Somehow change the default element-plus sorting to comply with pagination. When any of the possible sorting arrows are clicked, the data has to be fetched again completely.
       The page should stay the same. -->
      <div
        v-if="tickets.length === 0 && !loading && !pageLoading"
        class="h-full w-full flex items-center justify-center">
        <el-empty :description="$t('nothingFound')" />
      </div>
      <el-table
        v-if="!loading && !pageLoading && tickets.length > 0"
        :data="parsedTickets"
        :height="tableDimensions['height']"
        class="h-full w-full overflow-x-scroll"
        :default-sort="{
          prop: sortColProp,
          order: sortDesc ? 'descending' : 'ascending',
        }"
        :sort-by="sortCol"
        @sort-change="changeSort"
        @row-dblclick="(row: any, column: any, event: Event) => {
          router.push(localeRoute(`/tickets/${row.id}`)?.fullPath as string)
        }">
        <el-table-column
          class="hidden lg:block"
          prop="author"
          filter-class-name="Author"
          :label="$t('createdBy')"
          width="150"
          show-overflow-tooltip
          sortable />
        <el-table-column
          prop="title"
          filter-class-name="Title"
          :label="$t('title')"
          min-width="250"
          show-overflow-tooltip
          sortable />
        <el-table-column
          prop="state.name"
          filter-class-name="State"
          :label="$t('status')"
          width="200"
          sortable>
          <template #default="scope">
            <div class="flex justify-start items-center">
              <ColoredSelect
                :id="scope.row.id"
                :change-callback="updateTicketState"
                :change-callback-params="[scope.row.id]"
                :add-current-value-to-callback="true"
                :current="new ColoredSelectOption(scope.row.state, scope.row.state.color)"
                :options="
                  statusOptions.map(opt => {
                    return new ColoredSelectOption(opt, opt.color);
                  })
                "
                :key-text="'id'"
                :label-text="'name'"
                :read-only="!canEditState(scope.row)" />
            </div>
          </template>
        </el-table-column>
        <el-table-column
          prop="building.name"
          filter-class-name="Building"
          class="hidden lg:block"
          :label="$t('building')"
          width="150"
          show-overflow-tooltip
          sortable />
        <el-table-column
          prop="room"
          filter-class-name="Room"
          class="hidden lg:block"
          :label="$t('room')"
          width="100"
          show-overflow-tooltip
          sortable />
        <el-table-column
          prop="priority.name"
          filter-class-name="Priority"
          :label="$t('priority')"
          width="200"
          sortable>
          <template #default="scope">
            <div class="flex justify-start items-center">
              <ColoredSelect
                :id="scope.row.id"
                :change-callback="updateTicketPriority"
                :change-callback-params="[scope.row.id]"
                :add-current-value-to-callback="true"
                :current="new ColoredSelectOption(scope.row.priority, scope.row.priority.color)"
                :options="
                  priorityOptionsSorted.map(opt => {
                    return new ColoredSelectOption(opt, opt.color);
                  })
                "
                :key-text="'id'"
                :label-text="'name'"
                :read-only="!canEditPriority(scope.row)" />
            </div>
          </template>
        </el-table-column>
        <el-table-column
          class="hidden lg:block"
          prop="creationDate"
          filter-class-name="CreationDate"
          :label="$t('createDate')"
          width="200"
          show-overflow-tooltip
          sortable />
        <el-table-column
          fixed="right"
          :min-width="isUserAdmin ? 120 : 70">
          <template #default="scope">
            <el-tooltip
              :content="$t('detail')"
              placement="top-start"
              :show-after="500">
              <el-button
                link
                type="primary"
                size="small"
                @click="router.push(localeRoute(`/tickets/${scope.row.id}`)?.fullPath as string)">
                <ElIconEdit class="w-5 mx-2" />
              </el-button>
            </el-tooltip>
            <el-tooltip
              v-if="isUserAdmin"
              :content="$t('delete')"
              placement="top-start"
              :show-after="500">
              <el-button
                link
                type="danger"
                size="small"
                @click="
                  () => {
                    deleteTicket = scope.row;
                    displayDeleteDialog = true;
                  }
                ">
                <ElIconDelete class="w-5 mx-2" />
              </el-button>
            </el-tooltip>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination
        v-if="!loading && tickets.length > 0"
        class="mr-auto"
        layout="prev, pager, next"
        :total="totalCount"
        :page-size="25"
        :current-page="pageNumber"
        @current-change="fetchNewPage" />
    </div>
    <TicketDeleteConfirmationDialog
      :ticket="deleteTicket"
      :visible="displayDeleteDialog"
      @closed="
        () => {
          displayDeleteDialog = false;
        }
      "
      @deleted="
        () => {
          fetchTicketsFromStart(true);
        }
      " />
  </div>
</template>

<script lang="tsx" setup>
  import { Search } from '@element-plus/icons-vue';
  import { ref } from 'vue';
  import ColoredSelect from '~/components/coloredSelect.vue';
  import type priority from '~/types/api/response/priorityResponse';
  import type state from '~/types/api/response/stateResponse';
  import type ticket from '~/types/api/response/ticketResponse';
  import { ColoredSelectOption } from '~/types/frontend/ColoredSelectOption';
  const search = ref('');
  const { statusOptions, priorityOptions, tickets, totalCount } = storeToRefs(useTicketsStore());
  const { isUserAdmin, isUserFacilityManager, username } = storeToRefs(useAuthStore());
  const { fetchStatusOptions, fetchPriorityOptions, fetchTickets } = useTicketsStore();
  const loading = ref(true);
  const pageLoading = ref(false);
  const pageNumber = ref(1);
  const searchLoading = ref(false);
  const sortCol = ref('CreationDate');
  const sortColProp = ref('creationDate');
  const sortDesc = ref(true);
  const localeRoute = useLocaleRoute();
  const router = useRouter();
  const i18n = useI18n();
  const width = ref('100vw');
  const { $api } = useNuxtApp();

  const displayDeleteDialog = ref(false);
  const deleteTicket: Ref<ticket | null> = ref(null);
  const tableDimensions = ref({
    width: 0,
    height: 0,
  });
  const priorityOptionsSorted = computed(() =>
    [...priorityOptions.value].sort((prioA: priority, prioB: priority) => prioB.value - prioA.value)
  );

  onMounted(async () => {
    getTableDimensions();
  });

  onNuxtReady(async () => {
    await fetchStatusOptions();
    await fetchPriorityOptions();
    await fetchTicketsFromStart(false);
    determineViewWidth();
    window.addEventListener('resize', determineViewWidth);
  });

  /**
   * Fetches the tickets from the API.
   * @param fromSearch Indicates if the tickets are fetched from a search.
   */
  async function fetchTicketsFromStart(fromSearch: boolean) {
    pageLoading.value = fromSearch;
    searchLoading.value = fromSearch;
    loading.value = !fromSearch;
    await fetchTickets({ Title: [search.value] }, 0, 25, sortCol.value, sortDesc.value);
    pageLoading.value = false;
    loading.value = false;
    searchLoading.value = false;
  }

  /**
   * Determines the view width of the table.
   */
  function determineViewWidth() {
    if (typeof document === 'undefined') return;
    const navbar = document.getElementById('navbar');
    if (typeof navbar === 'undefined') return;
    if (typeof navbar?.offsetWidth === 'undefined') return;
    width.value = window.innerWidth - navbar?.offsetWidth + 'px';
    const table = document.querySelector('el-table__inner-wrapper');
    if (!table) return;
    return;
  }

  const parsedTickets = computed(() => {
    return tickets.value.map(ticket => {
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
        attachments: ticket.attachments,
      };
    });
  });

  /**
   * Fetches the dimensions of the table.
   */
  function getTableDimensions() {
    const element = document.getElementById('table_container');
    if (element) {
      tableDimensions.value.width = element.offsetWidth;
      tableDimensions.value.height = element.offsetHeight;
      return;
    } else {
      tableDimensions.value.width = 1000;
      tableDimensions.value.height = 1000;
    }
  }

  /**
   * Updated the state of a ticket.
   * @param ticketId Id of the ticket to update.
   * @param newStatus New status of the ticket.
   */
  function updateTicketState(ticketId: string, newStatus: state) {
    const ticket: ticket[] = tickets.value.filter(ticket => ticket.id === ticketId);
    if (!ticket[0]) {
      const foundTicket: ticket = ticket[0];
      $api.ticket
        .edit({
          id: foundTicket.id,
          title: foundTicket.title,
          description: foundTicket.description,
          priorityId: foundTicket.priority.id,
          stateId: newStatus.id,
          buildingId: foundTicket.building?.id,
          room: foundTicket.room,
          object: foundTicket.object,
        })
        .then(resp => {
          if (resp.error.value) {
            console.error(resp.error.value);
            const selectEl: HTMLSelectElement | null = document.getElementById(ticketId) as HTMLSelectElement;
            selectEl.dispatchEvent(
              new Event('error', {
                bubbles: false,
                cancelable: true,
              })
            );
            if (resp.error.value.statusCode === 403) {
              ElMessage({
                message: i18n.t('forbidden'),
                type: 'error',
              });
            } else {
              ElMessage({
                message: i18n.t('savingFailed'),
                type: 'error',
              });
            }

            return;
          }
          ElMessage({
            message: i18n.t('updated'),
            type: 'success',
          });
        })
        .catch(() => {
          const selectEl: HTMLSelectElement | null = document.getElementById(ticketId) as HTMLSelectElement;
          selectEl.dispatchEvent(
            new Event('error', {
              bubbles: false,
              cancelable: true,
            })
          );
          ElMessage({
            message: i18n.t('savingFailed'),
            type: 'error',
          });
        });
    }
    return;
  }

  /**
   * Updated the priority of a ticket.
   * @param ticketId Id of the ticket to update.
   * @param newPriority New priority of the ticket.
   */
  function updateTicketPriority(ticketId: string, newPriority: priority) {
    const ticket: ticket[] = tickets.value.filter(ticket => ticket.id === ticketId);
    if (!ticket[0]) {
      const foundTicket: ticket = ticket[0];
      $api.ticket
        .edit({
          id: foundTicket.id,
          title: foundTicket.title,
          description: foundTicket.description,
          priorityId: newPriority.id,
          stateId: foundTicket.state.id,
          buildingId: foundTicket.building?.id,
          room: foundTicket.room,
          object: foundTicket.object,
        })
        .then(resp => {
          if (resp.error.value) {
            console.error(resp.error.value);
            const selectEl: HTMLSelectElement | null = document.getElementById(ticketId) as HTMLSelectElement;
            selectEl.dispatchEvent(
              new Event('error', {
                bubbles: false,
                cancelable: true,
              })
            );
            if (resp.error.value.statusCode === 403) {
              ElMessage({
                message: i18n.t('forbidden'),
                type: 'error',
              });
            } else {
              ElMessage({
                message: i18n.t('savingFailed'),
                type: 'error',
              });
            }

            return;
          }
          ElMessage({
            message: i18n.t('updated'),
            type: 'success',
          });
        })
        .catch(() => {
          const selectEl: HTMLSelectElement | null = document.getElementById(ticketId) as HTMLSelectElement;
          selectEl.dispatchEvent(
            new Event('error', {
              bubbles: false,
              cancelable: true,
            })
          );
          ElMessage({
            message: i18n.t('savingFailed'),
            type: 'error',
          });
        });
    }
    return;
  }

  /**
   * Sorts the tickets by the given column.
   * @param sortData - Instructions for sorting the tickets.
   * @param sortData.column - The column object containing sorting details.
   * @param sortData.prop - The property name of the column to sort by.
   * @param sortData.order - The order of sorting, either 'ascending' or 'descending'.
   */
  function changeSort(sortData: { column: any; prop: string; order: any }) {
    sortCol.value = sortData.column.filterClassName;
    sortColProp.value = sortData.prop;
    sortDesc.value = sortData.order === 'ascending' ? false : true;
    pageLoading.value = true;
    pageNumber.value = 1;
    fetchNewPage(1)
      .then(() => {
        pageLoading.value = false;
      })
      .catch(e => {
        pageLoading.value = false;
        console.error(e);
      });
  }

  /**
   * Paginates to a new page.
   * @param page The number of the page to paginate to.
   */
  async function fetchNewPage(page: number) {
    pageNumber.value = page;
    pageLoading.value = true;
    await fetchTickets({ Title: [search.value] }, page * 25 - 25, 25, sortCol.value, sortDesc.value);
    pageLoading.value = false;
  }

  /**
   * Verifies if the user can edit the state of a ticket.
   * @param ticket The ticket to check.
   * @returns True if the user can edit the state of the ticket, false otherwise.
   * @description A user can edit the state of a ticket if they are an admin, a facility manager,
   * or the author of the ticket. If hes is only the author, there must not be a state set as default.
   */
  function canEditState(ticket: ticket) {
    return (
      isUserAdmin.value ||
      isUserFacilityManager.value ||
      (ticket.author === username.value && !statusOptions.value.some(state => state.isDefault))
    );
  }

  /**
   * Verifies if the user can edit the priority of a ticket.
   * @param ticket The ticket to check.
   * @returns True if the user can edit the priority of the ticket, false otherwise.
   * @description A user can edit the priority of a ticket if they are an admin, a facility manager,
   * or the author of the ticket.
   */
  function canEditPriority(ticket: ticket) {
    return isUserAdmin.value || isUserFacilityManager.value || ticket.author === username.value;
  }
</script>

<style></style>

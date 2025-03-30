<template>
  <div>
    <el-form
      ref="ticketFormInstance"
      class="mt-20 lg:mt-5 max-h-full overflow-y-clip"
      :style="{ width: width }"
      label-position="top"
      :model="ticketModel"
      :rules="ticketFormRules"
      status-icon>
      <div
        v-if="!is404 && ticketModel"
        v-loading="loading"
        class="px-5 pb-3">
        <TicketHeader
          :ticket="ticketModel"
          class="lg:col-span-full" />
      </div>
      <div
        class="overflow-y-auto px-5 max-h-[84vh] md:max-h-[88vh]"
        :style="{ width: width }">
        <div
          v-if="!is404 && ticketModel"
          v-loading="loading"
          class="grid grid-cols-1 gap-3 auto-rows-min lg:grid-cols-[7fr_4fr]"
          :element-loading-text="loadingText">
          <div class="lg:col-start-1 lg:col-end-3 lg:row-start-2 lg:row-end-3">
            <el-form-item
              :label="$t('title')"
              prop="title">
              <el-input
                v-model="ticketModel.title"
                :placeholder="$t('enter')"
                class="drop-shadow-md dark:base-bg-dark" />
            </el-form-item>
          </div>

          <DropdownGroup
            :ticket="ticketModel"
            class="lg:col-start-2 lg:col-end-3 lg:row-start-3 lg:row-end-4 self-start" />

          <el-form-item
            class="lg:col-start-1 lg:col-end-2 lg:row-start-3 lg:row-end-4"
            :label="$t('description')"
            prop="description">
            <el-input
              v-model="ticketModel.description"
              type="textarea"
              class="drop-shadow-md max-h-full dark:base-bg-dark"
              :rows="13"
              resize="vertical"
              :placeholder="$t('enter')" />
          </el-form-item>

          <div
            v-if="!isNew"
            class="flex justify-around self-start lg:block lg:text-right lg:col-start-2 lg:col-end-3 lg:row-start-5 lg:row-end-6">
            <el-text class="w-1/2 truncate text-center">{{ $t('createdBy') }}: {{ ticketModel.author }}</el-text>
            <br />
            <el-text class="w-1/2 text-center"
              >{{ $t('createdOn') }}: {{ new Date(ticketModel.creationDate).toLocaleString() }}</el-text
            >
          </div>

          <TicketFiles
            v-if="!isNew"
            class="lg:col-start-2 lg:col-end-3 lg:row-start-4 lg:row-end-5"
            :ticket="ticketModel" />

          <TicketCommentCollection
            v-if="!isNew"
            v-model:ticket="ticketModel"
            class="self-start lg:col-start-1 lg:col-end-2 lg:row-start-4 lg:row-end-6 mb-3" />

          <div
            v-if="isNew"
            class="self-start lg:col-start-1 lg:col-end-3 lg:row-start-4 lg:row-end-6 mb-3">
            <el-text>{{ $t('duplicateTickets') }}</el-text>
            <TicketDuplicateTracker :ticket="ticketModel" />
          </div>
        </div>
        <div
          v-if="is404"
          class="h-screen flex justify-center items-center">
          <el-result
            class=""
            icon="error"
            :title="$t('resourceNotFound')">
            <template #extra>
              <el-button
                type="primary"
                @click="router.back()"
                >{{ $t('back') }}</el-button
              >
            </template>
          </el-result>
        </div>
        <div v-else-if="ticketModel == null">
          <TicketDetailTicketLoadingSkeleton />
        </div>
      </div>
      <div
        class="flex justify-between px-5 py-3 lg:col-span-full lg:row-start-6 lg:row-end-7 sticky bottom-0 page-bg-light dark:page-bg-dark">
        <el-tooltip
          :disabled="!isNew"
          :content="$t('pdfExportNotOnUnsaved')"
          placement="top-start">
          <el-button
            :disabled="isNew"
            class="text-sm drop-shadow-md"
            type="default"
            :icon="Printer"
            @click="exportToPDF()"
            >{{ $t('pdfExport') }}</el-button
          >
        </el-tooltip>

        <div class="flex">
          <el-button
            class="text-sm justify-self-end drop-shadow-md"
            type="danger"
            :hidden="isNew || !isUserAdmin"
            @click="deleteDialogVisible = true"
            >{{ $t('delete') }}</el-button
          >

          <el-button
            class="text-sm justify-self-end drop-shadow-md"
            type="default"
            @click="router.back()"
            >{{ $t('close') }}</el-button
          >

          <el-button
            class="text-sm justify-self-end drop-shadow-md"
            type="primary"
            @click="onSaveClick()"
            >{{ $t('save') }}</el-button
          >
        </div>
      </div>
    </el-form>
    <el-dialog v-model="imagePreviewDisplay">
      <img
        w-full
        :src="imagePreviewSrc"
        alt="Preview Image"
        class="w-full" />
    </el-dialog>
    <TicketDeleteConfirmationDialog
      :ticket="ticketModel"
      :visible="deleteDialogVisible"
      @closed="
        () => {
          deleteDialogVisible = false;
        }
      "
      @deleted="
        () => {
          router.push(localePath('/tickets')?.fullPath as string);
        }
      " />
  </div>
</template>

<script lang="ts" setup async>
  import { Printer } from '@element-plus/icons-vue';
  import type { FormInstance, FormRules } from 'element-plus';

  import type building from '~/types/api/response/buildingResponse';
  import type priority from '~/types/api/response/priorityResponse';
  import type state from '~/types/api/response/stateResponse';
  import type ticket from '~/types/api/response/ticketResponse';
  import type ticketAttachment from '~/types/api/response/ticketAttachmentResponse';
  import type ticketComment from '~/types/api/response/ticketCommentResponse';

  import { fromTicketResponse as editTicketFromResponse } from '~/types/api/request/editTicket';
  import TicketNotFoundError from '~/types/error/ticketNotFound';

  import DropdownGroup from '~/components/ticketDetail/ticketDropdown.vue';
  import TicketCommentCollection from '~/components/ticketDetail/ticketCommentCollection.vue';
  import TicketFiles from '~/components/ticketDetail/ticketFiles.vue';
  import TicketHeader from '~/components/ticketDetail/ticketHeader.vue';
  import TicketDuplicateTracker from '~/components/ticketDetail/ticketDuplicateTracker.vue';

  const { $api } = useNuxtApp();
  const { isUserAdmin, isUserFacilityManager } = storeToRefs(useAuthStore());
  const i18n = useI18n();
  const localePath = useLocaleRoute();

  const { imagePreviewDisplay, imagePreviewSrc } = storeToRefs(useImagePreviewDisplayStore());

  const router = useRouter();
  const route = useRoute();

  const width = ref('100vw');
  const isNew = ref((route.params.id as string).toLocaleLowerCase() == 'new');
  const is404 = ref(false);
  const loading = ref(true);
  const loadingText = ref(i18n.t('loadingData'));
  const deleteDialogVisible = ref(false);
  const availableStates: Ref<state[]> = ref([] as state[]);
  const availablePriorities: Ref<priority[]> = ref([] as priority[]);
  const availableBuildings: Ref<building[]> = ref([] as building[]);

  const ticketFormInstance = ref<FormInstance>();
  const ticketModel: Ref<ticket | null> = ref(null);
  const ticketFormRules = reactive<FormRules<ticket>>({
    title: [{ required: true, message: i18n.t('titleRequired'), trigger: 'blur' }],
    state: [
      { required: true, message: i18n.t('stateRequired'), trigger: 'blur' },
      {
        validator: (rule: any, value: any, callback: any) => {
          if (!canUserChangeStateCheck() && !ticketModel.value?.state.isDefault) {
            callback(new Error(i18n.t('defaultStateForced')));
          } else {
            callback();
          }
        },
      },
    ],
    priority: [{ required: true, message: i18n.t('priorityRequired'), trigger: 'blur' }],
  });

  onNuxtReady(async () => {
    await Promise.all([
      $api.state.getAll(),
      $api.priority.getAll(),
      $api.building.getAll(),
      fetchOrInitializeTicket(route.params.id as string),
    ])
      .then(([states, priorities, buildings, ticketData]) => {
        availableStates.value = states.data.value ?? [];
        availablePriorities.value = priorities.data.value ?? [];
        availableBuildings.value = buildings.data.value ?? [];
        ticketModel.value = ticketData;
        is404.value = false;

        const defaultState = availableStates.value.find(state => state.isDefault);
        const canUserChangeState = canUserChangeStateCheck();
        if (isNew.value && !canUserChangeState && defaultState) {
          ticketModel.value!.state = defaultState;
        }
      })
      .catch(error => {
        if (error instanceof TicketNotFoundError) {
          is404.value = true;
        } else {
          ElNotification({
            title: i18n.t('error'),
            message: (error as Error).message,
            type: 'error',
            duration: 5000,
          });
        }
      })
      .finally(() => (loading.value = false));

    determineViewWidth();
    window.addEventListener('resize', determineViewWidth);
  });

  /**
   * Checks if the user can change the state of the ticket.
   * @returns True if the user can change the state, false otherwise.
   * @description The user can change the state if they are an admin or facility manager, or if the state is not a default state.
   * If the state is a default state, the user cannot change it.
   */
  function canUserChangeStateCheck(): boolean {
    const defaultState = availableStates.value.find(state => state.isDefault);

    const isUserPrivileged = isUserAdmin.value || isUserFacilityManager.value;

    return isUserPrivileged || !defaultState;
  }

  /**
   * Fetches the ticket with the given ID or initializes a new ticket if the ID is 'new'.
   * @param id The ID of the ticket to fetch.
   * @returns The fetched or initialized ticket.
   * @throws TicketNotFoundError if the ticket is not found.
   */
  async function fetchOrInitializeTicket(id: string): Promise<ticket> {
    if (id === 'new') {
      const newTicket = {
        title: '',
        description: '',
        author: '',
        attachments: [] as ticketAttachment[],
        comments: [] as ticketComment[],
      } as ticket;

      return newTicket;
    }

    const ticketResult = await $api.ticket.get(id);

    if (ticketResult.error.value) {
      console.log(ticketResult.error);
      if (ticketResult.error.value.statusCode === 404) {
        throw new TicketNotFoundError('Ticket not found');
      }
      if (ticketResult.error.value.statusCode === 500) {
        throw new Error('serverError');
      }
      if (ticketResult.error.value.message) {
        throw new Error(ticketResult.error.value.message);
      }
      if (ticketResult.error.value.data) {
        throw new Error(ticketResult.error.value.data);
      } else {
        throw new Error(i18n.t('error'));
      }
    }
    const ticketData = ticketResult.data.value;

    if (ticketData === null) {
      return await fetchOrInitializeTicket('new');
    }
    return ticketData;
  }

  /**
   * Handles the click event of the save button.
   * Validates the form and either creates a new ticket or saves changes to an existing ticket.
   */
  async function onSaveClick() {
    if (!ticketFormInstance.value) return;
    await ticketFormInstance.value.validate(async isValid => {
      if (!isValid) {
        ElNotification({
          title: i18n.t('error'),
          message: i18n.t('formInvalid'),
          type: 'error',
          duration: 5000,
        });
        return;
      } else {
        if (isNew.value) await createTicket();
        else await saveTicketChanges();
      }
    });
  }

  /**
   * Saves changes to an existing ticket.
   */
  async function saveTicketChanges() {
    try {
      loadingText.value = i18n.t('savingTicket');
      loading.value = true;
      const ticketResult = await $api.ticket.edit(editTicketFromResponse(ticketModel.value!));

      if (ticketResult.error.value) {
        if (ticketResult.error.value.statusCode === 404) {
          throw new Error(i18n.t('resourceNotFound'));
        }
        if (ticketResult.error.value.statusCode === 403) {
          throw new Error(i18n.t('forbidden'));
        }
        if (ticketResult.error.value.statusCode === 500) {
          throw new Error('serverError');
        }
        if (ticketResult.error.value.message) {
          throw new Error(ticketResult.error.value.message);
        }
        if (ticketResult.error.value.data) {
          throw new Error(ticketResult.error.value.data);
        } else {
          throw new Error(i18n.t('error'));
        }
      } else {
        ElMessage({
          message: i18n.t('saved'),
          type: 'success',
          duration: 5000,
        });

        // no data is returned when nothing was changed
        if (ticketResult.data.value) {
          ticketModel.value = ticketResult.data.value;
        }
      }
    } catch (error) {
      ElNotification({
        title: i18n.t('error'),
        message: (error as Error).message,
        type: 'error',
        duration: 5000,
      });
    } finally {
      loading.value = false;
    }
  }

  /**
   * Creates a new ticket.
   */
  async function createTicket() {
    try {
      loadingText.value = i18n.t('creatingTicket');
      loading.value = true;
      const ticketResult = await $api.ticket.create({
        title: ticketModel.value!.title,
        description: ticketModel.value!.description ?? null,
        author: ticketModel.value!.author,
        stateId: ticketModel.value!.state.id,
        priorityId: ticketModel.value!.priority.id,
        buildingId: ticketModel.value!.building?.id ?? null,
        object: ticketModel.value!.object ?? null,
        room: ticketModel.value!.room ?? null,
      });

      if (ticketResult.error.value) {
        if (ticketResult.error.value.statusCode === 403) {
          throw new Error(i18n.t('forbidden'));
        }
        if (ticketResult.error.value.statusCode === 500) {
          throw new Error('serverError');
        }
        if (ticketResult.error.value.message) {
          throw new Error(ticketResult.error.value.message);
        }
        if (ticketResult.error.value.data) {
          throw new Error(ticketResult.error.value.data);
        } else {
          throw new Error(i18n.t('error'));
        }
      }

      if (ticketResult.data.value) {
        ElMessage({
          message: i18n.t('saved'),
          type: 'success',
          duration: 5000,
        });
        router.push(localePath('/tickets/' + ticketResult.data.value.id)?.fullPath as string);
      }
    } catch (error) {
      ElNotification({
        title: i18n.t('error'),
        message: (error as Error).message,
        type: 'error',
        duration: 5000,
      });
    } finally {
      loading.value = false;
    }
  }

  /**
   * Exports the ticket to a PDF file.
   */
  async function exportToPDF() {
    loadingText.value = i18n.t('creatingPDFExport');
    loading.value = true;
    await $api.ticket.exportToPDF(route.params.id as string, i18n.locale.value);
    loading.value = false;
  }

  /**
   * Determines the width of the view based on the navbar width.
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
</script>

import { defineStore } from 'pinia';
import type EditUserRequest from '~/types/api/request/editUser';
import type building from '~/types/api/response/buildingResponse';
import type priority from '~/types/api/response/priorityResponse';
import type state from '~/types/api/response/stateResponse';

export interface EditablePriority {
  id: EditableStringProperty;
  value: EditableNumberProperty;
  name: EditableStringProperty;
  color: EditableStringProperty;
  new?: boolean;
}
export interface EditableState {
  id: EditableStringProperty;
  name: EditableStringProperty;
  color: EditableStringProperty;
  hasAutoPurge: EditableBooleanProperty;
  autoPurgeDays: AutoPurgeDaysProperty;
  isDefault: EditableBooleanProperty;
  new?: boolean;
}
export interface EditableBuilding {
  id: EditableStringProperty;
  name: EditableStringProperty;
  new?: boolean;
}
export interface EditableUser {
  userId: EditableStringProperty;
  type: EditableStringProperty;
  email: EditableNullableStringProperty;
  preferredEmailLcid: EditableNullableStringProperty;
  allowsEmailNotifications: EditableBooleanProperty;
}

export type EditableProperty =
  | EditableStringProperty
  | EditableBooleanProperty
  | EditableNumberProperty
  | EditableNullableStringProperty;

export type EditableObject = EditablePriority | EditableState | EditableBuilding | EditableUser;

export interface EditableStringProperty {
  editing: boolean;
  value: string;
}

export interface EditableBooleanProperty {
  editing: boolean;
  value: boolean;
}

export interface EditableNumberProperty {
  editing: boolean;
  value: number;
}

export interface AutoPurgeDaysProperty {
  editing: boolean;
  value: number | '' | null;
}

export interface EditableNullableStringProperty {
  editing: boolean;
  value: string | null;
}

export const useSettingsStore = defineStore({
  id: 'settingsStore',
  state: () => ({
    priorities: [] as EditablePriority[],
    states: [] as EditableState[],
    buildings: [] as EditableBuilding[],
    users: [] as EditableUser[],
    preferredEmailLanguage: '' as string,
    prioritiesLoading: false as boolean,
    statesLoading: false as boolean,
    buildingsLoading: false as boolean,
    usersLoading: false as boolean,
    myUser: null as EditableUser | null,
    initialLoading: true as boolean,
  }),
  actions: {
    async fetchPriorities() {
      const { $api } = useNuxtApp();
      const resp = await $api.priority.getAll();
      if (resp.error.value) {
        console.log(resp.error);
      }
      if (!!resp.data.value) {
        this.priorities = resp.data.value
          .map(
            p =>
              ({
                id: { editing: false, value: p.id },
                color: { editing: false, value: p.color },
                name: { editing: false, value: p.name },
                value: { editing: false, value: p.value },
              }) as EditablePriority
          )
          .sort((a, b) => a.value.value - b.value.value);
      }
    },
    async fetchStates() {
      const { $api } = useNuxtApp();
      this.statesLoading = true;
      const resp = await $api.state.getAll();
      if (resp.error.value) {
        console.log(resp.error.value);
      }
      if (!!resp.data.value) {
        this.states = resp.data.value.map(s => ({
          id: { editing: false, value: s.id },
          name: { editing: false, value: s.name },
          color: { editing: false, value: s.color },
          hasAutoPurge: { editing: false, value: s.hasAutoPurge },
          autoPurgeDays: { editing: false, value: s.autoPurgeDays },
          isDefault: { editing: false, value: s.isDefault },
        }));
      }
      this.statesLoading = false;
    },
    async fetchBuildings() {
      const { $api } = useNuxtApp();
      this.buildingsLoading = true;
      const resp = await $api.building.getAll();
      if (resp.error.value) {
        console.log(resp.error.value);
      }
      if (!!resp.data.value) {
        this.buildings = resp.data.value.map(b => ({
          id: { editing: false, value: b.id },
          name: { editing: false, value: b.name },
        }));
      }
      this.buildingsLoading = false;
    },
    async fetchUsers() {
      const { $api } = useNuxtApp();
      this.usersLoading = true;
      const resp = await $api.user.getAll();
      if (resp.error.value) {
        console.log(resp.error.value);
      }
      if (!!resp.data.value) {
        this.users = resp.data.value.map(u => ({
          userId: { editing: false, value: u.userId },
          type: { editing: false, value: u.type.toString() },
          email: { editing: false, value: u.email },
          preferredEmailLcid: { editing: false, value: u.preferredEmailLcid },
          allowsEmailNotifications: { editing: false, value: u.allowsEmailNotifications },
        }));
      }
      this.usersLoading = false;
    },
    fetchMyUser() {
      const { username } = useAuthStore();
      return new Promise(resolve => {
        this.myUser = this.users.find(u => u.userId.value === username) ?? null;
        resolve(true);
      });
    },
    async updateUsers(updatedUser: EditableUser) {
      this.usersLoading = true;
      const { $api } = useNuxtApp();
      const { isUserAdmin, username } = useAuthStore();
      let userObj = {
        userId: updatedUser.userId.value,
        newPreferredEmailLcid: updatedUser.preferredEmailLcid.value,
      } as EditUserRequest;
      if (isUserAdmin) {
        userObj.newRole = Number(updatedUser.type.value);
      }
      let resp = await $api.user.edit(userObj);
      if (resp.error.value) {
        document.getElementById('globalsettings')?.dispatchEvent(new Event('updateFailed'));
      }
      await this.fetchUsers();
      this.usersLoading = false;
      if (username === updatedUser.userId.value) {
        document.getElementById('globalsettings')?.dispatchEvent(new Event('ownRoleChanged'));
      }
    },
    async updateBuildings(updatedBuilding: EditableBuilding, operation: 'C' | 'U' | 'D') {
      this.buildingsLoading = true;
      const { $api } = useNuxtApp();
      const buildingObj = {
        id: updatedBuilding.id.value,
        name: updatedBuilding.name.value,
      } as building;
      const resp =
        operation === 'C'
          ? await $api.building.create(buildingObj.name)
          : operation === 'U'
            ? await $api.building.edit(buildingObj)
            : await $api.building.delete(buildingObj.id);
      if (resp.error.value) {
        if (operation === 'D') {
          document.getElementById('globalsettings')?.dispatchEvent(new Event('objectIsReferenced'));
        } else {
          document.getElementById('globalsettings')?.dispatchEvent(new Event('updateFailed'));
        }
        console.log(resp.error.value);
      }
      if (operation !== 'C') {
        await this.fetchBuildings();
      } else {
        if (!resp.error.value) {
          this.buildings.forEach(b => {
            if (b.id.value === updatedBuilding.id.value) {
              b.id.value = (resp.data.value as building).id;
              b.new = undefined;
            }
          });
          document.getElementById('globalsettings')?.dispatchEvent(new Event('saved'));
        }
      }
      this.buildingsLoading = false;
    },
    async updateStates(updatedState: EditableState, operation: 'C' | 'U' | 'D', setDefaultState?: boolean) {
      this.statesLoading = true;
      const { $api } = useNuxtApp();
      const stateObj = {
        autoPurgeDays: updatedState.autoPurgeDays.value !== '' ? Number(updatedState.autoPurgeDays.value) : null,
        color: updatedState.color.value,
        hasAutoPurge: updatedState.hasAutoPurge.value,
        id: updatedState.id.value,
        isDefault: updatedState.isDefault.value,
        name: updatedState.name.value,
      } as state;
      const currentState = this.states.filter(s => stateObj.id === s.id.value)[0];
      if (
        (currentState.isDefault.value && stateObj.isDefault && operation === 'U' && !setDefaultState) ||
        (currentState.isDefault.value && operation === 'D')
      ) {
        document.getElementById('globalsettings')?.dispatchEvent(new Event('defaultStateNotUpdatable'));
        await this.fetchStates();
        this.statesLoading = false;
        return;
      }
      if (updatedState.isDefault.value) {
        for (let s of this.states) {
          if (s.isDefault.value && s.id.value !== updatedState.id.value) {
            document.getElementById('globalsettings')?.dispatchEvent(new Event('onlyOneDefaultState'));
            await this.fetchStates();
            this.statesLoading = false;
            return;
          }
        }
      }
      let resp =
        operation === 'C'
          ? await $api.state.create(stateObj)
          : operation === 'U'
            ? await $api.state.edit(stateObj)
            : await $api.state.delete(stateObj.id);
      if (resp.error.value) {
        if (operation === 'D') {
          document.getElementById('globalsettings')?.dispatchEvent(new Event('objectIsReferenced'));
        } else {
          document.getElementById('globalsettings')?.dispatchEvent(new Event('updateFailed'));
        }
        console.log(resp.error.value);
      }
      if (operation !== 'C') {
        await this.fetchStates();
      } else {
        if (!resp.error.value) {
          this.states.forEach(s => {
            if (s.id.value === updatedState.id.value) {
              s.id.value = (resp.data.value as state).id;
              s.new = undefined;
            }
          });
          document.getElementById('globalsettings')?.dispatchEvent(new Event('saved'));
        }
      }
      this.statesLoading = false;
    },

    async updatePriorities(updatedPriority: EditablePriority, operation: 'C' | 'U' | 'D') {
      this.prioritiesLoading = true;
      const { $api } = useNuxtApp();
      const priorityObj = {
        color: updatedPriority.color.value,
        id: updatedPriority.id.value,
        name: updatedPriority.name.value,
        value: updatedPriority.value.value,
      } as priority;
      const resp =
        operation === 'C'
          ? await $api.priority.create(priorityObj)
          : operation === 'U'
            ? await $api.priority.edit(priorityObj)
            : await $api.priority.delete(priorityObj.id);
      if (resp.error.value) {
        if (operation === 'D') {
          document.getElementById('globalsettings')?.dispatchEvent(new Event('objectIsReferenced'));
        } else {
          document.getElementById('globalsettings')?.dispatchEvent(new Event('updateFailed'));
        }
        console.log(resp.error.value);
      }
      if (operation !== 'C') {
        await this.fetchPriorities();
      } else {
        if (!resp.error.value) {
          this.priorities.forEach(p => {
            if (p.id.value === updatedPriority.id.value) {
              p.id.value = (resp.data.value as priority).id;
              p.new = undefined;
            }
          });
          document.getElementById('globalSettings')?.dispatchEvent(new Event('saved'));
        }
      }
      this.prioritiesLoading = false;
    },
    async updateMyUser(updatedUser: EditUserRequest) {
      this.usersLoading = true;
      const { $api } = useNuxtApp();
      let resp = await $api.user.edit(updatedUser);
      if (resp.error.value) {
        document.getElementById('userSettings')?.dispatchEvent(new Event('updateFailed'));
      }
      this.usersLoading = false;
    },
  },
});

<template>
  <div
    v-if="!initialLoading"
    class="grid grid-cols-1 lg:grid-cols-2"
    id="usersettings">
    <div class="flex flex-col">
      <div class="flex justify-between lg:justify-between items-center py-2">
        <h1 class="text-lg">{{ $t('theme') }}</h1>
        <div class="pl-3 flex items-center">
          <h1 class="text-sm px-3">
            {{
              colorMode.preference === 'dark'
                ? $t('dark')
                : colorMode.preference === 'light'
                  ? $t('light')
                  : $t('system')
            }}
          </h1>
          <Theme />
        </div>
      </div>
      <div class="flex justify-between lg:justify-between items-center py-2">
        <h1 class="text-lg">{{ $t('language') }}</h1>
        <div class="pl-3 flex items-center">
          <h1 class="text-sm px-3">
            {{ locale }}
          </h1>
          <Language />
        </div>
      </div>

      <el-divider />
      <div class="flex justify-between lg:justify-between items-center py-2">
        <h1
          :title="$t('receiveEmailNotifications')"
          class="text-lg overflow-hidden text-nowrap overflow-ellipsis w-[75%]">
          {{ $t('receiveEmailNotifications') }}
        </h1>
        <el-switch
          v-if="myUser !== null"
          v-model="myUser.allowsEmailNotifications.value"
          @change="toggleEmailNotifications()"></el-switch>
      </div>
      <div class="flex justify-between lg:justify-between items-center py-2">
        <h1 class="text-lg">{{ $t('languageEmail') }}</h1>
        <div class="pl-3 flex items-center">
          <h1 class="text-sm px-3">
            <span v-if="_emailLocale !== ''">{{ _emailLocale ? _emailLocale : $t('null') }}</span>
            <span v-else><ElIconLoading class="animate-spin w-3" /></span>
          </h1>
          <LanguageEmail />
        </div>
      </div>
    </div>
  </div>
  <div
    v-else
    class="w-full h-full flex items-center justify-center">
    <ElIconLoading class="animate-spin w-5" />
  </div>
</template>

<script lang="ts" setup>
  import type EditUserRequest from '~/types/api/request/editUser';
  import Language from '../language.vue';
  import LanguageEmail from '../languageEmail.vue';
  import Theme from '../theme.vue';
  const colorMode = useColorMode();
  const i18n = useI18n();
  const { locales } = useI18n();
  const { myUser, initialLoading } = storeToRefs(useSettingsStore());
  const { updateMyUser } = useSettingsStore();
  const locale = i18n.localeProperties.value.name;
  const _emailLocale = computed(() => {
    if (myUser.value === null) {
      return null;
    }
    if (myUser.value.preferredEmailLcid.value === null) {
      return null;
    } else {
      if (!locales.value) return '';
      return locales.value.filter(l => {
        if (l.code === myUser.value?.preferredEmailLcid.value) {
          return l.name;
        }
      })[0].name;
    }
  });

  async function toggleEmailNotifications() {
    if (myUser.value === null) return;
    const updatedUser = {
      userId: myUser.value?.userId.value,
      newAreEmailNotificationsAllowed: myUser.value.allowsEmailNotifications.value,
    } as EditUserRequest;
    await updateMyUser(updatedUser);
  }

  onNuxtReady(() => {
    document.getElementById('usersettings')?.addEventListener('updateFailed', () => {
      ElMessage({
        type: 'warning',
        message: i18n.t('updateFailed'),
      });
    });
  });
</script>

<style></style>

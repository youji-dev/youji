<template>
  <el-form
    :model="form"
    label-width="auto"
    class="w-full min-h-lvh flex items-center justify-center"
    @keyup.enter="promoteUserToAdmin()">
    <div
      class="w-full md:w-2/3 lg:w-2/5 xl:w-1/3 2xl:w-1/4 mx-3 flex flex-col text-left p-3 rounded-lg shadow-md base-bg-light dark:base-bg-dark">
      <h1 class="font-bold text-xl text-center">{{ $t('promotionBannerButton') }}</h1>
      <br />
      <h2 class="text-lg">{{ $t('promotionTokenExplanationTitle') }}</h2>
      <p>{{ $t('promotionTokenExplanationDescription') }}</p>
      <br />
      <h2 class="text-lg">{{ $t('promotionTokenWhereTitle') }}</h2>
      <p>{{ $t('promotionTokenWhereDescription') }}</p>
      <br />
      <el-form-item class="w-full mt-3">
        <el-input
          v-model="form.token"
          :placeholder="$t('promotion-token')" />
      </el-form-item>
      <div class="flex items-center justify-between w-full">
        <el-button
          :loading="loading"
          :loading-icon="Loading"
          type="primary"
          plain
          round
          class="ml-auto mr-10"
          @click="promoteUserToAdmin()"
          >{{ $t('save') }}</el-button
        >
      </div>
    </div>
  </el-form>
</template>

<script lang="ts" setup>
  import { Loading } from '@element-plus/icons-vue';
  import { useAuthStore } from '~/stores/auth';
  import { ElNotification } from 'element-plus';

  definePageMeta({
    layout: 'base',
  });

  const router = useRouter();
  const localePath = useLocaleRoute();
  const { promoteUser, logUserOut } = useAuthStore();
  const i18n = useI18n();

  const form = ref({
    token: '',
  });
  const loading = ref(false);

  /**
   * Starts promotion flow
   */
  async function promoteUserToAdmin() {
    loading.value = true;
    try {
      const token = form.value.token;
      const success = await promoteUser(token);
      if (success) {
        notifySuccess(i18n.t('promotionSuccess'));
        logUserOut();
        await router.push(localePath('/login')?.fullPath as string);
      } else {
        notifyError(i18n.t('promotionFailed'));
      }
    } catch (error) {
      console.error(error);
    } finally {
      loading.value = false;
    }
  }

  /**
   * Displays a success notification.
   * @param message The message to display.
   */
  function notifySuccess(message: string) {
    ElNotification({
      title: i18n.t('success'),
      message,
      type: 'success',
      duration: 3000,
    });
  }

  /**
   * Displays an error notification.
   * @param message The message to display.
   */
  function notifyError(message: string) {
    ElNotification({
      title: i18n.t('error'),
      message,
      type: 'error',
      duration: 5000,
    });
  }
</script>

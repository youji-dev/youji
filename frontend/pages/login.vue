<template>
  <component :is="Sidebar"></component>
  <el-form :model="form" label-width="auto" class="w-full h-lvh flex items-center justify-center">
    <div
      class="w-full md:w-2/3 lg:w-2/5 xl:w-1/3 2xl:w-1/4 mx-3 flex flex-col justify-center items-center py-3 rounded-lg shadow-md base-bg-light dark:base-bg-dark">
      <el-divider content-position="left" border-style="dashed">
        <el-text class="" size="large" type="">{{
          $t("ticketSystem")
        }}</el-text>
      </el-divider>
      <el-form-item class="w-full px-10 mt-3">
        <el-input v-model="form.username" :placeholder="$t('username')" :prefix-icon="User" Text />
      </el-form-item>
      <el-form-item class="w-full px-10">
        <el-input v-model="form.password" :placeholder="$t('password')" :prefix-icon="Lock" Password type="password"
          show-password>
        </el-input>
      </el-form-item>
      <div class="flex items-center justify-between w-full">
        <el-button :loading="loading" :loading-icon="Loading" type="primary" plain round @click="loginUser()"
          class="ml-auto mr-10">{{ $t("login") }}</el-button>
      </div>
    </div>
  </el-form>
</template>

<script lang="ts" setup>
import { Loading, Lock, User } from "@element-plus/icons-vue";
import { storeToRefs } from "pinia";
import { useAuthStore } from "~/stores/auth";
const { authenticateUser } = useAuthStore();
const { authenticated, authErrors } = storeToRefs(useAuthStore());
const i18n = useI18n();
import { ElNotification } from "element-plus";
import Sidebar from "~/components/sidebar.vue";
const localePath = useLocaleRoute();
const router = useRouter();

let form = ref({
  username: "",
  password: "",
});
const loading = ref(false);


async function loginUser() {
  loading.value = true;
  try {
    const { username, password } = form.value;
    const user = { name: username, password };
    await authenticateUser(user);
    if (authenticated.value) {
      notifySuccess(i18n.t("authSuccess"));
      router.push(localePath("/")?.fullPath as string);
    } else {
      const errors = authErrors.value;
      if (errors.length === 0) {
        notifyError(i18n.t("serverError"));
      } else {
        const firstError = errors[0];
        if (firstError === "wrong credentials") {
          notifyError(i18n.t("wrongCredentials"));
        } else {
          notifyError(firstError);
        }
      }
    }
  } catch (error) {
    console.error(error);
  } finally {
    loading.value = false;
  }
}

function notifySuccess(message: string) {
  ElNotification({
    title: i18n.t("success"),
    message,
    type: "success",
    duration: 3000,
  });
}

function notifyError(message: string) {
  ElNotification({
    title: i18n.t("error"),
    message,
    type: "error",
    duration: 5000,
  });
}
</script>

<style></style>

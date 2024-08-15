<template>
  <!-- <component :is="Sidebar"></component> -->
  <!-- Route: /login -->
  <!-- Page for login mask -->
  <el-form
    :model="form"
    label-width="auto"
    class="w-full h-lvh flex items-center justify-center"
  >
    <div
      class="w-full md:w-2/3 lg:w-2/5 xl:w-1/3 2xl:w-1/4 mx-3 flex flex-col justify-center items-center py-3 rounded-lg shadow-md base-bg-light dark:base-bg-dark"
    >
      <el-divider content-position="left" border-style="dashed">
        <el-text class="" size="large" type="">{{
          $t("ticketSystem")
        }}</el-text>
      </el-divider>
      <el-form-item class="w-full px-10 mt-3">
        <el-input
          v-model="form.username"
          :placeholder="$t('username')"
          :prefix-icon="User"
          Text
        />
      </el-form-item>
      <el-form-item class="w-full px-10">
        <el-input
          v-model="form.password"
          :placeholder="$t('password')"
          :prefix-icon="Lock"
          Password
          type="password"
          show-password
        >
        </el-input>
      </el-form-item>
      <div class="flex items-center justify-between w-full">
        <el-checkbox
          v-model="form.remember"
          :label="$t('rememberMe')"
          class="mr-auto ml-10"
          size="large"
          style="font-weight: 400;"
        />

        <el-button
          :loading="loading"
          :loading-icon="Loading"
          type="primary"
          plain
          round
          @click="login()"
          class="ml-auto mr-10"
          >{{ $t("login") }}</el-button
        >
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

let form = ref({
  username: "",
  password: "",
  remember: false,
});
const loading = ref(false);

async function login() {
  loading.value = true;
  try {
    const user = ref({
      name: form.value.username,
      password: form.value.password,
    });
    await authenticateUser(user.value);
    if (authenticated.value) {
      ElNotification({
        title: i18n.t("success"),
        message: i18n.t("authSuccess"),
        type: "success",
        duration: 3000,
      });
      loading.value = false;
      //router.push(localePath("/"));
    } else {
      loading.value = false;
      if (authErrors.value.length > 0) {
        ElNotification({
        title: i18n.t("error"),
        message: i18n.t("serverError"),
        type: "error",
        duration: 5000,
      });
      } else {
        ElNotification({
        title: i18n.t("error"),
        message: i18n.t("authError"),
        type: "error",
        duration: 5000,
      });
      }

    }
  } catch (error) {
    console.error(error);
  }
}
</script>

<style></style>

<template>
  <!-- Route: /login -->
  <!-- Page for login mask -->
  <el-form :model="form" label-width="auto" class="w-full h-lvh flex flex-col items-center justify-center">
    <el-form-item class="w-1/4">
      <el-input v-model="form.username" :placeholder="$t('username')" :prefix-icon="User" />
    </el-form-item>
    <el-form-item class="w-1/4">
      <el-input v-model="form.password" :placeholder="$t('password')" :prefix-icon="Lock" />
    </el-form-item>
    <el-button type="primary" plain @click="login()">{{ $t('login') }}</el-button>
  </el-form>
</template>

<script lang="ts" setup>
import { Lock, User } from '@element-plus/icons-vue';
let form = {
  username: null,
  password: null
};
function login() {
  ElMessage("Processing...");
}
</script>

<style></style>
<template>
  <div class="flex width-full justify-center bg-yellow-300 h-8 items-center">
    <p class="text-black">This system has no admin user. Click here to promote yourself to the first admin.</p>
    <el-button
      :size="'default'"
      class="ml-2"
      link
      type="danger"
      @click="router.push(localeRoute('/user-promotion')?.fullPath as string)"
      >Promote</el-button
    >
  </div>
</template>

<script setup lang="ts">
  const router = useRouter();
  const localeRoute = useLocaleRoute();
</script>

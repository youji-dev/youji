<template>
  <el-skeleton animated>
    <template #template>
      <div class="grid grid-cols-1 gap-3 auto-rows-min lg:grid-cols-[7fr_4fr]">
        <div class="flex lg:col-span-full">
          <el-button
            class="text-sm"
            link
            :icon="ArrowLeft"
            @click="router.back()"
            >{{ $t('back') }}</el-button
          >
          <el-divider
            class="self-center"
            direction="vertical" />
          <el-skeleton-item
            class="self-center"
            variant="h1" />
        </div>

        <div class="lg:col-start-1 lg:col-end-2 lg:row-start-2 lg:row-end-3">
          <el-skeleton-item
            variant="p"
            class="w-20" />
          <el-skeleton-item class="h-6" />
        </div>

        <div class="lg:col-start-2 lg:col-end-3 lg:row-start-2 lg:row-end-4 self-center">
          <div
            v-for="i in 4"
            :key="i">
            <el-skeleton-item
              variant="p"
              class="w-20" />
            <el-skeleton-item class="h-6" />
          </div>
        </div>

        <div class="lg:col-start-1 lg:col-end-2 lg:row-start-3 lg:row-end-4">
          <el-skeleton-item
            variant="p"
            class="w-20" />
          <el-skeleton-item class="h-60" />
        </div>

        <div
          class="flex justify-around self-start lg:block lg:text-right lg:col-start-2 lg:col-end-3 lg:row-start-5 lg:row-end-6">
          <el-skeleton-item class="w-20" />
          <br class="hidden lg:block" />
          <el-skeleton-item class="w-20" />
        </div>

        <div class="lg:col-start-2 lg:col-end-3 lg:row-start-4 lg:row-end-5">
          <el-skeleton-item
            variant="p"
            class="w-20" />
          <div class="flex">
            <el-skeleton-item
              v-for="i in 4"
              :key="i"
              variant="image"
              class="w-40 h-40 mr-4" />
          </div>
        </div>

        <div class="self-start lg:col-start-1 lg:col-end-2 lg:row-start-4 lg:row-end-6">
          <el-skeleton-item class="h-20" />
          <el-skeleton-item
            variant="button"
            class="h-5 w-20 mt-2 float-end" />

          <el-divider class="mt-10 mb-3" />

          <el-timeline>
            <el-timeline-item
              v-for="i in 2"
              :key="i"
              class="drop-shadow-xl"
              placement="top">
              <el-skeleton-item class="h-16 mt-2" />
            </el-timeline-item>
          </el-timeline>
        </div>

        <div class="flex justify-between lg:col-span-full lg:row-start-6 lg:row-end-7">
          <el-skeleton-item
            variant="button"
            class="h-7 w-40" />

          <div>
            <el-skeleton-item
              variant="button"
              class="h-7 w-20 mr-2" />
            <el-skeleton-item
              variant="button"
              class="h-7 w-20" />
          </div>
        </div>
      </div>
    </template>
  </el-skeleton>
</template>

<script lang="ts" setup>
  import { ArrowLeft } from '@element-plus/icons-vue';
  const router = useRouter();
</script>

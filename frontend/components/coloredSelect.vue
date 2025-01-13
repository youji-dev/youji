<template>
  <div v-if="changeCallback" class="flex flex-row justify-center items-center">
    <select
      v-model="selectedOption"
      :id="id"
      class="colored-select"
      @change="
        changeCallback(
          ...(changeCallbackParams ?? []),
          addCurrentValueToCallback
            ? !!valueKeyForCallback
              ? selectedOption.option[valueKeyForCallback]
              : selectedOption.option
            : null
        )
      "
    >
      <option
        v-for="option in options"
        :value="option"
        :key="keyText ? option.option[keyText] : option.option"
        class="text-neutral-900"
      >
        {{ labelText ? option.option[labelText] : option.option }}
      </option>
    </select>
    <span
      class="rounded-full mx-2 h-2 w-2"
      :style="{ 'background-color': selectedOption.color }"
      >&nbsp;</span
    >
  </div>
  <div v-else class="flex flex-row justify-center items-center">
    <select
      v-model="selectedOption"
      :id="id"
      class="colored-select"
    >
      <option
        v-for="option in options"
        :value="option"
        :key="keyText ? option.option[keyText] : option.option"
      >
        {{ labelText ? option.option[labelText] : option.option }}
      </option>
    </select>
    <span
      class="rounded-full mx-2 h-2 w-2"
      :style="{ 'background-color': selectedOption.color }"
      >&nbsp;</span
    >
  </div>
</template>

<script lang="ts" setup>
import { ColoredSelectOption } from "~/types/frontend/ColoredSelectOption";

const props = defineProps({
  id: {
    type: String,
    required: false,
  },
  options: {
    type: Array<ColoredSelectOption>,
    required: true,
  },
  current: {
    type: ColoredSelectOption,
    required: true,
  },
  keyText: {
    type: String,
    required: false,
  },
  labelText: {
    type: String,
    required: false,
  },
  changeCallback: {
    type: Function,
    required: false,
  },
  changeCallbackParams: {
    type: Array<any>,
    required: false,
  },
  addCurrentValueToCallback: {
    type: Boolean,
    required: false,
  },
  valueKeyForCallback: {
    type: String,
    required: false,
  },
});
const {
  id,
  options,
  current,
  changeCallback,
  changeCallbackParams,
  addCurrentValueToCallback = false,
  keyText,
  labelText,
  valueKeyForCallback = null,
} = props;

const selectedOption = ref(current);
onMounted(() => {
  document.getElementById(id ?? "")?.addEventListener("error", () => {
    selectedOption.value = current;
  });
});
</script>

<style>
.colored-select {
  @apply px-2 py-[0.15rem] rounded-md bg-[rgba(0,0,0,0.1)] dark:bg-[rgba(255,255,255,0.1)];
}
</style>

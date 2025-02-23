<template>
  <div class="flex flex-row justify-center items-center">
    <select
      v-model="selectedOption"
      v-if="!readOnly"
      :id="id"
      class="colored-select bg-[rgba(0,0,0,0.1)] dark:bg-[rgba(255,255,255,0.1)]"
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
    <p v-else>{{ selectedOption.option[labelText] }}</p>
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
    required: true,
  },
  changeCallback: {
    type: Function,
    required: false,
    default: () => {},
  },
  changeCallbackParams: {
    type: Array<any>,
    required: false,
  },
  addCurrentValueToCallback: {
    type: Boolean,
    required: false,
    default: false,
  },
  valueKeyForCallback: {
    type: String,
    required: false,
    default: null,
  },
  readOnly: {
    type: Boolean,
    required: false,
    default: false,
  },
});
const {
  id,
  options,
  current,
  changeCallback,
  changeCallbackParams,
  addCurrentValueToCallback,
  keyText,
  labelText,
  valueKeyForCallback = null,
  readOnly,
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

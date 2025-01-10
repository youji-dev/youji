<template>
  <div v-if="changeCallback" class="flex flex-row justify-center items-center">
    <select
      v-model="selectedOption"
      :id="id"
      class="colored-select bg-[rgba(0,0,0,0.1)] dark:bg-[rgba(255,255,255,0.1)]"
      @change="
        changeCallback(
          ...(changeCallBackParams ?? []),
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
        :label="labelText ? option.option[labelText] : option.option"
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
      class="colored-select bg-[rgba(0,0,0,0.1)] dark:bg-[rgba(255,255,255,0.1)]"
    >
      <option
        v-for="option in options"
        :value="option"
        :key="keyText ? option.option[keyText] : option.option"
        :label="labelText ? option.option[labelText] : option.option"
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
import { contrastColor } from "contrast-color";
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
  changeCallBackParams: {
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

const convertHexToRGBA = (hexCode: string, opacity = 1) => {
  let hex = hexCode.replace("#", "");

  if (hex.length === 3) {
    hex = `${hex[0]}${hex[0]}${hex[1]}${hex[1]}${hex[2]}${hex[2]}`;
  }

  const r = parseInt(hex.substring(0, 2), 16);
  const g = parseInt(hex.substring(2, 4), 16);
  const b = parseInt(hex.substring(4, 6), 16);

  if (opacity > 1 && opacity <= 100) {
    opacity = opacity / 100;
  }

  return `rgba(${r},${g},${b},${opacity})`;
};
</script>

<style>
.colored-select {
  @apply px-2 py-[0.15rem] rounded-md;
}
</style>

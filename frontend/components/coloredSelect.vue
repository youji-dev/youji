<template>
    <select v-if="changeCallback" v-model="current" class="colored-select"
        :style="{ border: 'solid 0.5px ' + color, 'background-color': convertHexToRGBA(color, 0.35), color: contrastColor({bgColor: color})}"
        @change="changeCallback(changeCallBackParams, addCurrentValueToCallback ? current : null)">
        <option v-for="option in options" :style="{color: contrastColor({bgColor: color}) }" :value="option" :key="keyText ? option[keyText] : option" :label="labelText ? option[labelText] : option">{{ labelText ? option[labelText] : option }}</option>
    </select>
    <select v-else v-model="current"
        :style="{ border: 'solid 1px ' + color, 'background-color': convertHexToRGBA(color, 0.35), color: contrastColor({bgColor: color}) }" class="colored-select">
        <option v-for="option in options" :style="{color: contrastColor({bgColor: color})}" :value="keyText ? option[keyText] : option" :key="keyText ? option[keyText] : option" :label="labelText ? option[labelText] : option">{{ labelText ? option[labelText] : option }}</option>
    </select>
</template>

<script lang="ts" setup>
import { contrastColor } from 'contrast-color';

// const colorMode = useColorMode();

const props = defineProps({
    color: {
        type: String,
        required: true
    },
    options: {
        type: Array<any>,
        required: true
    },
    current: {
        type: null,
        required: true
    },
    keyText: {
        type: String,
        required: false
    },
    labelText: {
        type: String,
        required: false
    },
    changeCallback: {
        type: Function,
        required: false
    },
    changeCallBackParams: {
        type: Array<any>,
        required: false
    },
    addCurrentValueToCallback: {
        type: Boolean,
        required: false
    }
});
const { color, options, current, changeCallback, changeCallBackParams, addCurrentValueToCallback = false, keyText, labelText } = props;

const convertHexToRGBA = (hexCode : string, opacity = 1) => {  
    let hex = hexCode.replace('#', '');
    
    if (hex.length === 3) {
        hex = `${hex[0]}${hex[0]}${hex[1]}${hex[1]}${hex[2]}${hex[2]}`;
    }    
    
    const r = parseInt(hex.substring(0, 2), 16);
    const g = parseInt(hex.substring(2, 4), 16);
    const b = parseInt(hex.substring(4, 6), 16);
    
    /* Backward compatibility for whole number based opacity values. */
    if (opacity > 1 && opacity <= 100) {
        opacity = opacity / 100;   
    }

    return `rgba(${r},${g},${b},${opacity})`;
};
</script>

<style>
.colored-select {
    @apply px-2 py-[0.15rem] rounded-md
}
</style>
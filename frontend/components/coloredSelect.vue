<template>
    <select v-if="changeCallback" v-model="current" class="colored-select"
        :style="{ border: 'solid 1px ' + color, 'background-color': hexToRgbAHalfOpacity(color) }"
        @change="changeCallback(changeCallBackParams)">
        <option v-for="option in options" :value="option" :key="option" :label="option">{{ option }}</option>
    </select>
    <select v-else v-model="current"
        :style="{ border: 'solid 1px ' + color, 'background-color': hexToRgbAHalfOpacity(color) }" class="colored-select">
        <option v-for="option in options" :value="option" :key="option" :label="option">{{ option }}</option>
    </select>
</template>

<script lang="ts" setup>

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
        type: String,
        required: true
    },
    changeCallback: {
        type: Function,
        required: false
    },
    changeCallBackParams: {
        type: Array<any>,
        required: false
    }
});
const { color, options, current, changeCallback, changeCallBackParams } = props;

function hexToRgbAHalfOpacity(hex: string) {
    let c: any;
    console.log(hex)
    if (/^#([A-Fa-f0-9]{3}){1,2}$/.test(hex)) {
        c = hex.substring(1).split('');
        if (c.length == 3) {
            c = [c[0], c[0], c[1], c[1], c[2], c[2]];
        }
        c = '0x' + c.join('');
        return 'rgba(' + [(c >> 16) & 255, (c >> 8) & 255, c & 255].join(',') + ',0.5)';
    }
    throw new Error('Bad Hex');
}
</script>

<style>
.colored-select {
    @apply px-4 py-2
}
</style>
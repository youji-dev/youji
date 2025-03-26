<template>
  <el-table-column
    :min-width="minWidth"
    :label="label"
    :prop="prop + '[value]'">
    <template #default="{ row }">
      <span
        v-if="row[prop].editing && !disabled"
        class="w-full flex items-center justify-between">
        <el-input
          v-model="row[prop].value"
          :placeholder="$t('empty')"
          :type="inputType ? inputType : 'text'"
          @blur="
            row[prop].editing = !row[prop].editing;
            !row.new && saveCallback(row, 'U');
          "
          @vue:mounted="focusInput" />
        <el-icon-check
          class="w-4 mx-1"
          @click="
            row[prop].editing = false;
            !row.new && saveCallback(row, 'U');
          " />
      </span>
      <h1
        v-else
        class="w-full flex justify-between items-center">
        {{ row[prop].value !== '' && row[prop].value !== null ? row[prop].value : $t('empty') }}
        <span
          ><el-icon-edit-pen
            v-if="!disabled"
            class="w-3"
            @click="row[prop].editing = true"
        /></span>
      </h1>
    </template>
  </el-table-column>
</template>

<script lang="ts" setup>
  // This component is to be used as a replacement for <el-table-column/> elements in order to make them editable

  const props = defineProps({
    // The property which should be accessed. Has to be an object like {editing: boolean, value: any}
    prop: {
      type: String,
      required: true,
    },
    // The label of the column
    label: {
      type: String,
      required: true,
    },
    // The function to be called after a cell was edited
    saveCallback: {
      type: Function,
      required: true,
    },
    inputType: {
      type: String,
      required: false,
      default: '',
    },
    disabled: {
      type: Boolean,
      required: false,
    },
    minWidth: {
      type: String,
      required: false,
      default: '',
    },
  });

  const focusInput = (event: any) => {
    (event['el'] as HTMLElement).querySelector('input')?.focus();
  };

  const toggleEdit = (row: any) => {
    row;
  };

  const { prop, label, saveCallback, inputType, disabled, minWidth } = props;
</script>
<script lang="ts">
  // Use this function as a callback in your table element.
  // Every row (dataset) should have an "id" property.
  // Every property should be represented by a {editing: boolean, value: any} object.
  // If a row should be recognized as "new" so that it will pass the "C" (Create) operation to the save callback, please set the "new" property on your new row object to "true".
  // If your table data is in this format, this function will determine the corresponding object in your dataset based on the clicked row and column and set its "editing" value.

  /**
   * Callback function for the el-table element
   * @param row row that the callback should be executed on
   * @param col column that t he callback should be executed on
   * @param data the data array that contains the row object
   */
  export function handleCellClick(row: any, col: any, data: any) {
    const cellObj = data.filter((obj: any) => obj.id.value === row.id.value)[0][col.property.split('[')[0]];
    if (cellObj.editing !== 'undefined') {
      cellObj.editing = !cellObj.editing;
    }
  }
</script>

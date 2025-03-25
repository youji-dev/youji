import { defineStore } from 'pinia';

export const useImagePreviewDisplayStore = defineStore({
  id: 'ImagePreviewDisplayStore',
  state: () => ({
    imagePreviewDisplay: false,
    imagePreviewSrc: '',
  }),
  actions: {
    setImagePreviewDisplay(imagePreviewDisplay: boolean) {
      this.imagePreviewDisplay = imagePreviewDisplay;
    },
    setImagePreviewSrc(imagePreviewSrc: string) {
      this.imagePreviewSrc = imagePreviewSrc;
    },

    clearData() {
      this.imagePreviewDisplay = false;
      this.imagePreviewSrc = '';
    },
  },
});

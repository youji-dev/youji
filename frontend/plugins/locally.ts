export default defineNuxtPlugin(() => {
  return {
    provide: {
      locally: {
        getItem(item: any) {
          if (import.meta.client) {
            return localStorage.getItem(item);
          } else {
            return undefined;
          }
        },

        setItem(item: any, value: any) {
          if (import.meta.client) {
            return localStorage.setItem(item, value);
          }
        },
      },
    },
  };
});

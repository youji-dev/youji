export default defineNuxtPlugin(() => {
  return {
    provide: {
      locally: {
        getItem(item: string) {
          if (import.meta.client) {
            return localStorage.getItem(item);
          } else {
            return undefined;
          }
        },

        setItem(item: string, value: string) {
          if (import.meta.client) {
            return localStorage.setItem(item, value);
          }
        },
      },
    },
  };
});

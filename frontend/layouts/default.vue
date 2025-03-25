<template>
  <div class="page-bg-light dark:page-bg-dark w-full h-lvh max-h-lvh flex overflow-x-hidden overflow-y-scroll">
    <Sidebar
      class="flex-none"
      :openTicketsCount="openTickets" />
    <SidebarMobile
      class="flex-none"
      :openTicketsCount="openTickets" />
    <slot />
  </div>
</template>

<script lang="ts" setup>
  import SidebarMobile from '~/components/sidebar-mobile.vue';
  import Sidebar from '~/components/sidebar.vue';

  const { $api } = useNuxtApp();
  const { statusOptions } = storeToRefs(useTicketsStore());
  const { fetchStatusOptions } = useTicketsStore();

  const openTickets = ref<number | null>(null);

  onNuxtReady(async () => {
    await fetchStatusOptions();
    openTickets.value = await getOpenTicketCount();
  });

  async function getOpenTicketCount(): Promise<number | null> {
    const filter: Record<string, any[]> = {};

    if (statusOptions.value.some(state => state.hasAutoPurge)) {
      filter.State = statusOptions.value.filter(x => !x.hasAutoPurge).map(x => x.id);
    }

    var result = await $api.ticket.search(filter, 'CreationDate', false, 0, 0, true);

    if (result.data.value == null) return null;

    return result.data.value.total;
  }
</script>

<style>
  html,
  body {
    margin: 0;
    padding: 0;
  }
  @tailwind base;
  @tailwind components;
  @tailwind utilities;

  @layer components {
    .page-bg-dark {
      background-color: #0a0a0a;
    }

    .page-bg-light {
      background-color: #f2f3f5;
    }

    .base-bg-dark {
      background-color: #141414;
    }

    .base-bg-light {
      background-color: #ffffff;
    }

    .text-dark {
      color: #e5eaf3;
    }

    .text-light {
      color: #303133;
    }

    .logo-dark {
      background-color: #e5eaf3;
    }

    .logo-light {
      background-color: #303133;
    }
  }

  ::-webkit-scrollbar {
    width: 5px;
    z-index: 50;
  }

  /* Track */
  ::-webkit-scrollbar-track {
    background: transparent;
    z-index: 50;
  }

  /* Handle */
  ::-webkit-scrollbar-thumb {
    background: rgba(0, 0, 0, 0.3);
    border-radius: 2rem;
    z-index: 50;
  }

  /* Handle on hover */
  ::-webkit-scrollbar-thumb:hover {
    background: #777;
  }
</style>

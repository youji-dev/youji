import { defineStore } from "pinia";
import type CreateAdvancedTicketSearchRequest from "~/types/api/request/advancedTicketSearch";
import type priority from "~/types/api/response/priorityResponse";
import type state from "~/types/api/response/stateResponse";
import type ticket from "~/types/api/response/ticketResponse";
export const useTicketsStore = defineStore("tickets", {
    state: () => ({
        statusOptions: [] as Array<state>,
        tickets: [] as Array<ticket>,
        totalCount: 0 as number,
    }),
    actions: {
        async fetchStatusOptions() {
            const { $api } = useNuxtApp();
            const resp = await $api.state.getAll();
            if (resp.error) {
                console.log(resp.error);
            }
            if (!!resp.data.value) {
                this.statusOptions = resp.data.value ;
            }
        },

        async fetchTickets(search: string, skip: number, take: number, orderByCol: string, orderDesc: boolean) {
            const { $api } = useNuxtApp();
            const resp = await $api.ticket.search(search, orderByCol, orderDesc, skip, take);
            console.log("ticket response", resp.data.value);
            if (resp.error.value) {
                console.log(resp.error)
                return;
            } 
            if (!!resp.data.value){
                this.totalCount = resp.data.value.total;
                this.tickets = resp.data.value.results;
            }
        },

        async fetchTicketsAdvanced(options: CreateAdvancedTicketSearchRequest) {
            console.log(options);
            const { $api } = useNuxtApp();
            const resp = await $api.ticket.advancedSearch(options);
            console.log("ticket response", resp.data.value);
            if (resp.error.value) {
                console.log(resp.error)
                return;
            }
            if (!!resp.data.value) {
                this.totalCount = resp.data.value.total;
                this.tickets = resp.data.value.results;
            }
        },
        async fetchNewTickets() {
            const { $api } = useNuxtApp();
        }
    }
})
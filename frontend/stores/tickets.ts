import { defineStore } from "pinia";

export type Status = {
    text: string,
    color: string
}

export const useTicketsStore = defineStore("tickets", {
    state: () => ({
        statusOptions: {} as Array<Status>,
    }),
    actions: {
        async fetchStatusOptions() {
            // Fetch status options from backend
            this.statusOptions = [
                {
                    text: 'Neu',
                    color: 'primary'
                },
                {
                    text: 'In Arbeit',
                    color: "warning"
                },
                {
                    text: "Abgeschlossen",
                    color: "success"
                }
            ]
        }
    }
})
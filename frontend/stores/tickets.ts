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
                    color: '#e01d41'
                },
                {
                    text: 'In Arbeit',
                    color: "#ffc43b"
                },
                {
                    text: "Abgeschlossen",
                    color: "#62c451"
                }
            ]
        }
    }
})
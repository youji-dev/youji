import { defineStore } from 'pinia'

// In this store we can define actions for authenticating the user at the backend and store variables like the state of the authentication request, errors, user information ...
// All of these actions and variables can be used and called in our vue files.    

export const useMyAuthStore = defineStore({
  id: 'myAuthStore',
  state: () => ({ 
    // Variables
  }),
  actions: {
    // Actions
  }
})

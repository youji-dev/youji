import StateRepository from "~/repositories/state";
import TicketRepository from "~/repositories/ticket";

interface IApiInstance {
  ticket: TicketRepository;
  state: StateRepository;
}

export default defineNuxtPlugin(nuxtApp => {
  const modules: IApiInstance = {
    ticket: new TicketRepository(),
    state: new StateRepository(),
  };

  return {
    provide: {
      api: modules,
    },
  };
});

import TicketRepository from "~/repositories/modules/ticket";

interface IApiInstance {
  ticket: TicketRepository;
}

export default defineNuxtPlugin(nuxtApp => {
  const modules: IApiInstance = {
    ticket: new TicketRepository(),
  };

  return {
    provide: {
      api: modules,
    },
  };
});

import TicketRepository from "~/repositories/ticket";
import StateRepository from "~/repositories/state";
import PriorityRepository from "~/repositories/priority";
import BuildingRepository from "~/repositories/building";

interface IApiInstance {
  ticket: TicketRepository;
  state: StateRepository;
  priority: PriorityRepository;
  building: BuildingRepository;
}

export default defineNuxtPlugin(nuxtApp => {
  const modules: IApiInstance = {
    ticket: new TicketRepository(),
    state: new StateRepository(),
    priority: new PriorityRepository(),
    building: new BuildingRepository(),
  };

  return {
    provide: {
      api: modules,
    },
  };
});

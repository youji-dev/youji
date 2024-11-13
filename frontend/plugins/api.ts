import TicketRepository from "~/repositories/ticket";
import StateRepository from "~/repositories/state";
import PriorityRepository from "~/repositories/priority";
import BuildingRepository from "~/repositories/building";
import CommentRepository from "~/repositories/comment";
import AttachmentRepository from "~/repositories/attachment";

interface IApiInstance {
  ticket: TicketRepository;
  state: StateRepository;
  priority: PriorityRepository;
  building: BuildingRepository;
  comment: CommentRepository;
  attachment: AttachmentRepository;
}

export default defineNuxtPlugin(() => {
  const modules: IApiInstance = {
    ticket: new TicketRepository(),
    state: new StateRepository(),
    priority: new PriorityRepository(),
    building: new BuildingRepository(),
    comment: new CommentRepository(),
    attachment: new AttachmentRepository(),
  };

  return {
    provide: {
      api: modules,
    },
  };
});

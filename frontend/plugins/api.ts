import TicketRepository from "~/repositories/ticket";
import StateRepository from "~/repositories/state";
import PriorityRepository from "~/repositories/priority";
import BuildingRepository from "~/repositories/building";
import CommentRepository from "~/repositories/comment";
import AttachmentRepository from "~/repositories/attachment";
import UserRepository from "~/repositories/user";

interface IApiInstance {
  ticket: TicketRepository;
  state: StateRepository;
  priority: PriorityRepository;
  building: BuildingRepository;
  comment: CommentRepository;
  attachment: AttachmentRepository;
  user: UserRepository;
}

export default defineNuxtPlugin(() => {
  const modules: IApiInstance = {
    ticket: new TicketRepository(),
    state: new StateRepository(),
    priority: new PriorityRepository(),
    building: new BuildingRepository(),
    comment: new CommentRepository(),
    attachment: new AttachmentRepository(),
    user: new UserRepository(),
  };

  return {
    provide: {
      api: modules,
    },
  };
});

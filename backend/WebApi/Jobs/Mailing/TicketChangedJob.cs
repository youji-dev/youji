using DomainLayer.BusinessLogic.Mailing;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;
using Quartz;

namespace Application.WebApi.Jobs.Mailing
{
    [PersistJobDataAfterExecution]
    public class TicketChangedJob(TicketRepository ticketRepository, MailingService mailingService) : JobBase, IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Ticket[]? lastRunState = this.GetJobData<Ticket[]>(context, "last_run_state");
            DateTimeOffset? lastFireTime = context.Trigger.GetPreviousFireTimeUtc();

            if (lastRunState is not null && lastFireTime is not null)
            {
                Ticket[] changedTickets = [.. ticketRepository.Find(t => t.LastChangedDate >= lastFireTime)];
                foreach (var ticket in changedTickets)
                {
                    Ticket? oldVersion = lastRunState.FirstOrDefault(t => t.Id == ticket.Id);
                    if (oldVersion == null)

                    this.HandleChangedTicket(ticket, );
                }
            }

            Ticket[] currentState = [.. ticketRepository.GetAll()];
            this.SetJobData(context, "last_run_state", currentState);
        }

        public void HandleChangedTicket(Ticket newVersion, Ticket oldVersion)
        {

        }
    }
}

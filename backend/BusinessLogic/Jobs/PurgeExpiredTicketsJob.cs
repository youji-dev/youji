using PersistenceLayer.DataAccess.Repositories;
using Quartz;

namespace DomainLayer.BusinessLogic.Jobs
{
    /// <summary>
    /// Represents a job that deletes expired tickets after a given time.
    /// </summary>
    /// <param name="ticketRepo">Instance of <see cref="TicketRepository"/></param>
    public class PurgeExpiredTicketsJob(TicketRepository ticketRepo) : IJob
    {
        /// <inheritdoc/>
        public async Task Execute(IJobExecutionContext context)
        {
            var relevantTickets = ticketRepo.GetAll().Where(ticket => ticket.State.HasAutoPurge);
            var expiredRelevantTickets = relevantTickets.Where(ticket => ticket.State.AutoPurgeDays != null
                    && DateTime.UtcNow.AddDays(-(int)ticket.State.AutoPurgeDays) > ticket.LastStateUpdate).ToArray();

            foreach (var ticket in expiredRelevantTickets)
            {
                await ticketRepo.DeleteAsync(ticket);
            }

            return;
        }
    }
}

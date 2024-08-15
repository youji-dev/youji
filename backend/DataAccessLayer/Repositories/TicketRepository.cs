﻿using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// Represents the repository of the ticket entity.
    /// </summary>
    /// <param name="context">Instance of <see cref="DataContext"/></param>
    public class TicketRepository(DataContext context) : Repository<Ticket, Guid>(context)
    {
    }
}

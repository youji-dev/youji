﻿using PersistenceLayer.DataAccess.Entities;

namespace PersistenceLayer.DataAccess.Repositories
{
    public class TicketRepository : Repository<Ticket, Guid>
    {
        public TicketRepository(DataContext context)
            : base(context)
        {
        }
    }
}

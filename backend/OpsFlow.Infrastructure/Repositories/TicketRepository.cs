

using Microsoft.EntityFrameworkCore;
using OpsFlow.Application.Interfaces;
using OpsFlow.Domain.Entities;
using OpsFlow.Infrastructure.Database;

namespace OpsFlow.Infrastructure.Repositories
{
    public class TicketRepository:ITicketRepository
    {

        private readonly OpsFlowDbContext _opsFlowDbContext;

        public TicketRepository(OpsFlowDbContext opsFlowDbContext)
        {
            _opsFlowDbContext = opsFlowDbContext;
        }

        public async Task<Ticket?> GetByIdAsync(int TicketId)
        {
            return await _opsFlowDbContext.Tickets.FindAsync(TicketId);
        }


        public async Task<List<Ticket>> GetAllAsync()
        {
            return await _opsFlowDbContext.Tickets.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Ticket ticket)
        {
             await _opsFlowDbContext.Tickets.AddAsync(ticket);
        }

        public async Task SaveChangesAsync()
        {
            await _opsFlowDbContext.SaveChangesAsync();
        }

    }
}

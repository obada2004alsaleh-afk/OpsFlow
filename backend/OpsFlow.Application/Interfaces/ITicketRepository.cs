

using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(int ticketId);
        Task<List<Ticket>> GetAllAsync();
        Task AddAsync(Ticket ticket);
        Task SaveChangesAsync();
    }
}

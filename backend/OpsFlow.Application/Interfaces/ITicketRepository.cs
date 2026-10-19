
using AssignmentEntity = OpsFlow.Domain.Entities.Assignment;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(int ticketId);
        Task<List<Ticket>> GetAllAsync();
        Task AddAsync(Ticket ticket);
        Task SaveChangesAsync();
        Task<List<Ticket>> GetByCustomerIdAsync(int customerId);
        Task SaveStatusChangeAsync(
             Ticket ticket,
              TicketStatusHistory history,
              AssignmentEntity? assignmentToDeactivate);
    }
}

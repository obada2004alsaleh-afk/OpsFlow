
using AssignmentEntity = OpsFlow.Domain.Entities.Assignment;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Interfaces
{
    public  interface IAssignmentRepository
    {
        public Task AddAssignmentAsync(AssignmentEntity assignment);

        public Task<AssignmentEntity?> GetActiveByTicketIdAsync(int id);

        Task SaveAssignmentAsync(
      AssignmentEntity? activeAssignment,
      AssignmentEntity newAssignment,
      Ticket ticket,
      TicketStatusHistory? statusHistory);

        public Task SaveChangesAsync();
    }
}

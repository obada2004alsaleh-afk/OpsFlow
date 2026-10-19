

using Microsoft.EntityFrameworkCore;
using OpsFlow.Application.Interfaces;
using OpsFlow.Domain.Entities;
using OpsFlow.Infrastructure.Database;

namespace OpsFlow.Infrastructure.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly OpsFlowDbContext _opsFlowDbContext;
        public AssignmentRepository(OpsFlowDbContext opsFlowDbContext)
        {
            _opsFlowDbContext = opsFlowDbContext;
        }

        public async Task<Assignment?> GetActiveByTicketIdAsync(int ticketId)
        {
            return await _opsFlowDbContext.Assignments
                .Include(a => a.TechnicianProfile)
                .FirstOrDefaultAsync(a =>
                    a.TicketId == ticketId && a.IsActive);
        }

        public async Task AddAssignmentAsync(Assignment assignment)
        {
            await _opsFlowDbContext.Assignments.AddAsync(assignment);
        }

        public async Task SaveAssignmentAsync(
      Assignment? activeAssignment,
      Assignment newAssignment,
      Ticket ticket,
      TicketStatusHistory? statusHistory)
        {
            await using var transaction =
                await _opsFlowDbContext.Database.BeginTransactionAsync();

            if (activeAssignment != null)
            {
                // The old assignment was deactivated in the UseCase.
                await _opsFlowDbContext.SaveChangesAsync();
            }

            await _opsFlowDbContext.Assignments.AddAsync(newAssignment);

            if (statusHistory != null)
            {
                ticket.Status = statusHistory.ToStatus;

                await _opsFlowDbContext.TicketStatusHistories
                    .AddAsync(statusHistory);
            }

            await _opsFlowDbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _opsFlowDbContext.SaveChangesAsync();
        }

    }
}

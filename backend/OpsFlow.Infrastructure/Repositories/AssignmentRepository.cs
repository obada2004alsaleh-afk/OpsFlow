

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
            var response = await _opsFlowDbContext.Assignments.FirstOrDefaultAsync(u => u.TicketId == ticketId && u.IsActive);
            return response;
        }

        public async Task AddAssignmentAsync(Assignment assignment)
        {
            await _opsFlowDbContext.Assignments.AddAsync(assignment);
        }

        public async Task SaveAssignmentAsync(Assignment? activeAssignment, Assignment newAssignment)
        {
            await using var transaction =
            await _opsFlowDbContext.Database.BeginTransactionAsync();

            if (activeAssignment != null)
            {
                await _opsFlowDbContext.SaveChangesAsync();
            }



            await _opsFlowDbContext.Assignments.AddAsync(newAssignment);
            await _opsFlowDbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _opsFlowDbContext.SaveChangesAsync();
        }

    }
}

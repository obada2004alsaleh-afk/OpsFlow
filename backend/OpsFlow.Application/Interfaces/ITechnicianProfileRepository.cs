
using OpsFlow.Application.TechnicianProfileRepository.AssignTicket;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Interfaces
{
    public interface ITechnicianProfileRepository
    {
         Task AddAsync(TechnicianProfile technicianProfile);
         Task<TechnicianProfile?> GetByIdAsync(int id);
         Task <List<TechnicianProfile>> GetAllAsync();
         Task SaveChangesAsync();
    }
}

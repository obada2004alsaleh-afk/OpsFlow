
using Microsoft.EntityFrameworkCore;
using OpsFlow.Application.Interfaces;
using OpsFlow.Domain.Entities;
using OpsFlow.Infrastructure.Database;

namespace OpsFlow.Infrastructure.Repositories
{
    public class TechnicianProfileRepository: ITechnicianProfileRepository
    {

        private readonly OpsFlowDbContext _context;

        public TechnicianProfileRepository(OpsFlowDbContext context)
        {
            _context = context;
        }


        public async Task<TechnicianProfile?> GetByIdAsync(int technicianProfileId)
        {
            return await _context.TechnicianProfiles.Include(u => u.User).FirstOrDefaultAsync(U => U.TechnicalId == technicianProfileId);

        }


        public async Task<List<TechnicianProfile>> GetAllAsync()
        {
            return await _context.TechnicianProfiles.Include(t => t.User).AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(TechnicianProfile technicianProfile)
        {
            await _context.TechnicianProfiles.AddAsync(technicianProfile);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

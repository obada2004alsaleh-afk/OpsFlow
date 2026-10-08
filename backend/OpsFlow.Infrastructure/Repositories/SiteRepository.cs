
using Microsoft.EntityFrameworkCore;
using OpsFlow.Application.Interfaces;
using OpsFlow.Domain.Entities;
using OpsFlow.Infrastructure.Database;

namespace OpsFlow.Infrastructure.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly OpsFlowDbContext _context;

        public SiteRepository(OpsFlowDbContext context)
        {
            _context = context;
        }


        public async Task<Site?> GetByIdAsync(int siteId)
        {
            return await _context.Sites.FindAsync(siteId);

        }


        public async Task<List<Site>> GetAllAsync()
        {
            return await _context.Sites.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Site site)
        {
            await _context.Sites.AddAsync(site);
        }

        public async Task SaveChangesAsync()
        {
             await _context.SaveChangesAsync();
        }

    }
}

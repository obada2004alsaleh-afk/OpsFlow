using OpsFlow.Application.Interfaces;
using OpsFlow.Domain.Entities;
using OpsFlow.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace OpsFlow.Infrastructure.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly OpsFlowDbContext _dbContext;

        public AssetRepository(OpsFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Asset asset)
        {
             await _dbContext.Assets.AddAsync(asset);
        }

        public async Task<List<Asset>> GetAllAsync()
        {
            return await _dbContext.Assets.AsNoTracking().ToListAsync();
        }

        public async Task<Asset?> GetByIdAsync(int assetId)
        {
            return await _dbContext.Assets.FindAsync(assetId);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }



        }
}

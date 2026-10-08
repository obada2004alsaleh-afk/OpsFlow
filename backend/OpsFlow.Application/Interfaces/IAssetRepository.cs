

using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Interfaces
{
    public interface IAssetRepository
    {

        Task<Asset?> GetByIdAsync(int assetId);
        Task<List<Asset>> GetAllAsync();
        Task AddAsync(Asset asset);

        Task SaveChangesAsync();
    }
}

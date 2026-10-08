

using OpsFlow.Application.Assets.Response;
using OpsFlow.Application.Interfaces;

namespace OpsFlow.Application.Assets.GetAssetById
{
    public class GetAssetByIdUseCase
    {
        private readonly IAssetRepository _assetRepository;

        public GetAssetByIdUseCase(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<AssetResponse> ExecuteAsync(int assetId)
        {
            var asset = await _assetRepository.GetByIdAsync(assetId);
            if (asset == null)
            {
                throw new KeyNotFoundException($"Asset with ID {assetId} not found.");
            }
            return new AssetResponse
            {
                AssetId = asset.AssetId,
                Name = asset.Name,
                SiteId = asset.SiteId,
                CreatedAt = asset.CreatedAt,
                IsActive = asset.IsActive
            };
        }

    }
}

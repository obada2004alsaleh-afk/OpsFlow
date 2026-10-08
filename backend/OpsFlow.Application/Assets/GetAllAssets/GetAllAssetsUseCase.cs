

using OpsFlow.Application.Assets.Response;
using OpsFlow.Application.Interfaces;

namespace OpsFlow.Application.Assets.GetAllAssets
{
    public class GetAllAssetsUseCase
    {

        private readonly IAssetRepository _assetRepository;
        public GetAllAssetsUseCase(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }
        public async Task<List<AssetResponse>> ExecuteAsync()
        {
            var assets = await _assetRepository.GetAllAsync();
            return assets.Select(asset => new AssetResponse
            {
                AssetId = asset.AssetId,
                Name = asset.Name,
                SiteId = asset.SiteId,
                CreatedAt = asset.CreatedAt,
                IsActive = asset.IsActive
            }).ToList();
        }
    }
}

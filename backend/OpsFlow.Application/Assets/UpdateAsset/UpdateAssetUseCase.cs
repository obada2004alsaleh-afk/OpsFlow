using OpsFlow.Application.Assets.Response;
using OpsFlow.Application.Interfaces;


namespace OpsFlow.Application.Assets.UpdateAsset
{
    public class UpdateAssetUseCase
    {
        private readonly IAssetRepository _assetRepository;

        public UpdateAssetUseCase(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<AssetResponse> ExecuteAsync(int AssetId,UpdateAssetRequest updateAssetRequest)
        {
            var Asset = await _assetRepository.GetByIdAsync(AssetId);

            if(Asset == null)
            {
                throw new KeyNotFoundException($"Asset with ID {AssetId} not found.");
            }

            if (updateAssetRequest.Name != null && string.IsNullOrWhiteSpace(updateAssetRequest.Name))
            {
                throw new ArgumentException("Asset name cannot be empty.");
            }

            if (updateAssetRequest.Name != null)
            {
                Asset.Name = updateAssetRequest.Name;
            }




            if (updateAssetRequest.IsActive.HasValue)
            {
                Asset.IsActive = updateAssetRequest.IsActive.Value;
            }

            await _assetRepository.SaveChangesAsync();

            return new AssetResponse
            {
                AssetId = Asset.AssetId,
                Name = Asset.Name,
                SiteId = Asset.SiteId,
                CreatedAt = Asset.CreatedAt,
                IsActive = Asset.IsActive
            };

        }

    }
}

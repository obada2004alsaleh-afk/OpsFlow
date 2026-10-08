

using OpsFlow.Application.Assets.Response;
using OpsFlow.Application.Interfaces;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Assets.CreateAsset
{
    public class CreateAssetUseCase
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ISiteRepository _siteRepository;

        public CreateAssetUseCase(
            IAssetRepository assetRepository,
            ISiteRepository siteRepository)
        {
            _assetRepository = assetRepository;
            _siteRepository = siteRepository;
        }


        public async Task<AssetResponse> ExecuteAsync(CreateAssetRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Asset name cannot be empty.");
            }

            if (request.SiteId <= 0)
            {
                throw new ArgumentException("Site ID must be greater than zero.");
            }

            var site = await _siteRepository.GetByIdAsync(request.SiteId);

            if(site == null)
            {
                throw new KeyNotFoundException($"Site with ID {request.SiteId} not found.");
            }

            if(site.IsActive == false)
            {
                throw new InvalidOperationException($"Site with ID {request.SiteId} is not active.");
            }

            var asset =new Asset
            {
                Name = request.Name,
                SiteId = request.SiteId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _assetRepository.AddAsync(asset);
            await _assetRepository.SaveChangesAsync();
            
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

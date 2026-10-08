


using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Sites.Response;

namespace OpsFlow.Application.Sites.UpdateSite
{
    public class UpdateSiteUseCase
    {

        private readonly ISiteRepository _siteRepository;

        public UpdateSiteUseCase(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public async Task<SiteResponse> ExecuteAsync(int siteId, UpdateSiteRequest request)
        {
            var site = await _siteRepository.GetByIdAsync(siteId);
           
            if (site == null)
            {
                throw new KeyNotFoundException($"Site with ID {siteId} not found.");
            }
            if (request.Name != null && string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Site name cannot be empty.");
            }

            if (request.Address != null && string.IsNullOrWhiteSpace(request.Address))
            {
                throw new ArgumentException("Site address cannot be empty.");
            }
            if (request.Name != null)
            {
                site.Name = request.Name;
            }

            if (request.Email != null)
            {
                site.Email = request.Email;
            }

            if (request.Phone != null)
            {
                site.Phone = request.Phone;
            }

            if (request.Address != null)
            {
                site.Address = request.Address;
            }



            if (request.IsActive.HasValue)
            {
                site.IsActive = request.IsActive.Value;
            }

            await _siteRepository.SaveChangesAsync();
            return new SiteResponse
            {
                SiteId = site.SiteId,
                Name = site.Name,
                Email = site.Email,
                Phone = site.Phone,
                Address = site.Address,
                IsActive = site.IsActive,
                CustomerId = site.CustomerId,
                CreatedAt = site.CreatedAt
            };
        }

    }
}



using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Sites.Response;

namespace OpsFlow.Application.Sites.GetById
{
    public class GetBySiteIdUseCase
    {

        private readonly ISiteRepository _siteRepository;

        public GetBySiteIdUseCase(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }


        public async Task<SiteResponse> ExecuteAsync(int siteId)
        {
            var site = await _siteRepository.GetByIdAsync(siteId);
            if (site == null)
            {
                throw new KeyNotFoundException($"Site with ID {siteId} not found.");
            }

            return new SiteResponse
            {
                SiteId = site.SiteId,
                Name = site.Name,
                Email = site.Email,
                Phone = site.Phone,
                Address = site.Address,
                CustomerId = site.CustomerId,
                CreatedAt = site.CreatedAt,
                IsActive = site.IsActive
            };

        }
    }
}

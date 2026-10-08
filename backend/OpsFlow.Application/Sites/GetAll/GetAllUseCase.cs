using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Sites.Response;


namespace OpsFlow.Application.Sites.GetAll
{
    public class GetAllUseCase
    {

        private readonly ISiteRepository _siteRepository;

        public GetAllUseCase(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }


        public async Task<List<SiteResponse>> ExecuteAsync()
        {
            var sites = await _siteRepository.GetAllAsync();
          
            return sites.Select(site => new SiteResponse
            {
                SiteId = site.SiteId,
                Name = site.Name,
                Email = site.Email,
                Phone = site.Phone,
                Address = site.Address,
                CustomerId = site.CustomerId,
                CreatedAt = site.CreatedAt,
                IsActive = site.IsActive
            }).ToList();
        }

    }
}

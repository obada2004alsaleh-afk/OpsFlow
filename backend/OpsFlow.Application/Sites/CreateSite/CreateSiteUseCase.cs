using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Sites.Response;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Sites.CreateSite
{
    public class CreateSiteUseCase
    {

        private readonly ISiteRepository _siteRepository;
        private readonly ICustomerRepository _customerRepository;
        public CreateSiteUseCase(ISiteRepository siteRepository, ICustomerRepository customerRepository)
        {
            _siteRepository = siteRepository;
            _customerRepository = customerRepository;
        }


        public async Task<SiteResponse> ExecuteAsync(CreateSiteRequest request)
        {

            if (request.CustomerId <= 0 )
            {
                throw new ArgumentException("CustomerId must be greater than zero.");
            }
            
            var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId);

            if(customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {request.CustomerId} not found.");
            }

            if(customer.IsActive == false)
            {
                throw new InvalidOperationException($"Customer with ID {request.CustomerId} is not active.");
            }

            var newSite = new Site
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                CustomerId = request.CustomerId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _siteRepository.AddAsync(newSite);
            await _siteRepository.SaveChangesAsync();

            return new SiteResponse
            {
                SiteId = newSite.SiteId,
                Name = newSite.Name,
                Email = newSite.Email,
                Phone = newSite.Phone,
                Address = newSite.Address,
                CustomerId = newSite.CustomerId,
                CreatedAt = newSite.CreatedAt,
                IsActive = newSite.IsActive



            };


        }
    }
}

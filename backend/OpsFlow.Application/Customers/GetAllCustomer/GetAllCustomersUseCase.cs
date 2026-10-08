


using OpsFlow.Application.Customers.Response;
using OpsFlow.Application.Interfaces;

namespace OpsFlow.Application.Customers.GetAllCustomers
{
    public class GetAllCustomersUseCase
    {

        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomersUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<CustomerResponse>> ExecuteAsync()
        {
          var customers=  await _customerRepository.GetAllAsync();
           
            return customers.Select(c => new CustomerResponse
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                CreatedAt = c.CreatedAt,
                IsActive = c.IsActive
            }).ToList();
        }


    }
}

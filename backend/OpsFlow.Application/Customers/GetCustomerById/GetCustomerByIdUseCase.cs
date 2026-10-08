

using OpsFlow.Application.Customers.Response;
using OpsFlow.Application.Interfaces;

namespace OpsFlow.Application.Customers.GetCustomerById
{
    public class GetCustomerByIdUseCase
    {

        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerResponse> ExecuteAsync(int CustomerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(CustomerId);

            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found");
            }

            return new CustomerResponse
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                CreatedAt = customer.CreatedAt,
                IsActive = customer.IsActive
            };
        }
    }
}

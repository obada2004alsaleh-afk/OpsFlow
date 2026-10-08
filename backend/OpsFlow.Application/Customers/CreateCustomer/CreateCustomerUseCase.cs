using OpsFlow.Application.Customers.Response;
using OpsFlow.Application.Interfaces;
using OpsFlow.Domain.Entities;


namespace OpsFlow.Application.Customers.CreateCustomer
{
    public class CreateCustomerUseCase
    {

        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerResponse> ExecuteAsync(CreateCustomerRequest request)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(request.Email);

            if(customer != null)
            {
                throw new InvalidOperationException(
                 "Customer with the same email already exists.");
            }

            var newCustomer = new Customer
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _customerRepository.AddAsync(newCustomer);
            await _customerRepository.SaveChangesAsync();

            return new CustomerResponse
            {
                CustomerId = newCustomer.CustomerId,
                Name = newCustomer.Name,
                Email = newCustomer.Email,
                Phone = newCustomer.Phone,
                CreatedAt = newCustomer.CreatedAt,
                IsActive = newCustomer.IsActive
            };
        }
    }
}

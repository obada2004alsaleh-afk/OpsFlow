using OpsFlow.Application.Customers.Response;
using OpsFlow.Application.Interfaces;

namespace OpsFlow.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerUseCase
    {

        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }


       public async Task<CustomerResponse> ExecuteAsync(int customerId,UpdateCustomerRequest updateCustomer)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);

            if(customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
            }

            if (updateCustomer.Email != null && updateCustomer.Email != customer.Email)
            {
                var existingCustomer =
                    await _customerRepository.GetCustomerByEmailAsync(updateCustomer.Email);

                if (existingCustomer != null &&
                 existingCustomer.CustomerId != customerId)
                {
                    throw new InvalidOperationException(
                        "Customer with the same email already exists.");
                }
            
            }

            if (updateCustomer.Email != null)
                customer.Email = updateCustomer.Email;

            if (updateCustomer.Name != null)
            {
                customer.Name = updateCustomer.Name;
            }

            if (updateCustomer.Phone != null)
            {
                customer.Phone = updateCustomer.Phone;
            }

            if (updateCustomer.IsActive.HasValue)
            {
                customer.IsActive = updateCustomer.IsActive.Value;
            }

            await _customerRepository.SaveChangesAsync();
            
            return new CustomerResponse
            {
                CustomerId = customer.CustomerId,
                Email = customer.Email,
                Phone = customer.Phone,
                CreatedAt = customer.CreatedAt,
                Name = customer.Name,
                IsActive = customer.IsActive
            };

        }
    }
}



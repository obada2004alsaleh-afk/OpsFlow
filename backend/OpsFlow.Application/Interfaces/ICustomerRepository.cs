

using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Interfaces
{
    public interface ICustomerRepository
    {

        Task<Customer?> GetCustomerByIdAsync(int customerId);

        Task<List<Customer>> GetAllAsync();

        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task AddAsync(Customer customer);

        Task SaveChangesAsync();


    }
}

using OpsFlow.Domain.Entities;
using OpsFlow.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using OpsFlow.Application.Interfaces;

namespace OpsFlow.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly OpsFlowDbContext _dbContext;

        public CustomerRepository(OpsFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            return await _dbContext.Customers.FindAsync(customerId);
        }


        public async Task<List<Customer>> GetAllAsync()
        {
            return await _dbContext.Customers.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
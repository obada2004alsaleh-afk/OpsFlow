using OpsFlow.Application.Interfaces;
using OpsFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpsFlow.Infrastructure.Database;

namespace OpsFlow.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OpsFlowDbContext _dbContext;

        public UserRepository(OpsFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
           return await  _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user); // Add the user to the DbSet
        }
         public async Task<List<User>> GetAllAsync()
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetByUserIdAsync(int UserId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == UserId);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync(); // Save changes to the database
        }

    }
}

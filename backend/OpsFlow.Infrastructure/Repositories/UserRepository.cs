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
    }
}

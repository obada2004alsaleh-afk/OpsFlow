using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task AddAsync(User user);

        Task<List<User>> GetAllAsync();
        Task SaveChangesAsync();

        Task<User?> GetByUserIdAsync(int UserId);


    }
}

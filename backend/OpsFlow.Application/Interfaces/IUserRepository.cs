using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

    }
}

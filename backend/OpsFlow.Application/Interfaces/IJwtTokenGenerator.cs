

using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);

    }
}
